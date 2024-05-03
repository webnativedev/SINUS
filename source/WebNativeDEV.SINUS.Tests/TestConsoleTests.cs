// <copyright file="TestConsoleTests.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebNativeDEV.SINUS.TestConsole;
using WebNativeDEV.SINUS.MsTest;
using FluentAssertions;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Type_or_Member'.
#pragma warning disable SA1600 // Elements should be documented

[TestClass]
public class TestConsoleTests : TestBase
{
    [TestMethod]
    public void Given_TestConsole_When_ExecutingProgramCS_Then_NoErrorsShouldBeSeen()
    {
        // arrange
        var sut = () => Program.Main(["test"]);

        // act + assert
        sut.Should().NotThrow();
    }

    [TestMethod]
    public void Given_TestConsole_When_ExecutingProgramCSWithWronglyExpectedOutcome_Then_ErrorsShouldBeSeen()
    {
        // arrange
        var sut = () => Program.Main(["not test"]);

        // act + assert
        sut.Should().Throw<Exception>();
    }
}
