// <copyright file="ChromeTestBase.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.MsTest.Chrome;

using WebNativeDEV.SINUS.Core.UITesting.Chrome;
using WebNativeDEV.SINUS.MsTest.Abstract;

/// <summary>
/// Defines a Test base defined for chrome including different non-generic references.
/// </summary>
public abstract class ChromeTestBase : BrowserTestBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChromeTestBase"/> class.
    /// </summary>
    protected ChromeTestBase()
        : base((runDir, logsDir, loggerFactory) => new ChromeBrowserFactory(runDir, logsDir, loggerFactory))
    {
    }
}
