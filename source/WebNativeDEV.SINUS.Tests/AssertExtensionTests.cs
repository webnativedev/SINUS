// <copyright file="AssertExtensionTests.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebNativeDEV.SINUS.Core.MsTest.Assertions;
using WebNativeDEV.SINUS.MsTest;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Type_or_Member'.
#pragma warning disable SA1600 // Elements should be documented

[TestClass]
public class AssertExtensionTests : TestBase
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
    [ExpectedException(typeof(AssertFailedException))]
    public void Given_AssertionExtensions_When_ExecutionOfNoException_Then_ActionWithNullThrowsAnError()
        => this.Test()
            .Given("Assert class extended")
            .When("Running assert with null", data => Assert.That.NoExceptionOccurs(null!))
            .Then("exception should be thrown")
            .Dispose();

    [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
    [ExpectedException(typeof(AssertFailedException))]
    public void Given_AssertionExtensions_When_ExecutionOfAreEqualToActual_Then_ActionWithNullThrowsAnError()
        => this.Test()
            .Given("Assert class extended and actual value available", data => data.StoreActual("test"))
            .When("Running assert with null", data => Assert.That.AreEqualToActual<string?>(data, null!))
            .Then("exception should be thrown")
            .Dispose();

    [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
    [ExpectedException(typeof(AssertFailedException))]
    public void Given_AssertionExtensions_When_ExecutionOfAreEqualToActual_Then_StoreWithNullThrowsAnError()
        => this.Test()
            .Given("Assert class extended and actual value available", data => data.StoreActual("test"))
            .When("Running assert with null", data => Assert.That.AreEqualToActual(null!, "test"))
            .Then("exception should be thrown")
            .Dispose();
}
