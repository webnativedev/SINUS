// <copyright file="BrowserTestBase.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.MsTest.Abstract;

using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WebNativeDEV.SINUS.Core.FluentAPI;
using WebNativeDEV.SINUS.Core.FluentAPI.Contracts;
using WebNativeDEV.SINUS.Core.UITesting.Contracts;

/// <summary>
/// This class represents a testbase that includes references to browser objects.
/// </summary>
[TestClass]
public abstract class BrowserTestBase : TestBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BrowserTestBase"/> class.
    /// </summary>
    /// <param name="createBrowserFactory">A reference to a creational method.</param>
    protected BrowserTestBase(Func<string, string, ILoggerFactory, IBrowserFactory> createBrowserFactory)
        : base()
    {
        this.Factory = createBrowserFactory?.Invoke(
            this.RunDir,
            this.LogsDir,
            this.LoggerFactory) as IBrowserFactory
            ?? throw new ArgumentOutOfRangeException(nameof(createBrowserFactory));
    }

    /// <summary>
    /// Gets the browser factory.
    /// </summary>
    private IBrowserFactory Factory { get; }

    /// <summary>
    /// Creates a Runner object to run Tests on.
    /// </summary>
    /// <returns>An object of runner.</returns>
    protected new IBrowserRunner Test() => new BrowserRunner(this.LoggerFactory, this.Factory);
}
