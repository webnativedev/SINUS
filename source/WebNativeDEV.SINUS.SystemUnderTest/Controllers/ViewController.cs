// <copyright file="ViewController.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.SystemUnderTest.Controllers;

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Represents the view controller that is responsible to deliver html code.
/// </summary>
[ApiController]
[Route("[controller]")]
[Produces("text/html")]
public class ViewController : ControllerBase
{
    private readonly ILogger<ViewController> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ViewController"/> class.
    /// </summary>
    /// <param name="logger">Dependency injected logger instance.</param>
    public ViewController(ILogger<ViewController> logger)
    {
        this.logger = logger;
    }

    /// <summary>
    /// This function returns most basic hello world webpage content.
    /// </summary>
    /// <returns>The HTML content as string with content type html.</returns>
    [HttpGet("/simpleView")]
    [Produces("text/html")]
    public ContentResult Get()
    {
        this.logger.LogDebug("View Accessed Get()");
        return this.Content(
            """
                <html><head><title>SINUS TestSystem</title></head>
                <body>
                    <h1>Hello World</h1>
                </body>
                </html>
            """, "text/html");
    }
}