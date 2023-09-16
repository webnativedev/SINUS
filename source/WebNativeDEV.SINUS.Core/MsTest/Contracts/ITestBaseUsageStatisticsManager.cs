// <copyright file="ITestBaseUsageStatisticsManager.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.MsTest.Contracts;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebNativeDEV.SINUS.MsTest;

/// <summary>
/// Usage statistics.
/// </summary>
public interface ITestBaseUsageStatisticsManager
{
    /// <summary>
    /// Gets the attribute value of browser created.
    /// </summary>
    string AttributeBrowserCreated { get; }

    /// <summary>
    /// Gets the attribute value of browser disposed.
    /// </summary>
    string AttributeBrowserDisposed { get; }

    /// <summary>
    /// Gets the attribute value of waf created.
    /// </summary>
    string AttributeWafCreated { get; }

    /// <summary>
    /// Gets the attribute value of waf disposed.
    /// </summary>
    string AttributeWafDisposed { get; }

    /// <summary>
    /// Gets the attribute value of business requirement.
    /// </summary>
    string AttributeBusinessRequirement { get; }

    /// <summary>
    /// Gets the attribute value of business requirements.
    /// </summary>
    string AttributeBusinessRequirements { get; }

    /// <summary>
    /// Gets the attribute value of the out of scope information.
    /// </summary>
    string AttributeOutOfScope { get; }

    /// <summary>
    /// Gets the attribute value of the technical requirement.
    /// </summary>
    string AttributeTechnicalRequirement { get; }

    /// <summary>
    /// Gets the attribute value of the technical requirements.
    /// </summary>
    string AttributeTechnicalRequirements { get; }

    /// <summary>
    /// Registers an execution of a test.
    /// </summary>
    /// <param name="scope">Test dependencies.</param>
    void Register(TestBaseScopeContainer scope);

    /// <summary>
    /// Registers an execution of a test by name and context.
    /// </summary>
    /// <param name="testBase">The testbase of the test to especially retrieve its test context.</param>
    /// <returns>Generated dependencies container.</returns>
    TestBaseScopeContainer Register(TestBase testBase);

    /// <summary>
    /// Sets attributes.
    /// </summary>
    /// <param name="testName">The test to flag.</param>
    /// <param name="key">The attribute key.</param>
    /// <param name="value">The attribute value.</param>
    void SetAttribute(string testName, string key, object value);

    /// <summary>
    /// Checks dictionary for correctness.
    /// </summary>
    /// <param name="testName">The test to check.</param>
    /// <param name="check">Check function validating the state.</param>
    /// <returns>Whether or not the data is in correct state.</returns>
    bool CheckAttribute(string testName, Func<Dictionary<string, object>, bool> check);

    /// <summary>
    /// Checks all dictionaries for correctness.
    /// </summary>
    /// <param name="check">Check function validating the state.</param>
    /// <returns>Whether or not the data is in correct state.</returns>
    bool CheckAttribute(Func<Dictionary<string, object>, bool> check);

    /// <summary>
    /// Prints the usage statistics of the browser objects.
    /// </summary>
    /// <param name="filter">a Filter to search for specific test names.</param>
    void PrintBrowserUsageStatistic(string? filter = null);

    /// <summary>
    /// Prints the usage statistics of the waf objects.
    /// </summary>
    /// <param name="filter">a Filter to search for specific test names.</param>
    void PrintWafUsageStatistic(string? filter = null);

    /// <summary>
    /// Prints the usage statistics of the full test.
    /// </summary>
    /// <param name="filter">a Filter to search for specific test names.</param>
    void PrintUsageStatistic(string? filter = null);

    /// <summary>
    /// Prints the business requirements of all tests.
    /// </summary>
    /// <param name="filter">a Filter to search for specific test names.</param>
    void PrintBusinessRequirements(string? filter = null);

    /// <summary>
    /// Prints the technical requirements of all tests.
    /// </summary>
    /// <param name="filter">a Filter to search for specific test names.</param>
    void PrintTechnicalRequirements(string? filter = null);
}
