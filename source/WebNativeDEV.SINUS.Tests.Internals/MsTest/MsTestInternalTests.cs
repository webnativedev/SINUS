// <copyright file="MsTestInternalTests.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Tests.Internals.MsTest;

using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebNativeDEV.SINUS.Core.MsTest;
using WebNativeDEV.SINUS.Core.MsTest.Contracts;
using WebNativeDEV.SINUS.MsTest;
using WebNativeDEV.SINUS.Core.Assertions;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Type_or_Member'.
#pragma warning disable SA1600 // Elements should be documented

public class MsTestInternalTests : TestBase
{
    [TestMethod]
    public void Given_AssemblyTestContext_When_Creating_Then_DataShouldBeStored()
    => this.Test(r => r
        .GivenASimpleSystem(new AssemblyTestContext(Substitute.For<TestContext>()))
        .When<IAssemblyTestContext>((sut, data) => data.Actual = sut.TestContext)
        .Then(data => data.Should().ActualBeNotNull()));
}
