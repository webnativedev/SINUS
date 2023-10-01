# SINUS (Selenium In .Net Ui Solutions)

[![Main](https://github.com/webnativedev/SINUS/actions/workflows/dotnet_main.yml/badge.svg)](https://github.com/webnativedev/SINUS/actions/workflows/dotnet_main.yml) [![CodeQL](https://github.com/webnativedev/SINUS/actions/workflows/codeql.yml/badge.svg)](https://github.com/webnativedev/SINUS/actions/workflows/codeql.yml) ![Nuget](https://img.shields.io/nuget/v/WebNativeDEV.SINUS.Core) ![Nuget](https://img.shields.io/nuget/dt/WebNativeDEV.SINUS.Core?logo=nuget) [![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=webnativedev_SINUS&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=webnativedev_SINUS) [![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=webnativedev_SINUS&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=webnativedev_SINUS) [![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=webnativedev_SINUS&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=webnativedev_SINUS) [![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=webnativedev_SINUS&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=webnativedev_SINUS) [![Bugs](https://sonarcloud.io/api/project_badges/measure?project=webnativedev_SINUS&metric=bugs)](https://sonarcloud.io/summary/new_code?id=webnativedev_SINUS) [![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=webnativedev_SINUS&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=webnativedev_SINUS)

Main idea of this package is to provide a readable and easy way to perform web UI tests.
Hereby the package is opinionated and makes some decisions that need to be accepted for its usage.
It defines a way (one of hundreds) how tests should be written and defines also the technology to use.
Additionally a lot of checks are added to guarantee consistency and preventing memory leaks.

Dependencies:

* MS Test
* Microsoft.AspNetCore.Mvc.Testing
* Microsoft.Extensions.Logging...
* FluentAssertions
* NSubstitute
* Selenium

![Example Screenshot](/docs/sinus-screenshot.png "Example Screenshot")

There are hard dependencies in SINUS to C#, newest .NET-Versions, MS-Test and Selenium (including partly Chrome) using BaseClasses and pre-defined patterns.
SINUS focuses on a Given-When-Then approach of writing tests (BDT - Behaviour Driven Testing) to provide human readable descriptions with few effort.

Example:

```csharp
    [TestMethod]
    public void Given_ATestSystem_When_StoringTheTitle_Then_ItShouldBeTheRightValue()
        => this.Test(r => r
            .GivenASystemAndABrowserAtRandomEndpoint<Program>("SimpleView", "/simpleView")
            .When((browser, data) => data.StoreActual(browser.Title))
            .Then((browser, data) => data.Should().ActualBe("SINUS TestSystem"))
            .DebugPrint());
```

Calling the test:

```batch
dotnet test --filter "FullyQualifiedName=WebNativeDEV.SINUS.Tests.SimpleBrowserTests.Given_ATestSystem_When_StoringTheTitle_Then_ItShouldBeTheRightValue"
```

Debug standard output:

```text
    ...

    +-----------------------------
    | Start: (TaskId:  <null>, ThreadId: 16)
    |      Args: 
    |          * --urls=https://localhost:10012
    |          * --isKestrelHost=yes
    |          * --ExecutionMode=Mock
    |          * --environment=Development
    |          * --contentRoot=C:\Users\kienb\source\repos\webnativedev\SINUS\source\WebNativeDEV.SINUS.SystemUnderTest
    |          * --applicationName=WebNativeDEV.SINUS.SystemUnderTest
    |      Mocking: activated
    |      Start Failing: deactivated
    |      ExternalArgs: 
    +-----------------------------

    ...

Given: A Test System (TaskId: 1, ThreadId: 4)
    Create Browser requested for https://localhost:10012/simpleView
    Create driver instance for chrome with options 'Headless: True, IgnoreSslErrors: True'
    write chromedriverservice-logs to: ...\chromedriverservices_2023-09-30__11-29-11-1992760.log
info: WebNativeDEV.SINUS.SystemUnderTest.Controllers.ViewController[0]
      View Accessed Get() (TaskId:  <null>, ThreadId: 10)
    Broswer object created OpenQA.Selenium.Chrome.ChromeDriver - logger: Microsoft.Extensions.Logging.Logger - content: ...\In\[HOSTNAME]
= 4382 ms (TaskId: 1, ThreadId: 4)

When: Storing The Title (TaskId: 1, ThreadId: 4)
    Title requested SINUS TestSystem
= 18 ms (TaskId: 1, ThreadId: 4)

Then: It Should Be The Right Value (TaskId: 1, ThreadId: 4)
= 23 ms (TaskId: 1, ThreadId: 4)

Debug: Debug (TaskId: 1, ThreadId: 4)
    Content dump:
       +----------------------------
       | Count: 1
       +----------------------------
       | actual: SINUS TestSystem (Type: System.String)
       +----------------------------
= 24 ms (TaskId: 1, ThreadId: 4)

Close: Close (TaskId: 1, ThreadId: 4)
    Driver quitted
    
    ...
    
    +-----------------------------
    | Shutdown: (TaskId:  <null>, ThreadId: 14)
    |      Args: 
    |          * --urls=https://localhost:10012
    |          * --ExecutionMode=Mock
    |          * --environment=Development
    |          * --contentRoot=C:\Users\kienb\source\repos\webnativedev\SINUS\source\WebNativeDEV.SINUS.SystemUnderTest
    |          * --applicationName=WebNativeDEV.SINUS.SystemUnderTest
    |      Mocking: activated
    +-----------------------------
= 514 ms (TaskId: 1, ThreadId: 4)

The test result is evaluated as successful for test 'Given_ATestSystem_When_StoringTheTitle_Then_ItShouldBeTheRightValue'. (Checked Exceptions: 0, TaskId: 1, ThreadId: 4)
```

By reducing the complexity you are not able to use all of the features Selenium has.
Nevertheless, the most common features are supported and covered by SINUS in a nicely shaped API that guides you through the test case and will add logging so that errors can be found simpler without any effort on the test-developer side.

[Getting Started](./docs/GettingStarted.md) | [Strategy](./docs/TestStrategy.md)
