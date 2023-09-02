// <copyright file="ExecutionEngine.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Execution;

using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebNativeDEV.SINUS.Core.ArgumentValidation;
using WebNativeDEV.SINUS.Core.Execution.Contracts;
using WebNativeDEV.SINUS.Core.FluentAPI.Contracts;
using WebNativeDEV.SINUS.Core.Logging;
using WebNativeDEV.SINUS.Core.Sut;
using WebNativeDEV.SINUS.MsTest;

/// <summary>
/// The execution engine.
/// </summary>
public sealed class ExecutionEngine : IExecutionEngine
{
    /// <summary>
    /// Random Endpoint placeholder/template that will be used later for string replacement.
    /// </summary>
    public const string RandomEndpoint = "https://localhost:" + RandomEndpointPlaceholder;

    private const string RandomEndpointPlaceholder = "{RANDOMENDPOINT}";
    private const int RetryCountCreatingSut = 10;
    private const int RetryDelay = 60;
    private static readonly object LockerSutStatic = new();
    private readonly ILogger<ExecutionEngine> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExecutionEngine"/> class.
    /// </summary>
    /// <param name="loggerFactory">A factory to create a logger.</param>
    public ExecutionEngine(ILoggerFactory loggerFactory)
    {
        this.logger = loggerFactory.CreateLogger<ExecutionEngine>();
        this.logger.LogDebug("Created a log for execution engine");
    }

    /// <inheritdoc/>
    public ExecutionOutput Run(ExecutionParameter? parameter)
    {
        parameter = Ensure.NotNull(parameter, nameof(parameter));

        ExecutionOutput returnValue = new()
        {
            IsPreparedOnly = parameter.Actions == null || !parameter.Actions.Any(action => action != null),
            ShouldRunIfAlreadyExceptionOccured = parameter.RunCategory switch
            {
                RunCategory.Given => false,
                RunCategory.When => false,
                RunCategory.Then => true,
                RunCategory.Debug => true,
                RunCategory.Dispose => true,
                _ => throw new InvalidDataException("this list should completely be handled"),
            },
        };

        if (parameter.CreateSut)
        {
            lock (LockerSutStatic)
            {
                this.CreateSut(parameter, returnValue);
            }
        }

        this.RunActions(parameter, returnValue);

        return returnValue;
    }

    private void RunActions(ExecutionParameter parameter, ExecutionOutput returnValue)
    {
        parameter = Ensure.NotNull(parameter);
        var namings = Ensure.NotNull(parameter.Namings);

        List<Action> actions = new();
        actions.AddRange(
            parameter
                .SetupActions
                ?.Select<Action<ExecutionSetupParameters>?, Action>(
                    action =>
                        () => action?.Invoke(new ExecutionSetupParameters()
                        {
                            Endpoint = returnValue.SutEndpoint,
                        })) ?? Array.Empty<Action>());

        actions.AddRange(
            parameter
                .Actions
                ?.Cast<Action>() ?? Array.Empty<Action>());

        var actionCount = actions.Count;

        if (!parameter.RunActions || actionCount == 0)
        {
            string skipDescription = namings.GetReadableDescription(
                parameter.RunCategory,
                parameter.Description,
                1,
                1);
            PerformanceDataScope.WriteSkip(this.logger, parameter.RunCategory.ToString(), skipDescription, actions.Count);
            return;
        }

        for (int i = 0; i < actionCount; i++)
        {
            var action = actions[i];
            string description = namings.GetReadableDescription(
                parameter.RunCategory,
                parameter.Description,
                i,
                actionCount);

            using (new PerformanceDataScope(this.logger, parameter.RunCategory.ToString(), description))
            {
#pragma warning disable CA1031 // do not catch general exception types

                try
                {
                    if (parameter.ExceptionsCount == 0 || returnValue.ShouldRunIfAlreadyExceptionOccured)
                    {
                        action.Invoke();
                    }
                    else
                    {
                        this.logger.LogInformation(
                            "execution of {Category} skipped, because exceptions are already tracked",
                            parameter.RunCategory);
                    }
                }
                catch (Exception exc)
                {
                    this.logger.LogError(
                        exc,
                        "Exception occured in execution of {Category} - {ExcClass}: {ExcMessage}",
                        parameter.RunCategory,
                        exc.GetType().ToString(),
                        exc.Message);
                    this.logger.LogError("Stacktrace:\n{StackTrace}", exc.StackTrace);
                    returnValue.Exceptions.Add(exc);
                }

#pragma warning restore CA1031 // do not catch general exception types
            }
        }
    }

    private void CreateSut(ExecutionParameter parameter, ExecutionOutput returnValue)
    {
        if (!parameter.CreateSut || (parameter.ExceptionsCount != 0 && !returnValue.ShouldRunIfAlreadyExceptionOccured))
        {
            return;
        }

        bool successful = false;
        for (int retryIndex = 0; retryIndex < RetryCountCreatingSut; retryIndex++)
        {
            try
            {
                var sutType = Ensure.NotNull(parameter.SutType, nameof(parameter.SutType));
                returnValue.SutEndpoint = parameter.SutEndpoint;
                if (returnValue.SutEndpoint == RandomEndpoint)
                {
                    int port = 10001 + RandomNumberGenerator.GetInt32(100);
                    returnValue.SutEndpoint = returnValue.SutEndpoint.Replace(
                        RandomEndpointPlaceholder,
                        port.ToString(CultureInfo.InvariantCulture),
                        StringComparison.InvariantCulture);
                }

                // if endpoint is null then in-memory, else public
                var wafType = typeof(SinusWebApplicationFactory<>).MakeGenericType(sutType);
                ISinusWebApplicationFactory? waf = Activator.CreateInstance(
                    wafType,
                    returnValue.SutEndpoint)
                    as ISinusWebApplicationFactory;

                returnValue.WebApplicationFactory = waf;
                returnValue.HttpClient = waf?.CreateClient();

                if (returnValue.HttpClient == null || returnValue.WebApplicationFactory == null)
                {
                    returnValue.HttpClient?.CancelPendingRequests();
                    returnValue.HttpClient?.Dispose();
                    returnValue.HttpClient = null;

                    returnValue.WebApplicationFactory?.CloseCreatedHost();
                    returnValue.WebApplicationFactory?.Dispose();
                    returnValue.WebApplicationFactory = null;
                }
                else
                {
                    successful = true;
                    break;
                }
            }
            catch (IOException exception)
            {
                this.logger.LogError(
                    exception,
                    "retry attempt {Attempt}\n{Exception}",
                    retryIndex + 1,
                    exception.Message);
                Thread.Sleep(TimeSpan.FromSeconds(RetryDelay));
            }
        }

        if (!successful)
        {
            throw new InvalidOperationException("System under test could not be created.");
        }
    }
}
