// <copyright file="TestBaseUsageStatisticsManager.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.MsTest;

using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WebNativeDEV.SINUS.Core.MsTest.Contracts;
using WebNativeDEV.SINUS.Core.Requirements;
using WebNativeDEV.SINUS.Core.UITesting;
using WebNativeDEV.SINUS.MsTest;
using static System.Runtime.InteropServices.JavaScript.JSType;

/// <summary>
/// Manages the usage of tests.
/// </summary>
internal class TestBaseUsageStatisticsManager : ITestBaseUsageStatisticsManager
{
    private readonly Dictionary<string, Dictionary<string, object>> usages = new();

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
    /// Gets the attribute value of the technical approval.
    /// </summary>
    public string AttributeTechnicalApproval => "AttributeTechnicalApproval";

    /// <summary>
    /// Gets the attribute value of the technical requirement.
    /// </summary>
    public string AttributeTechnicalRequirement => "AttributeTechnicalRequirement";

    /// <summary>
    /// Gets the attribute value of the technical requirements.
    /// </summary>
    public string AttributeTechnicalRequirements => "AttributeTechnicalRequirements";

    /// <inheritdoc/>
    public TestBaseScopeContainer Register(TestBase testBase)
    {
        return new TestBaseScopeContainer(testBase);
    }

    /// <inheritdoc/>
    public void Register(TestBaseScopeContainer scope)
    {
        this.usages.Add(scope.TestBase.TestName, new Dictionary<string, object>());
        this.usages[scope.TestBase.TestName].Add("scope", scope);

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

        this.StoreAttribute<TechnicalApprovalAttribute>(
            this.AttributeTechnicalApproval,
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
    {
        var methodData = this.usages
            .Where(x => filter == null || x.Key == filter)
            .Where(x => x.Value != null && x.Value.ContainsKey(this.AttributeBusinessRequirement))
            .Select(x => (TestName: x.Key,
                          Data: x.Value[this.AttributeBusinessRequirement] as List<object>))
            .Where(x => x.Data != null)
            .SelectMany(
                x => x.Data ?? throw new InvalidDataException(),
                (x, item) => (x.TestName, Descriptions: (((string[] Descriptions, string Level))item).Descriptions))
            .SelectMany(
                x => x.Descriptions ?? throw new InvalidDataException(),
                (x, item) => (x.TestName, Description: item))
            .Where(x => x.Description != null)
            .ToList();

        var classData = this.usages
            .Where(x => filter == null || x.Key == filter)
            .Where(x => x.Value != null && x.Value.ContainsKey(this.AttributeBusinessRequirements))
            .Select(x => (TestName: x.Key,
                          Data: x.Value[this.AttributeBusinessRequirements] as List<object>))
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
                BusinessRequirement = group.Key,
                Tests = group.Select(x => x.TestName).ToList(),
                Count = group.Count(),
            })
            .ToList();
        if (!data.Any())
        {
            return;
        }

        var usageLogger = TestBaseSingletonContainer.CreateLogger<TestBase>();
        usageLogger.LogInformation("+--------------------------------");
        usageLogger.LogInformation("| Business Requirements: {Count}", data.Count);

        foreach (var item in data)
        {
            usageLogger.LogInformation("| {Count} {Capability}", item.Count, item.BusinessRequirement);
            foreach (var test in item.Tests)
            {
                usageLogger.LogInformation("|     {Test}", test);
            }
        }

        usageLogger.LogInformation("+--------------------------------");
        usageLogger.LogInformation(" ");
    }

    /// <inheritdoc/>
    public void PrintTechnicalRequirements(string? filter = null)
    {
        var methodData = this.usages
            .Where(x => filter == null || x.Key == filter)
            .Where(x => x.Value != null && x.Value.ContainsKey(this.AttributeTechnicalRequirement))
            .Select(x => (TestName: x.Key,
                          Data: x.Value[this.AttributeTechnicalRequirement] as List<object>))
            .Where(x => x.Data != null)
            .SelectMany(
                x => x.Data ?? throw new InvalidDataException(),
                (x, item) => (x.TestName, Descriptions: (((string[] Descriptions, string Level))item).Descriptions))
            .SelectMany(
                x => x.Descriptions ?? throw new InvalidDataException(),
                (x, item) => (x.TestName, Description: item))
            .Where(x => x.Description != null)
            .ToList();

        var classData = this.usages
            .Where(x => filter == null || x.Key == filter)
            .Where(x => x.Value != null && x.Value.ContainsKey(this.AttributeTechnicalRequirements))
            .Select(x => (TestName: x.Key,
                          Data: x.Value[this.AttributeTechnicalRequirements] as List<object>))
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
                BusinessRequirement = group.Key,
                Tests = group.Select(x => x.TestName).ToList(),
                Count = group.Count(),
            })
            .ToList();
        if (!data.Any())
        {
            return;
        }

        var usageLogger = TestBaseSingletonContainer.CreateLogger<TestBase>();
        usageLogger.LogInformation("+--------------------------------");
        usageLogger.LogInformation("| Technical Requirements: {Count}", data.Count);

        foreach (var item in data)
        {
            usageLogger.LogInformation("| {Count} {Capability}", item.Count, item.BusinessRequirement);
            foreach (var test in item.Tests)
            {
                usageLogger.LogInformation("|     {Test}", test);
            }
        }

        usageLogger.LogInformation("+--------------------------------");
        usageLogger.LogInformation(" ");
    }

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
        if (!data.Any())
        {
            return;
        }

        var usageLogger = TestBaseSingletonContainer.CreateLogger<TestBase>();
        usageLogger.LogInformation("+--------------------------------");
        usageLogger.LogInformation("| Tests: {Count}", data.Count);

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

        usageLogger.LogInformation("+--------------------------------");
        usageLogger.LogInformation(" ");

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
    public bool CheckAttribute(string testName, Func<Dictionary<string, object>, bool> check)
    {
        return !check.Invoke(this.usages[testName]);
    }

    /// <inheritdoc/>
    public bool CheckAttribute(Func<Dictionary<string, object>, bool> check)
    {
        foreach (var usage in this.usages)
        {
            if (!this.CheckAttribute(usage.Key, check))
            {
                return false;
            }
        }

        return true;
    }

    private static void PrintSpecificStatistic(List<(string Title, bool Created, bool Disposed)> data, string category)
    {
        if (!data.Any())
        {
            return;
        }

        var usageLogger = TestBaseSingletonContainer.CreateLogger<TestBase>();
        usageLogger.LogInformation("+--------------------------------");
        usageLogger.LogInformation("| Tests Including {Category}: {Count}", category, data.Count);

        foreach (var (title, _, disposed) in data)
        {
            var disposedInfo = disposed
                        ? "disposed"
                        : "leak    ";
            usageLogger.LogInformation("| ({DisposedInfo}) {Id}", disposedInfo, title);
        }

        usageLogger.LogInformation("+--------------------------------");
        usageLogger.LogInformation(" ");
    }

    private void StoreAttribute<T>(string key, TestBaseScopeContainer scope, Action<T, List<object>> action)
        where T : Attribute
    {
        var result = new List<object>();

        foreach (var attribute in scope.Method.GetCustomAttributes(typeof(T), true))
        {
            if (attribute is T baAttr)
            {
                action.Invoke(baAttr, result);
            }
        }

        if (result.Any())
        {
            this.usages[scope.TestBase.TestName].Add(key, result);
        }
    }

    private void StoreClassLevelAttribute<T>(string key, TestBaseScopeContainer scope, Action<T, List<object>> action)
        where T : Attribute
    {
        var result = new List<object>();

        foreach (var attribute in scope.Method?.DeclaringType?.GetCustomAttributes(typeof(T), true) ?? Array.Empty<object>())
        {
            if (attribute is T baAttr)
            {
                action.Invoke(baAttr, result);
            }
        }

        if (result.Any())
        {
            this.usages[scope.TestBase.TestName].Add(key, result);
        }
    }
}
