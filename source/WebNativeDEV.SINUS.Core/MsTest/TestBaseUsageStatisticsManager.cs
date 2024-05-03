// <copyright file="TestBaseUsageStatisticsManager.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.MsTest;

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using WebNativeDEV.SINUS.Core.Logging;
using WebNativeDEV.SINUS.Core.MsTest.Contracts;
using WebNativeDEV.SINUS.Core.Requirements;
using WebNativeDEV.SINUS.MsTest;

/// <summary>
/// Manages the usage of tests.
/// </summary>
internal class TestBaseUsageStatisticsManager : ITestBaseUsageStatisticsManager
{
    private readonly ConcurrentDictionary<string, Dictionary<string, object>> usages = [];

    /// <inheritdoc/>
    public string AttributeBrowserCreated => "browser created";

    /// <summary>
    /// Gets the attribute value of browser disposed.
    /// </summary>
    public string AttributeBrowserDisposed => "browser disposed";

    /// <summary>
    /// Gets the attribute value of waf created.
    /// </summary>
    public string AttributeWafCreated => "waf created";

    /// <summary>
    /// Gets the attribute value of waf disposed.
    /// </summary>
    public string AttributeWafDisposed => "waf disposed";

    /// <summary>
    /// Gets the attribute value of business requirement.
    /// </summary>
    public string AttributeBusinessRequirement => "BusinessRequirement";

    /// <summary>
    /// Gets the attribute value of business requirements.
    /// </summary>
    public string AttributeBusinessRequirements => "BusinessRequirements";

    /// <summary>
    /// Gets the attribute value of the out of scope information.
    /// </summary>
    public string AttributeOutOfScope => "OutOfScope";

    /// <summary>
    /// Gets the attribute value of the technical requirement.
    /// </summary>
    public string AttributeTechnicalRequirement => "AttributeTechnicalRequirement";

    /// <summary>
    /// Gets the attribute value of the technical requirements.
    /// </summary>
    public string AttributeTechnicalRequirements => "AttributeTechnicalRequirements";

    /// <inheritdoc/>
    public TestBaseScopeContainer Register(TestBase testBase, string? scenario = null)
    {
        return new TestBaseScopeContainer(testBase, scenario);
    }

    /// <inheritdoc/>
    public void Register(TestBaseScopeContainer scope)
    {
        try
        {
            if (!this.usages.TryAdd(scope.TestName, []))
            {
                throw new InvalidDataException("test name already stored in usage manager, please rename your test");
            }

            this.usages[scope.TestName].Add("scope", scope);
        }
        catch (Exception outerExc)
        {
            throw new InvalidDataException("error registering usage information to statistics manager", outerExc);
        }

        this.StoreAttribute<BusinessRequirementAttribute>(
            this.AttributeBusinessRequirement,
            scope,
            (attr, result) => result.Add((descriptions: attr.Description, level: "method")));

        this.StoreClassLevelAttribute<BusinessRequirementsAttribute>(
            this.AttributeBusinessRequirements,
            scope,
            (attr, result) => result.Add((capability: attr.Capability, requirements: attr.Requirements, level: "class")));

        this.StoreAttribute<OutOfScopeAttribute>(
            this.AttributeOutOfScope,
            scope,
            (attr, result) => result.Add((description: attr.Description, level: "method")));

        this.StoreAttribute<TechnicalRequirementAttribute>(
            this.AttributeTechnicalRequirement,
            scope,
            (attr, result) => result.Add((description: attr.Description, level: "method")));

        this.StoreClassLevelAttribute<TechnicalRequirementsAttribute>(
            this.AttributeTechnicalRequirements,
            scope,
            (attr, result) => result.Add((capability: attr.Capability, requirements: attr.Requirements, level: "class")));
    }

    /// <inheritdoc/>
    public void PrintBusinessRequirements(string? filter = null)
        => this.PrintRequirements(
            filter,
            this.AttributeBusinessRequirement,
            this.AttributeBusinessRequirements,
            "Business");

    /// <inheritdoc/>
    public void PrintTechnicalRequirements(string? filter = null)
        => this.PrintRequirements(
            filter,
            this.AttributeTechnicalRequirement,
            this.AttributeTechnicalRequirements,
            "Technical");

    /// <inheritdoc/>
    public void PrintUsageStatistic(string? filter = null)
    {
        var data = this.usages
            .Where(x => filter == null || x.Key == filter)
            .Select(x => (Title: x.Key,
                          BrowserCreated: x.Value.ContainsKey(this.AttributeBrowserCreated),
                          BrowserDisposed: x.Value.ContainsKey(this.AttributeBrowserDisposed),
                          WafCreated: x.Value.ContainsKey(this.AttributeWafCreated),
                          WafDisposed: x.Value.ContainsKey(this.AttributeWafDisposed)))
            .ToList();
        if (data.Count == 0)
        {
            return;
        }

        var usageLogger = TestBaseSingletonContainer.CreateLogger<TestBase>();
        usageLogger.LogInformation("{Sparator}\n| Tests: {Count}", LoggerConstants.SeparationLine, data.Count);

        foreach (var (title, browserCreated, browserDisposed, wafCreated, wafDisposed) in data)
        {
            string browserInfo;
            if (!browserCreated && !browserDisposed)
            {
                browserInfo = "b:---";
            }
            else if (browserCreated && browserDisposed)
            {
                browserInfo = "b:-ok";
            }
            else
            {
                browserInfo = "b:err";
            }

            string wafInfo;
            if (!wafCreated && !wafDisposed)
            {
                wafInfo = "w:---";
            }
            else if (wafCreated && wafDisposed)
            {
                wafInfo = "w:-ok";
            }
            else
            {
                wafInfo = "w:err";
            }

            usageLogger.LogInformation("| ({BrowserInfo}|{WafInfo}) {Id}", browserInfo, wafInfo, title);
        }

        usageLogger.LogInformation("{SeparationLine}", LoggerConstants.SeparationLineWithNewLine);

        var scopes = this.usages.Select(x => x.Value.GetValueOrDefault("scope") as TestBaseScopeContainer);
        var grouping = scopes
            .Where(x => x != null)
            .Cast<TestBaseScopeContainer>()
            .GroupBy(x => x.TestBase.TestContext.FullyQualifiedTestClassName)
            .Select(group => new
            {
                ClassName = group.Key,
                Count = group.Count(),
            })
            .ToList();
        foreach (var item in grouping)
        {
            usageLogger.LogInformation("{Count} - {TestClass}", item.Count, item.ClassName);
        }
    }

    /// <inheritdoc/>
    public void PrintBrowserUsageStatistic(string? filter = null)
    {
        var data = this.usages
            .Where(x => filter == null || x.Key == filter)
            .Select(x => (Title: x.Key,
                          Created: x.Value.ContainsKey(this.AttributeBrowserCreated),
                          Disposed: x.Value.ContainsKey(this.AttributeBrowserDisposed)))
            .Where(x => x.Created || x.Disposed)
            .ToList();
        PrintSpecificStatistic(data, "Browser");
    }

    /// <inheritdoc/>
    public void PrintWafUsageStatistic(string? filter = null)
    {
        var data = this.usages
            .Where(x => filter == null || x.Key == filter)
            .Select(x => (Title: x.Key,
                          Created: x.Value.ContainsKey(this.AttributeWafCreated),
                          Disposed: x.Value.ContainsKey(this.AttributeWafDisposed)))
            .Where(x => x.Created || x.Disposed)
            .ToList();
        PrintSpecificStatistic(data, "Waf");
    }

    /// <inheritdoc/>
    public void SetAttribute(string testName, string key, object value)
    {
        this.usages[testName].Add(key, value);
    }

    /// <inheritdoc/>
    public bool CheckAttribute(string testName, Func<Dictionary<string, object>, bool> check, bool shouldThrow = false)
    {
        var result = check.Invoke(this.usages[testName]);
        if (!result && shouldThrow)
        {
            throw new InvalidDataException($"attribute check failed for test {testName}");
        }

        return result;
    }

    /// <inheritdoc/>
    public bool CheckAttribute(Func<Dictionary<string, object>, bool> check, bool shouldThrow = false)
    {
        foreach (var usage in this.usages)
        {
            if (!this.CheckAttribute(usage.Key, check, shouldThrow))
            {
                return false;
            }
        }

        return true;
    }

    private static void PrintSpecificStatistic(List<(string Title, bool Created, bool Disposed)> data, string category)
    {
        if (data.Count == 0)
        {
            return;
        }

        var usageLogger = TestBaseSingletonContainer.CreateLogger<TestBase>();
        usageLogger.LogInformation("{SeparationLine}| Tests Including {Category}: {Count}", LoggerConstants.SeparationLineWithNewLine, category, data.Count);

        foreach (var (title, _, disposed) in data)
        {
            var disposedInfo = disposed
                        ? "disposed"
                        : "leak    ";
            usageLogger.LogInformation("| ({DisposedInfo}) {Id}", disposedInfo, title);
        }

        usageLogger.LogInformation("{SeparationLine}", LoggerConstants.SeparationLineWithNewLine);
    }

    private void PrintRequirements(string? filter, string methodKey, string classKey, string title)
    {
        var methodData = this.usages
            .Where(x => filter == null || x.Key == filter)
            .Where(x => x.Value != null && x.Value.ContainsKey(methodKey))
            .Select(x => (TestName: x.Key,
                          Data: x.Value[methodKey] as List<object>))
            .Where(x => x.Data != null)
            .SelectMany(
                x => x.Data ?? throw new InvalidDataException(),
                (x, item) => (x.TestName, (((string[] Descriptions, string Level))item).Descriptions))
            .SelectMany(
                x => x.Descriptions ?? throw new InvalidDataException(),
                (x, item) => (x.TestName, Description: item))
            .Where(x => x.Description != null)
            .ToList();

        var classData = this.usages
            .Where(x => filter == null || x.Key == filter)
            .Where(x => x.Value != null && x.Value.ContainsKey(classKey))
            .Select(x => (TestName: x.Key,
                          Data: x.Value[classKey] as List<object>))
            .Where(x => x.Data != null)
            .SelectMany(
                x => x.Data ?? throw new InvalidDataException(),
                (x, item) => (x.TestName, Descriptions: (((string capability, string[] requirements, string level))item).requirements))
            .SelectMany(
                x => x.Descriptions ?? throw new InvalidDataException(),
                (x, item) => (x.TestName, Description: item))
            .Where(x => x.Description != null)
            .ToList();

        var data = methodData
            .Union(classData)
            .GroupBy(x => x.Description)
            .Select(group => new
            {
                Requirement = group.Key,
                Tests = group.Select(x => x.TestName).ToList(),
                Count = group.Count(),
            })
            .ToList();
        if (data.Count == 0)
        {
            return;
        }

        var usageLogger = TestBaseSingletonContainer.CreateLogger<TestBase>();
        usageLogger.LogInformation("{SeparationLine}| {Title} Requirements: {Count}", LoggerConstants.SeparationLineWithNewLine, title, data.Count);

        foreach (var item in data)
        {
            usageLogger.LogInformation("| {Count} {Capability}", item.Count, item.Requirement);
            foreach (var test in item.Tests)
            {
                usageLogger.LogInformation("|     {Test}", test);
            }
        }

        usageLogger.LogInformation("{SeparationLine}", LoggerConstants.SeparationLineWithNewLine);
    }

    private void StoreAttribute<T>(string key, TestBaseScopeContainer scope, Action<T, List<object>> action)
        where T : Attribute
    {
        var result = new List<object>();

        foreach (var attribute in scope.Method?.GetCustomAttributes(typeof(T), true) ?? [])
        {
            if (attribute is T baAttr)
            {
                action.Invoke(baAttr, result);
            }
        }

        if (result.Count > 0)
        {
            this.usages[scope.TestName].Add(key, result);
        }
    }

    private void StoreClassLevelAttribute<T>(string key, TestBaseScopeContainer scope, Action<T, List<object>> action)
        where T : Attribute
    {
        var result = new List<object>();

        foreach (var attribute in scope.Method?.DeclaringType?.GetCustomAttributes(typeof(T), true) ?? [])
        {
            if (attribute is T baAttr)
            {
                action.Invoke(baAttr, result);
            }
        }

        if (result.Count > 0)
        {
            this.usages[scope.TestName].Add(key, result);
        }
    }
}
