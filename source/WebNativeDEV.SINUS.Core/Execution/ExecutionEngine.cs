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
using System.Linq;
using System.Reflection.Metadata;
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
    private const int RetryCountCreatingSut = 6;
    private readonly ILogger<ExecutionEngine> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExecutionEngine"/> class.
    /// </summary>
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

        this.CreateSut(parameter, returnValue);
        this.RunActions(parameter, returnValue);

        return returnValue;
    }

    private static string CalculateDescription(int index, int count, string? description, string testName, RunCategory category)
    {
        string prefix = count == 1
                    ? string.Empty
                    : $"{index + 1:00}: ";

        string newDescription = string.Empty;
        if (!string.IsNullOrWhiteSpace(description))
        {
            newDescription = description;
        }
        else
        {
            var items = testName.Split('_', ' ');
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == category.ToString())
                {
                    newDescription = items[i + 1];
                }
            }

            var builder = new StringBuilder();
            for (int i = 0; i < newDescription.Length; i++)
            {
                if (char.IsUpper(newDescription[i]) && i > 0)
                {
                    builder.Append(' ');
                }

                builder.Append(newDescription[i]);
            }

            newDescription = builder.ToString();
        }

        return $"{prefix}{newDescription}";
    }

    private void RunActions(ExecutionParameter parameter, ExecutionOutput returnValue)
    {
        var testBase = Ensure.NotNull(parameter.TestBase);

        if (!parameter.RunActions || parameter.Actions == null || !parameter.Actions.Any(x => x != null))
        {
            string skipDescription = CalculateDescription(
                1,
                1,
                parameter.Description,
                testBase.TestName,
                parameter.RunCategory);
            PerformanceDataScope.WriteSkip(this.logger, parameter.RunCategory.ToString(), skipDescription);
            return;
        }

        var actionCount = parameter.Actions.Count;
        for (int i = 0; i < actionCount; i++)
        {
            var action = parameter.Actions?[i];
            if (action == null)
            {
                continue;
            }

            string description = CalculateDescription(
                i,
                actionCount,
                parameter.Description,
                testBase.TestName,
                parameter.RunCategory);

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

                // if endpoint is null then in-memory, else public
                var wafType = typeof(SinusWebApplicationFactory<>).MakeGenericType(sutType);
                ISinusWebApplicationFactory? waf = Activator.CreateInstance(
                    wafType,
                    args: new object?[] { parameter.SutEndpoint, this.logger })
                    as ISinusWebApplicationFactory;

                returnValue.WebApplicationFactory = waf;
                returnValue.HttpClient = waf?.CreateClient();

                if (returnValue.HttpClient == null || returnValue.WebApplicationFactory == null)
                {
                    returnValue.HttpClient?.CancelPendingRequests();
                    returnValue.HttpClient?.Dispose();
                    returnValue.HttpClient = null;
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
                this.logger.LogError(exception, "retry attempt {Attempt}", retryIndex + 1);
                Thread.Sleep(TimeSpan.FromSeconds(10));
            }
        }

        if (!successful)
        {
            throw new InvalidOperationException("System under test could not be created.");
        }
    }
}
