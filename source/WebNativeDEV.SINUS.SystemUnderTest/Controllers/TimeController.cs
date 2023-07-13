// <copyright file="TimeController.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.SystemUnderTest.Controllers;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebNativeDEV.SINUS.SystemUnderTest.Services.Abstractions;

/// <summary>
/// Public controller that allows access to operations depending on time.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class TimeController : ControllerBase
{
    private readonly ITimeProvider timeProvider;
    private readonly ILogger<TimeController> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TimeController"/> class.
    /// </summary>
    /// <param name="timeProvider">Provider managing the external dependency to the clock.</param>
    /// <param name="logger">Logger created to write further information.</param>
    public TimeController(ITimeProvider timeProvider, ILogger<TimeController> logger)
    {
        this.timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
        this.logger = logger;
    }

    /// <summary>
    /// Calculates the square of an integer without further control of overflows.
    /// </summary>
    /// <returns>The integer result of the mathematical calculation.</returns>
    [HttpGet("/time/sec")]
    public int GetSeconds()
    {
        this.logger.LogInformation("Check for seconds in provider {Provider}", this.timeProvider.ToString());
        return this.timeProvider.GetCurrentSeconds();
    }
}
