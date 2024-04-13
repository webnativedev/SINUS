using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebNativeDEV.SINUS.TestConsole;
using WebNativeDEV.SINUS.MsTest;
using FluentAssertions;
using NSubstitute.ExceptionExtensions;

namespace WebNativeDEV.SINUS.Tests
{
    [TestClass]
    public class TestConsoleTests : TestBase
    {
        [TestMethod]
        public void Given_TestConsole_When_ExecutingProgramCS_Then_NoErrorsShouldBeSeen()
        {
            // arrange
            var sut = () => Program.Main(new[] { "test" });

            // act + assert
            sut.Should().NotThrow();
        }

        [TestMethod]
        public void Given_TestConsole_When_ExecutingProgramCSWithWronglyExpectedOutcome_Then_ErrorsShouldBeSeen()
        {
            // arrange
            var sut = () => Program.Main(new[] { "not test" });

            // act + assert
            sut.Should().Throw<Exception>();
        }
    }
}
