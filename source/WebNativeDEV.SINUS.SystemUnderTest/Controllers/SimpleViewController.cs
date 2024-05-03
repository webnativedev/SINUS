// <copyright file="SimpleViewController.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.SystemUnderTest.Controllers;

using Microsoft.AspNetCore.Mvc;
using System.Globalization;

/// <summary>
/// Represents the view controller that is responsible to deliver html code.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="SimpleViewController"/> class.
/// </remarks>
/// <param name="logger">Dependency injected logger instance.</param>
[ApiController]
[Route("/simpleView")]
[Produces("text/html")]
public class SimpleViewController(ILogger<SimpleViewController> logger) : ControllerBase
{
    private readonly ILogger<SimpleViewController> logger = logger;

    /// <summary>
    /// This function returns most basic hello world webpage content.
    /// </summary>
    /// <returns>The HTML content as string with content type html.</returns>
    [HttpGet("")]
    [Produces("text/html")]
    public ContentResult Get()
    {
        this.logger.LogInformation(
            "View Accessed Get() (TaskId: {TaskId}, ThreadId: {ThreadId})",
            Task.CurrentId?.ToString(CultureInfo.InvariantCulture) ?? " <null>",
            Environment.CurrentManagedThreadId);

        return this.Content(
            """
                <html><head><title>SINUS TestSystem</title></head>
                <body>
                    <h1 id="titleText">Hello World</h1>
                </body>
                </html>
            """, "text/html");
    }
}