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

        var data = new List<string>();

        foreach (var attribute in scope.Method.GetCustomAttributes(typeof(BusinessRequirementAttribute), true))
        {
            if (attribute is BusinessRequirementAttribute baAttr)
            {
                foreach (var description in baAttr.Description)
                {
                    data.Add(description);
                }
            }
        }

        this.usages[scope.TestBase.TestName].Add("BusinessRequirementAttribute", data);
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
}
