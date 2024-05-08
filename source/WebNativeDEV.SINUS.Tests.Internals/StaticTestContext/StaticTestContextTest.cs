// <copyright file="StaticTestContextTest.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Tests.Internals.Execution;

using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
using System.Reflection;
using WebNativeDEV.SINUS.Core.Execution;
using WebNativeDEV.SINUS.Core.Execution.Model;
using WebNativeDEV.SINUS.Core.FluentAPI.Model;
using WebNativeDEV.SINUS.Core.MsTest;
using WebNativeDEV.SINUS.Core.MsTest.Context;
using WebNativeDEV.SINUS.MsTest;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Type_or_Member'.
#pragma warning disable SA1600 // Elements should be documented

[TestClass]
public class StaticTestContextTest : TestBase
{
    [TestMethod]
    public void Given_StaticTestContext_When_CreatingAContext_Then_AllActionsShouldRunWithoutException()
    {
        // arrange
        var context = StaticTesterContext.CreateStaticTest();

        // act
        Action action = () =>
        {
            context.AddResultFile("some name");
            context.Write("test 1");
            context.Write("test {Nr}", 2);
            context.WriteLine("test 3");
            context.WriteLine("test {Nr}", 4);
        };

        // assert
        action.Should().NotThrow();
    }
}
