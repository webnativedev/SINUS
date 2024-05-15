using FluentAssertions;
using System.Diagnostics;
using System.Globalization;
using WebNativeDEV.SINUS.Core.Assertions;
using WebNativeDEV.SINUS.Core.FluentAPI.Model;
using WebNativeDEV.SINUS.Core.MsTest;
using WebNativeDEV.SINUS.Core.MsTest.Context;
using WebNativeDEV.SINUS.Core.UITesting.Model;
using WebNativeDEV.SINUS.Core.Utils;

namespace WebNativeDEV.SINUS.StaticTests
{
    [TestClass]
    public class StaticTesterTests
    {
        /// <summary>
        /// Gets or sets the TestContext injected by the framework.
        /// </summary>
        public TestContext TestContext { get; set; } = null!;

        [TestMethod]
        public void Given_EmptyPage_When_MakingAWebsiteWithJavascript_Then_ThePageCodeShouldBeProperlyExecutedV1()
            => StaticTester.Test(TestContext, runner => runner
                .GivenABrowserAt("empty page", "about:blank", BrowserFactoryOptions.VisibleChrome)
                .When((browser, data) =>
                {
                    browser.ExecuteScript("""document.write('<html><head><title>Test</title></head><body><button id="testbutton" onclick="document.getElementsByTagName(\'body\')[0].style.backgroundColor=\'rgba(255,128,0,1)\'">Action</button></body></html>');""");
                    browser.TakeScreenshot();
                    browser.FindElement("testbutton").Click();
                    browser.TakeScreenshot();
                })
                .Then(
                    (browser, data) => browser.Title.Should().Be("Test"),
                    (browser, data) => (browser.Url?.AbsoluteUri ?? string.Empty).Should().Be("about:blank"),
                    (browser, data) => browser.PageSource.Should().StartWith("<html>"),
                    (browser, data) => browser.HumanReadablePageName.Should().Be("empty page"),

                    (browser, data) => browser.FindElement("testbutton").Should().NotBeNull(),
                    (browser, data) => browser.FindElement("testbutton").Text.Should().Be("Action"),
                    (browser, data) => browser.FindElements("//head").Should().HaveCount(1),
                    (browser, data) => browser.FindElements("//body").Should().HaveCount(1),
                    (browser, data) => browser.FindElements("//body").First().GetCssValue("backgroundColor").Replace(" ", "", true, CultureInfo.InvariantCulture).Should().Be("rgba(255, 128, 0, 1)".Replace(" ", ""))
                )
                .DebugBreak()
            ).Outcome.Should().Be(TestOutcome.Success);
        
        [TestMethod]
        public void Given_EmptyPage_When_MakingAWebsiteWithJavascript_Then_ThePageCodeShouldBeProperlyExecutedV2()
            => StaticTester.Test(runner => runner
                .GivenABrowserAt(("empty page", "about:blank"),
                                 Debugger.IsAttached
                                    ? BrowserFactoryOptions.VisibleChrome
                                    : BrowserFactoryOptions.HeadlessChrome)
                .When((browser, data) =>
                {
                    browser.ExecuteScript("""document.write('<html><head><title>Test</title></head><body><button id="testbutton" onclick="document.getElementsByTagName(\'body\')[0].style.backgroundColor=\'rgba(255,128,0,1)\'">Action</button></body></html>');""");
                    browser.FindElement("testbutton").Click();
                })
                .Then(
                    (browser, data) => browser.Title.Should().Be("Test"),
                    (browser, data) => browser.FindElements("//body").First().GetCssValue("backgroundColor").Replace(" ", "", true, CultureInfo.InvariantCulture).Should().Be("rgba(255, 128, 0, 1)".Replace(" ", ""))
                )
                .Debug(() => Debugger.Break(), Debugger.IsAttached)
            ).Should().BeSuccessful();

        [TestMethod]
        public void Given_EmptyPage_When_MakingAWebsiteWithJavascript_Then_ThePageCodeShouldBeProperlyExecutedV3()
            => StaticTester.Test(runner => runner
                .GivenABrowserAt(("empty page", "about:blank"), BrowserFactoryOptions.HeadlessChrome)
                .When((browser, data) =>
                {
                    browser.ExecuteScript("""document.write('<html><head><title>Test</title></head><body><button id="testbutton" onclick="document.getElementsByTagName(\'body\')[0].style.backgroundColor=\'rgba(255,128,0,1)\'">Action</button></body></html>');""");
                    browser.FindElement("testbutton").Click();
                })
                .Then(
                    (browser, data) => browser.Title.Should().Be("Test"),
                    (browser, data) => browser.FindElements("//body").First().GetCssValue("backgroundColor").Replace(" ", "", true, CultureInfo.InvariantCulture).Should().Be("rgba(255, 128, 0, 1)".Replace(" ", ""))
                )
                .Debug( () => SinusUtils.RunOnlyInsideDebugSession(
                            () => SinusUtils.RunOnlyInsideVisualStudio(
                                () => Debugger.Break())))
            ).Should().BeSuccessful();

        /// <summary>
        /// Maintenance test related to zombie processes.
        /// Fails if too old processes stay on the machine.
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            StaticTester.Maintenance(StaticTesterContext.CreateStaticTest("Maintenance_ProcessesKilled"), () => SinusUtils.CountChromeZombieProcesses(2)).Should().BeSuccessful();
            StaticTester.Maintenance(() => SinusUtils.AssertOnDataLeak()).Should().BeSuccessful();
        }
    }
}