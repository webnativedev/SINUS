# SINUS (Selenium In .Net Ui Solutions)

[![Main-Branch-Flow](https://github.com/webnativedev/SINUS/actions/workflows/dotnet_main.yml/badge.svg)](https://github.com/webnativedev/SINUS/actions/workflows/dotnet_main.yml)
[![CodeQL](https://github.com/webnativedev/SINUS/actions/workflows/codeql.yml/badge.svg)](https://github.com/webnativedev/SINUS/actions/workflows/codeql.yml)

Main idea of this package is to provide a readable and easy way to perform UI tests.
Hereby the package is opinionated and makes some decisions that need to be accepted for the usage of this package.
It defines a way (one of hundreds) how tests should be written and defines also a technology to use.

There are hard dependencies in SINUS to C#, .NET-Versuibs, MS-Test, Chrome and Selenium including BaseClasses and pre-defined patterns.
SINUS focuses on a Given-When-Then approach of writing tests (BDT - Behaviour Driven Testing) including required human readable descriptions.
Accessing objects in the UI is preferred by IDs.

Example:

```csharp
    [TestMethod]
    public void Given_ABrowserOpensGoogle_When_ReadingTheTitle_Then_TitleShouldBeSet()
        => this.UITest()
            .GivenABrowserAt("Google", "http://www.google.at")
            .When("Reading the title", (browser, data) => data["Title"] = browser.Title)
            .Then("Title should be set", (browser, data) => Assert.IsNotNull(data["Title"]))
            .Dispose();
```

By reducing the complexity you are not able to use all of the features Selenium has.
Nevertheless, the most common features are supported and covered by SINUS in a nicely shaped API that guides you through the test case.

## Strategy

Creating automated tests makes a lot of sense, because it enables you to get feedback
about your development process even before you hand over the application to the
quality assurance and later your customers. In software engineering we refer here to
the term "shift-left" or "fail-fast", what is an ultimately good thing and improvement
in the industry of software development.

* Create a lot of unit tests.
  * Opinionated: SINUS can be your one-stop-shop library for testing
    based on MS-Test in a nice BDT API, but consider that you will introduce also
    some dependencies to libraries that need to considered (e.g.: Selenium).
* create some service tests.
  * Opinionated: SINUS will help you with that task as well, but it might be
    beneficial for the overall process to create a small UI to test. A lot
    of applications have these capabilities via Swagger/OpenAPI-Frontends.
* create few UI tests.
  * Opinionated: SINUS is primarily built for this task, but use the amount of
    tests with caution. UIs are constantly changing, so keep your IDs to capture
    to reduce the amount of test refactoring effort.

Quality criterias are:

* fully automated (including result interpretation)
* has full control
* isolated (preferred in-memory)
  * parallelizable
* stable (not flaky) in its result
* running fast
* testing a single concept in the system
* readable
* maintainable
* trustworthy

### Unit Tests

Unit tests should be "FIRST" (fast, isolated/independent, repeatable, self-validating, thorough).

* bootstrap code (container registration, initialization)
* configuration (constants, enums, readonly fields)
* model classes and data transfer objects (DTO)
* language / framework features of the programming environment
* functions that directly return variables
* facades without any logic
* private methods
* exception messages and log messages

To sum it up, we are testing execution logic that can be called from outside of the unit without depending on the internal setup and implementation.

## Getting Started

* Create a test project
* install SINUS via nuget
  * SINUS can be used for UI-Tests and Unit Tests
  * consider to not create any other integration test project 
    (implementing small UI wrappers enables manual testing that is fully automatable).
* (Optionally) Create a "TestInitializer" (or similar named class) that covers the startup of a test
  * AssemblyInitialize, ClassInitialize - usage example can be seen at (TestInitializer.cs in the demo test project).
  * Opinionated: don't see this as optional... in case the test-contexts are not captured via these methods unproper \
    file handling could be the result (e.g.: logs in the folder of execution instead of in the testresults).
* Create a class and attribute it with \[TestClass\]
* inherit the class from ChromeTestBase
* Create a method and attribute it with \[TestMethod\]
* Instead of Arrange-Act-Assert use an inherited method UITest() for browser-based testing OR Test() for testing without a UI.
* With the Given part (use intelliSense) you can spin-up a browser and optionally a System-Under-Test (SUT) in-memory or public (meaning reachable via network outside the test).
  * use public SUT configuration for UI tests, because the web driver spawns outside the unit test in a separate process.
  * consider to use components for mocking / test doubles
* With the When part (use intelliSense) you can perform an action
* With the Then part (use intelliSense) you can perform checks on the action.
  * Consider to install an Assertion Library helping you writing more meaningful assertions.
  * Opinionated: It is not necessary to do so.
* You can optionally add some debug information at the end.
* Call dispose at then end (explicitly or via using block), because it frees the resources properly (especially network resources where network resources require additional action).
  * Opinionated: Call it explicitly. It allows you to impelement the method as expression body via ```=>```.
* Consider using StyleCop (also for the test-project)
  * Opinionated: remove CS1591, SA1600 for tests to remove the necessary XML documentation which is already covered via method name and description fields of SINUS.
