// See https://aka.ms/new-console-template for more information
using WebNativeDEV.SINUS.Core.MsTest;
using WebNativeDEV.SINUS.Core.UITesting.Model;
using WebNativeDEV.SINUS.Core.Utils;

StaticTester.Test(r =>
    r
        .GivenABrowserAt("about:blank", new BrowserFactoryOptions(headless: false, ignoreSslErrors: true))
        .When((browser, store) => browser.ExecuteScript("document.title = 'test'"))
        .Then((browser, store) =>
        {
            if (browser.Title != "test")
            {
                throw new InvalidDataException("title not accepted");
            }
        }));

SinusUtils.KillChromeZombieProcesses(2);