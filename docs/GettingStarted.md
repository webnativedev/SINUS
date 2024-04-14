# Getting Started

* Create a test project
* install SINUS via nuget
  * SINUS can be used for UI-Tests and Unit Tests
  * consider to not create any other test project for testing the externally visible API
    (implementing small UI wrappers enables manual testing that is fully automatable).
  * You might want to create another project for internal APIs using the AssemblyAttributes including System.Runtime.CompilerServices.InternalsVisibleTo.
* (Optionally) Create a "TestInitializer" (or similar named class) that covers the startup of a test
  * AssemblyInitialize, ClassInitialize - usage example can be seen at (TestInitializer.cs in the demo test project).
* Create a class and attribute it with \[TestClass\]
* inherit the class from TestBase
* Create a method and attribute it with \[TestMethod\]
* Instead of Arrange-Act-Assert use an inherited method Test() for browser-based testing or for testing without a UI.
* With the Given part (use intelliSense) you can spin-up a browser and optionally a System-Under-Test (SUT) in-memory or public (meaning reachable via network outside the test).
  * use public SUT configuration for UI tests, because the web driver spawns outside the unit test in a separate process.
  * consider to use components for mocking / test doubles and mocking modes in the systems to test
* With the When part (use intelliSense) you can perform the main action and here no action means a prepared test, that can not yet be evaluated properly.
* With the Then part (use intelliSense) you can perform checks on the action.
  * FluentAssertions is supported and comes as dependency as mainly supported Assertion Library helping you writing tests more expressive.
* You can optionally add some debug information at the end in the "Debug" phase (mostly used for printing the current state; this is supported by DebugPrint).
* Consider using StyleCop (also for the test-project)
  * Opinionated: remove CS1591, SA1600 for tests to remove the necessary XML documentation which is already covered via method name and description fields of SINUS.
* Consider using a memory leak analysis tool or directly a supporting nuget package.
  * (no own experience yet:) https://www.jetbrains.com/dotmemory/unit/ | https://www.nuget.org/packages/JetBrains.DotMemoryUnit/
  * Optional: SINUS supports you in finding memory leaks for browser and spinned up system-under-test-instances. Additionally zombie chrome processes can be observed and also the test results folder for the amount of log folders.

```
// <copyright file="SimpleBrowserTests.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Tests;

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebNativeDEV.SINUS.Core.Assertions;
using WebNativeDEV.SINUS.Core.MsTest;
using WebNativeDEV.SINUS.Core.UITesting.Model;
using WebNativeDEV.SINUS.Core.Utils;
using WebNativeDEV.SINUS.MsTest;
using WebNativeDEV.SINUS.SystemUnderTest;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Type_or_Member'.
#pragma warning disable SA1600 // Elements should be documented

[TestClass]
public class SimpleBrowserTests : TestBase
{
    [TestMethod]
    public void Given_ABlankWebsite_When_StoringTheTitle_Then_ItShouldNotBeNull()
        => this.Test(r => r
            .GivenABrowserAt(("empty page", "about:blank"))
            .When("storing the title", (browser, data) => data.StoreActual(browser?.Title))
            .Then("it should equal to the real title", (browser, data) => Assert.IsNotNull(data.ReadActual<string>())));
}
```

Consider that the project's unit test project demonstrate all capabilities.