# SINUS (Selenium In .Net Ui Solutions)

[![Main](https://github.com/webnativedev/SINUS/actions/workflows/dotnet_main.yml/badge.svg)](https://github.com/webnativedev/SINUS/actions/workflows/dotnet_main.yml) [![CodeQL](https://github.com/webnativedev/SINUS/actions/workflows/codeql.yml/badge.svg)](https://github.com/webnativedev/SINUS/actions/workflows/codeql.yml) ![Nuget](https://img.shields.io/nuget/v/WebNativeDEV.SINUS.Core) ![Nuget](https://img.shields.io/nuget/dt/WebNativeDEV.SINUS.Core?logo=nuget) [![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=webnativedev_SINUS&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=webnativedev_SINUS) [![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=webnativedev_SINUS&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=webnativedev_SINUS) [![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=webnativedev_SINUS&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=webnativedev_SINUS) [![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=webnativedev_SINUS&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=webnativedev_SINUS) [![Bugs](https://sonarcloud.io/api/project_badges/measure?project=webnativedev_SINUS&metric=bugs)](https://sonarcloud.io/summary/new_code?id=webnativedev_SINUS) [![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=webnativedev_SINUS&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=webnativedev_SINUS)


![Example Screenshot](/docs/sinus-screenshot.png "Example Screenshot")

Main idea of this package is to provide a readable and easy way to perform web UI tests.
Hereby the package is opinionated and makes some decisions that need to be accepted for the usage of this package.
It defines a way (one of hundreds) how tests should be written and defines also a technology to use.

There are hard dependencies in SINUS to C#, .NET-Versions, MS-Test, Chrome and Selenium including BaseClasses and pre-defined patterns.
SINUS focuses on a Given-When-Then approach of writing tests (BDT - Behaviour Driven Testing) including human readable descriptions.
Accessing objects in the UI is preferred by IDs.

Example:

```csharp
    [TestMethod]
    public void Given_ATestSystem_When_StoringTheTitle_Then_ItShouldBeTheRightValue()
        => this.Test(r => r
            .GivenASystemAndABrowserAtRandomEndpoint<Program>(this.simpleView, new BrowserFactoryOptions(headless: false))
            .When((browser, data) => data.StoreActual(browser.Title))
            .Then((browser, data) => data.Should().ActualBe("SINUS TestSystem")));
```

Calling the test:

```batch
dotnet test --filter "FullyQualifiedName=WebNativeDEV.SINUS.Tests.BrowserTests.Given_ABrowserOpensGoogle_When_ReadingTheTitle_Then_TitleShouldBeSetToGoogle"
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
  * Opinionated: track code coverage automatically to be confident about the amount of tests.
    Herefore use CI/CD analysis and quality gates, but consider also local analysis.
    https://github.com/danielpalme/ReportGenerator
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

Consider:

* standard flow (positive cases)
* edge cases
* failing flows (negative cases)

(see equivalence partitioning)

### Unit Tests

Unit tests should be "FIRST" (fast, independent, repeatable, self-validating, thorough) and RITE (readable, isolated, thorough, explicit).

Do not test:

* bootstrap code (container registration, initialization)
* configuration (constants, enums, readonly fields)
* model classes and data transfer objects (DTO)
* language / framework features of the programming environment
* functions that directly return variables
* facades without any logic
* private methods
* Finalizers (especially in combination with IDisposable pattern implementation)
* exception messages and log messages

To sum it up, we are testing execution logic that can be called from outside of the unit without depending on the internal setup and implementation.

## Integration Tests

By using a custom WebApplicationFactory we can execute isolated tests in-memory. 
This mode helps in REST-based tests, but is not compatible with Selenium tests requiring a publicly available service.

Hereby we see methods that instrument the code that is checked by the integration test to be able to evaluate the code coverage.
https://learn.microsoft.com/en-us/visualstudio/test/microsoft-code-coverage-console-tool?view=vs-2022

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
* inherit the class from TestBase
* Create a method and attribute it with \[TestMethod\]
* Instead of Arrange-Act-Assert use an inherited method Test() for browser-based testing or for testing without a UI.
* With the Given part (use intelliSense) you can spin-up a browser and optionally a System-Under-Test (SUT) in-memory or public (meaning reachable via network outside the test).
  * use public SUT configuration for UI tests, because the web driver spawns outside the unit test in a separate process.
  * consider to use components for mocking / test doubles
  * Opinionated: use Moq for mocking
* With the When part (use intelliSense) you can perform the main action and here no action means a prepared test, that can not yet be evaluated properly.
* With the Then part (use intelliSense) you can perform checks on the action.
  * Consider to install an Assertion Library helping you writing more meaningful assertions.
  * Opinionated: It is not necessary to do so.
* You can optionally add some debug information at the end.
* Call dispose at then end (explicitly or via using block), because it frees the resources properly (especially network resources where network resources require additional action).
  * Opinionated: Call it explicitly. It allows you to impelement the method as expression body via ```=>```.
* Consider using StyleCop (also for the test-project)
  * Opinionated: remove CS1591, SA1600 for tests to remove the necessary XML documentation which is already covered via method name and description fields of SINUS.
* Consider using a memory leak analysis tool or directly a supporting nuget package.
  * (no own experience yet:) https://www.jetbrains.com/dotmemory/unit/ | https://www.nuget.org/packages/JetBrains.DotMemoryUnit/