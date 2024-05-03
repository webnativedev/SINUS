# Template

## Create Test-Initializer and Finalizer

```
// <copyright file="TestInitializer.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

#pragma warning disable SA1403 // File may only contain a single namespace
#pragma warning disable SA1402 // File may only contain a single type

[assembly: Microsoft.VisualStudio.TestTools.UnitTesting.TestDataSourceDiscovery(Microsoft.VisualStudio.TestTools.UnitTesting.TestDataSourceDiscoveryOption.DuringExecution)]

namespace WebNativeDEV.SINUS.Tests
{
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using WebNativeDEV.SINUS.Core.Assertions;
    using WebNativeDEV.SINUS.Core.MsTest;
    using WebNativeDEV.SINUS.Core.Utils;
    using WebNativeDEV.SINUS.MsTest;

    /// <summary>
    /// Standard test class used especially to store the testing context and for some configuration tasks.
    /// </summary>
    [TestClass]
    public class TestInitializer : TestBase
    {
        private const int MaxAgeOfProessInMinutes = 2;

        /// <summary>
        /// Method that is called by the MS-Test Framework on assmebly startup.
        /// </summary>
        /// <param name="testContext">The current context of the test execution (assembly level).</param>
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext testContext)
            => TestBaseSingletonContainer.AssemblyTestContext = testContext;

        /// <summary>
        /// Method that is called by the MS-Test Framework on assmebly cleanup.
        /// </summary>
        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            SinusUtils.KillChromeZombieProcesses(MaxAgeOfProessInMinutes);
        }

        /// <summary>
        /// Maintenance Test related to the number of output folders.
        /// Fails if too much folders are created.
        /// </summary>
        [TestMethod]
        public void Maintenance_CountOfResultFoldersBelow200()
        {
            this.Maintenance(() => SinusUtils.CountResultFoldersBelowParameter(this, max: 200)).Should().BeSuccessful();
        }

        /// <summary>
        /// Maintenance test related to zombie processes.
        /// Fails if too old processes stay on the machine.
        /// </summary>
        [TestMethod]
        public void Maintenance_ProcessesKilled()
        {
            this.Maintenance(() => SinusUtils.CountChromeZombieProcesses(MaxAgeOfProessInMinutes)).Should().BeSuccessful();
        }
    }
}

namespace WebNativeDEV.SINUS.Tests.ZZZ
{
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using WebNativeDEV.SINUS.Core.Assertions;
    using WebNativeDEV.SINUS.Core.Utils;
    using WebNativeDEV.SINUS.MsTest;

    /// <summary>
    /// This test is named in an odd fashion, because it will then be executed as
    /// the last test by ms-test.
    /// </summary>
    [TestClass]
    public class ZTest : TestBase
    {
        /// <summary>
        /// Last test to be executed (based on name), so that all statistics.
        /// Based on https://learn.microsoft.com/en-us/dotnet/core/testing/order-unit-tests?pivots=mstest
        /// the naming influences the execution order in mstest.
        /// </summary>
        [TestMethod("Final Summary")]
        public void Maintenance_ZZZ_TestSummary()
        {
            this.Maintenance(() => SinusUtils.AssertOnDataLeak()).Should().BeSuccessful();
        }
    }
}
```

## Creating an MS-Test TestClass

.NET 8.0
Supress: 1701;1702;SA1010;SA1009;CS1591;SA1600



```csharp
// <copyright file="XyzTests.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Tests;

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebNativeDEV.SINUS.Core.Assertions;
using WebNativeDEV.SINUS.Core.FluentAPI.Model;
using WebNativeDEV.SINUS.Core.Requirements;
using WebNativeDEV.SINUS.MsTest;
using WebNativeDEV.SINUS.Core.MsTest;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Type_or_Member'.
#pragma warning disable SA1600 // Elements should be documented

[TestClass]
public class AssertionTests : TestBase
{
    [TestMethod]
    public void Given_SomeSituation_When_DoingStuff_Then_ItShouldResultInNoError()
        => this.Test(r => r
            .Given("a simple setup")
            .When("nothing was done")
            .ThenNoError()).Should().BeSuccessful();

    /// <summary>
    /// Dynamic Data Display Name calculator proxying to TestNamingConventionManager.
    /// This works when the test naming conventions are met.
    /// </summary>
    /// <param name="methodInfo">The method to work on.</param>
    /// <param name="data">The arguments, but with the convention that the last object contains the testname.</param>
    /// <returns>A calculated name of the test.</returns>
    public static string DefaultDataDisplayName(MethodInfo methodInfo, object[] data)
        => TestNamingConventionManager.DynamicDataDisplayNameAddValueFromLastArgument(methodInfo, data);

    public static IEnumerable<object?[]> ValidValues
        => [
            ["test", "TestString"],
            [new int?(5), "NullableTestInt5"],
            [1, "IntOne"],
            [2.3, "DoubleTwoPointThree"],
            [new DateTime(2023, 1, 1, 1, 1, 1, 1, 1, DateTimeKind.Utc), "DateTimeTest"],
        ];

    [TestMethod]
    [DynamicData(
        nameof(ValidValues),
        DynamicDataDisplayName = nameof(DefaultDataDisplayName))]
    public void Given_SomeSituation_When_DoingStuffWithValue_Then_ItShouldResultInNoError(object? value, string scenario)
        => this.Test(scenario, r => r
            .Given(data => data["value"] = value)
             .When(data => data["checkedValue"] = Ensure.NotNull(data["value"]))
             .Then(
                   data => data["value"].Should().Be(value),
                   data => data["checkedValue"].Should().Be(value),
                   data => data["value"].Should().BeEquivalentTo(data["checkedValue"]))
            .DebugPrint("scenario", scenario)).Should().BeSuccessful();

    [TestMethod]
    [TestCategory("external")]
    public void Given_ABrowserLoadingGoogle_When_CheckTitle_Then_TitleIsNotNull()
        => this.Test(r => r
            .GivenABrowserAt(("Google", "https://www.google.at"))
            .When(
                "Navigation to page finished",
                (browser, data) => data.Actual = browser.Title)
            .Then(
                "Title should not be null",
                (browser, data) => data.Actual.Should().NotBeNull()));

    [TestMethod]
    public void Given_DescriptiveSituation_When_DescriptiveAction_Then_NoCheckButFinalResultIsInconclusiveBecauseNoAction()
    {
        this.Test(r => r
            .Given("A situation")
            .When("Something needs to be done")
            .Then("It should have an effect")
            .ExpectInconclusive());
    }
}

```

## stylecop.json

```
{
  // ACTION REQUIRED: This file was automatically added to your project, but it
  // will not take effect until additional steps are taken to enable it. See the
  // following page for additional information:
  //
  // https://github.com/DotNetAnalyzers/StyleCopAnalyzers/blob/master/documentation/EnableConfiguration.md

  "$schema": "https://raw.githubusercontent.com/DotNetAnalyzers/StyleCopAnalyzers/master/StyleCop.Analyzers/StyleCop.Analyzers/Settings/stylecop.schema.json",
  "settings": {
    "documentationRules": {
      "companyName": "WebNativeDEV",
      "copyrightText": "Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information."
    }
  }
}
```