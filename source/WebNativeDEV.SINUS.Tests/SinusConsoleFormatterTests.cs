// <copyright file="SinusConsoleFormatterTests.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Tests;

using global::WebNativeDEV.SINUS.MsTest;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebNativeDEV.SINUS.Core.MsTest;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Type_or_Member'.
#pragma warning disable SA1600 // Elements should be documented

[TestClass]
public class SinusConsoleFormatterTests : TestBase
{
    [TestMethod]
    public void Given_Formatter_When_CreatingAFormatter_Then_ItShouldBeAbleToFormatMessages()
    {
        this.RunFormatterTest();
    }

    [TestMethod]
    public void Given_Formatter_When_CreatingAFormatterAndChangeOption_Then_ItShouldBeAbleToFormatMessages()
    {
        this.RunFormatterTest(changeOption: true);
    }

    [TestMethod]
    public void Given_Formatter_When_CreatingAFormatter_Then_ItShouldSilentlyStopWhenTextWriterIsNull()
    {
        this.RunFormatterTest(setTextWriterNull: true, expectedResult: string.Empty);
    }

    [TestMethod]
    public void Given_Formatter_When_CreatingAFormatter_Then_ItShouldSilentlyStopWhenFormatterIsNull()
    {
        this.RunFormatterTest(setFormatterNull: true, expectedResult: string.Empty);
    }

    [TestMethod]
    public void Given_Formatter_When_CreatingAFormatter_Then_ItShouldSilentlyStopWhenFormatterAndTextWriterIsNull()
    {
        this.RunFormatterTest(setTextWriterNull: true, setFormatterNull: true, expectedResult: string.Empty);
    }

    private void RunFormatterTest(
        bool setTextWriterNull = false,
        bool setFormatterNull = false,
        bool changeOption = false,
        string expectedResult = "test: state")
    {
        var stream = new MemoryStream();

        TextWriter? writer = new StreamWriter(stream);
        if (setTextWriterNull)
        {
            writer = null;
        }

        Func<string, Exception?, string>? formatter = (state, exc) => "test: " + state;

        if (setFormatterNull)
        {
            formatter = null;
        }

        var logEntry =
            new LogEntry<string>(
                Microsoft.Extensions.Logging.LogLevel.Critical,
                "category",
                new Microsoft.Extensions.Logging.EventId(5, "eventId5"),
                "state",
                null,
                formatter!);

        var config = new ConfigureOptions<ConsoleFormatterOptions>(
            (options) => new ConsoleFormatterOptions()
            {
                IncludeScopes = false,
            });

        var postConfig = new PostConfigureOptions<ConsoleFormatterOptions>(
            "abc",
            (options) => new ConsoleFormatterOptions());
        var optionsFactory = new OptionsFactory<ConsoleFormatterOptions>(new[] { config }, new[] { postConfig });

        var configuration = new ConfigurationBuilder().Build();
        var tokenSource = new ConfigurationChangeTokenSource<ConsoleFormatterOptions>(configuration);
        var optionsCache = new OptionsCache<ConsoleFormatterOptions>();

        var optionsMonitor = new OptionsMonitor<ConsoleFormatterOptions>(
            optionsFactory,
            new[] { tokenSource },
            optionsCache);

        this.Test()
            .Given(
                "Creating a formatter",
                data => data.StoreSut(new SinusConsoleFormatter(optionsMonitor)))
            .When(
                "writing a message to TextWriter",
                data =>
                {
                    data.ReadSut<SinusConsoleFormatter>().Write(logEntry, null, writer!);
                    if (changeOption)
                    {
                        configuration.Reload();
                    }
                })
            .Then(
                $"it should be as expected '{expectedResult}'",
                data =>
                {
                    writer?.Flush();
                    var content = Encoding.ASCII.GetString(stream.ToArray()).ReplaceLineEndings(string.Empty).Trim();
                    Assert.AreEqual(expectedResult, content, false);
                })
            .DebugPrint()
            .Dispose();
    }
}
