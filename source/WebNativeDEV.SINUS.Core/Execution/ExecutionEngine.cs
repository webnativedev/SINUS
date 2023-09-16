// <copyright file="ExecutionEngine.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Execution;

using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NSubstitute.Core;
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
using WebNativeDEV.SINUS.Core.Execution.Exceptions;
using WebNativeDEV.SINUS.Core.Execution.Model;
using WebNativeDEV.SINUS.Core.FluentAPI.Model;
using WebNativeDEV.SINUS.Core.Logging;
using WebNativeDEV.SINUS.Core.MsTest;
using WebNativeDEV.SINUS.Core.Sut;
using WebNativeDEV.SINUS.Core.Sut.Contracts;
using WebNativeDEV.SINUS.MsTest;

/// <summary>
/// The execution engine.
/// </summary>
internal sealed class ExecutionEngine : IExecutionEngine
{
    /// <summary>
    /// Random Endpoint placeholder/template that will be used later for string replacement.
    /// </summary>
    public const string RandomEndpoint = "https://localhost:" + RandomEndpointPlaceholder;

    private const string RandomEndpointPlaceholder = "{RANDOMENDPOINT}";
    private const int RetryCountCreatingSut = 5;
    private const int RetryDelay = 60;
    private static readonly object LockerSutStatic = new();
    private readonly ILogger logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExecutionEngine"/> class.
    /// </summary>
    public ExecutionEngine()
        : this(TestBaseSingletonContainer.LoggerFactory)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExecutionEngine"/> class.
    /// </summary>
    /// <param name="factory">Logger factory dependency.</param>
    public ExecutionEngine(ILoggerFactory factory)
    {
        this.logger = factory.CreateLogger<ExecutionEngine>();
        this.logger.LogDebug("Created a log for execution engine");
    }

    /// <inheritdoc/>
    public ExecutionOutput Run(ExecutionParameter? parameter)
    {
        parameter = Ensure.NotNull(parameter, nameof(parameter));

        ExecutionOutput returnValue = new()
        {
            IsPreparedOnly = !parameter.Actions.Any(),
            ShouldRunIfAlreadyExceptionOccured = parameter.RunCategory switch
            {
                RunCategory.Given => false,
                RunCategory.When => false,
                _ => true,
            },
            RunCategory = parameter.RunCategory,
            TestBase = parameter.TestBase,
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

    private static string? CalculateSutEndpoint(ExecutionParameter? parameter)
    {
        var endpoint = parameter?.SutEndpoint;
        if (endpoint == RandomEndpoint)
        {
            int port = 10001 + RandomNumberGenerator.GetInt32(100);
            endpoint = endpoint.Replace(
                RandomEndpointPlaceholder,
                port.ToString(CultureInfo.InvariantCulture),
                StringComparison.InvariantCulture);
        }

        return endpoint;
    }

    private static bool CreateWaf(ExecutionParameter parameter, ExecutionOutput returnValue)
    {
        var sutType = Ensure.NotNull(parameter?.SutType, nameof(parameter.SutType));
        returnValue.SutEndpoint = CalculateSutEndpoint(parameter);

        // if endpoint is null then in-memory, else public
        var wafType = typeof(SinusWebApplicationFactory<>).MakeGenericType(sutType);
        ISinusWebApplicationFactory? waf = Activator.CreateInstance(
            wafType,
            returnValue.SutEndpoint,
            parameter?.TestBase?.TestName,
            parameter?.SutArgs.ToArray())
            as ISinusWebApplicationFactory;

        returnValue.WebApplicationFactory = waf;
        returnValue.HttpClient = waf?.CreateClient();

        if (returnValue.HttpClient == null || returnValue.WebApplicationFactory == null)
        {
            throw new ExecutionEngineRunException("Web Application Factory was not created properly.");
        }

        return true;
    }

    private void RunActions(ExecutionParameter parameter, ExecutionOutput returnValue)
    {
        parameter = Ensure.NotNull(parameter);
        var namings = Ensure.NotNull(parameter.Namings);

        List<Action> actions = new();
        actions.AddRange(
            parameter.SetupActions.Select<Action<ExecutionSetupParameters>, Action>(
                action => () => action?.Invoke(new ExecutionSetupParameters()
                {
                    Endpoint = returnValue.SutEndpoint,
                })));

        actions.AddRange(parameter.Actions);

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

#pragma warning disable CA1031 // don't catch general exceptions

        bool successful = false;
        Exception? lastException = null;
        for (int retryIndex = 0; retryIndex < RetryCountCreatingSut && !successful; retryIndex++)
        {
            try
            {
                successful = CreateWaf(parameter, returnValue);
                lastException = null;
            }
            catch (IOException exception)
            {
                this.logger.LogError(
                    exception,
                    "retry attempt {Attempt}\n{Exception}",
                    retryIndex + 1,
                    exception.Message);

                Thread.Sleep(TimeSpan.FromSeconds(RetryDelay));

#pragma warning disable S1215 // "GC.Collect" should not be called
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.WaitForFullGCComplete();
#pragma warning restore S1215 // "GC.Collect" should not be called

                lastException = new ExecutionEngineRunException("creation of web application failed because of IO", exception);
            }
            catch (Exception exception)
            {
                this.logger.LogError(
                    exception,
                    "Creating WebApplication failed\n{Exception}",
                    exception.Message);

                returnValue.Exceptions.Add(new ExecutionEngineRunException("creation of web application failed", exception));

                returnValue.HttpClient?.CancelPendingRequests();
                returnValue.HttpClient?.Dispose();
                returnValue.HttpClient = null;
                returnValue.WebApplicationFactory?.CloseCreatedHost();
                returnValue.WebApplicationFactory?.Dispose();
                returnValue.WebApplicationFactory = null;

                successful = false;
                break;
            }
        }

        if (lastException != null)
        {
            returnValue.Exceptions.Add(lastException);
        }

#pragma warning restore CA1031 // don't catch general exceptions

        this.logger.LogInformation("Execution was {Successful}", successful ? "successful" : "not successful");
    }
}
