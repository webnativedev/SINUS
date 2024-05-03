// See https://aka.ms/new-console-template for more information
using WebNativeDEV.SINUS.Core.MsTest;
using WebNativeDEV.SINUS.Core.UITesting.Model;
using WebNativeDEV.SINUS.Core.Utils;

namespace WebNativeDEV.SINUS.TestConsole;

public class Program
{
    public static void Main(string[] args)
        => StaticTester.Test(r =>
            r
                .GivenABrowserAt("about:blank", new BrowserFactoryOptions(headless: args.Length != 0, ignoreSslErrors: true))
                .When((browser, store) => browser.ExecuteScript("document.title = 'test'"))
                .Then((browser, store) =>
                {
                    if (browser.Title != (args.FirstOrDefault("test")))
                    {
                        throw new InvalidDataException("title not accepted");
                    }
                }));
}