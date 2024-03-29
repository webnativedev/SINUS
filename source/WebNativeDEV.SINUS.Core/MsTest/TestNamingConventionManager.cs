﻿// <copyright file="TestNamingConventionManager.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.MsTest;

using System;
using System.Linq;
using System.Reflection;
using System.Text;
using WebNativeDEV.SINUS.Core.ArgumentValidation;
using WebNativeDEV.SINUS.Core.FluentAPI.Model;
using WebNativeDEV.SINUS.Core.MsTest.Contracts;

/// <summary>
/// Validator for the test naming.
/// Convention is Given_X_When_Y_Then_Z.
/// </summary>
public class TestNamingConventionManager : ITestNamingConventionManager
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestNamingConventionManager"/> class.
    /// </summary>
    /// <param name="scope">The dependency store.</param>
    public TestNamingConventionManager(TestBaseScopeContainer scope)
        : this(Ensure.NotNull(scope).TestName)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestNamingConventionManager"/> class.
    /// </summary>
    /// <param name="testName">The name of the test.</param>
    public TestNamingConventionManager(string testName)
    {
        this.TestName = testName;

        if (this.TestName.StartsWith("Maintenance_", StringComparison.InvariantCulture))
        {
            this.GivenDescription = string.Empty;
            this.WhenDescription = string.Empty;
            this.ThenDescription = string.Empty;
            this.IsMaintenance = true;
            return;
        }

        var nameparts = this.TestName.Split('_', StringSplitOptions.RemoveEmptyEntries);

        if (nameparts.Length != 6)
        {
            throw new InvalidOperationException("Given_X_When_Y_Then_Z - naming convention has not right count of elements");
        }

        if (nameparts[0] != "Given" || nameparts[2] != "When" || nameparts[4] != "Then")
        {
            throw new InvalidOperationException("Given_X_When_Y_Then_Z - naming convention has not used right keywords");
        }

        this.GivenDescription = nameparts[1];
        this.WhenDescription = nameparts[3];
        this.ThenDescription = nameparts[5];

        if (!char.IsUpper(this.GivenDescription[0]))
        {
            throw new InvalidOperationException("Given_X_When_Y_Then_Z - naming convention Given-Description does not start with an upper case character.");
        }

        if (!char.IsUpper(this.WhenDescription[0]))
        {
            throw new InvalidOperationException("Given_X_When_Y_Then_Z - naming convention When-Description does not start with an upper case character.");
        }

        if (!char.IsUpper(this.ThenDescription[0]))
        {
            throw new InvalidOperationException("Given_X_When_Y_Then_Z - naming convention Then-Description does not start with an upper case character.");
        }
    }

    /// <inheritdoc />
    public bool IsMaintenance { get; }

    /// <inheritdoc />
    public string TestName { get; }

    /// <inheritdoc />
    public string GivenDescription { get; }

    /// <inheritdoc />
    public string WhenDescription { get; }

    /// <inheritdoc />
    public string ThenDescription { get; }

    /// <summary>
    /// Calculates a display name for different scenarios.
    /// </summary>
    /// <param name="testName">The method name to work on.</param>
    /// <param name="scenario">The scenario name.</param>
    /// <returns>The calculated test scenario name.</returns>
    public static string DynamicDataDisplayNameAddScenario(string? testName, string? scenario)
    {
        testName = Ensure.NotNull(testName);

        if (string.IsNullOrWhiteSpace(scenario))
        {
            return testName;
        }

        var newName = testName.Replace(
                "_Then",
                "WithValue" + scenario + "_Then",
                StringComparison.InvariantCulture);
        return newName;
    }

    /// <summary>
    /// Calculates a display name for different scenarios.
    /// </summary>
    /// <param name="methodInfo">The method to work on.</param>
    /// <param name="data">The arguments of the function. Last argument is the scenario name.</param>
    /// <returns>The calculated test scenario name.</returns>
    public static string DynamicDataDisplayNameAddValueFromLastArgument(MethodInfo methodInfo, object[] data)
    {
        var originalName = methodInfo?.Name
            ?? throw new ArgumentException("methodName is null", nameof(methodInfo));
        return DynamicDataDisplayNameAddScenario(originalName, data.LastOrDefault(string.Empty) as string);
    }

    /// <inheritdoc />
    public string GetName(RunCategory category)
    {
        return category switch
        {
            RunCategory.Given => this.GivenDescription,
            RunCategory.When => this.WhenDescription,
            RunCategory.Then => this.ThenDescription,
            _ => category.ToString(),
        };
    }

    /// <inheritdoc />
    public string GetReadableName(RunCategory category)
    {
        string name = this.GetName(category);

        var builder = new StringBuilder();

        for (int i = 0; i < name.Length; i++)
        {
            if (char.IsUpper(name[i]) && i > 0)
            {
                builder.Append(' ');
            }

            builder.Append(name[i]);
        }

        return builder.ToString();
    }

    /// <inheritdoc />
    public string GetReadableDescription(RunCategory category, string? description, int index, int count)
    {
        string prefix = count == 1
                    ? string.Empty
                    : $"{index + 1:00}: ";

        if (!string.IsNullOrWhiteSpace(description))
        {
            return $"{prefix}{description}";
        }

        return $"{prefix}{this.GetReadableName(category)}";
    }
}
