// <copyright file="ZTest.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Tests.Internals.ZZZ
{
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using WebNativeDEV.SINUS.Core.MsTest;
    using WebNativeDEV.SINUS.Core.MsTest.Extensions;
    using WebNativeDEV.SINUS.Core.Sut;
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
        [TestMethod("Final Internals Summary")]
        public void Maintenance_InternalTestSummary()
        {
            TestBaseSingletonContainer.TestBaseUsageStatisticsManager.Register(this);
            this.AssertOnDataLeak().Should().BeTrue();
        }
    }
}