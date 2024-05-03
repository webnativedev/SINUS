// <copyright file="TimeController.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.SystemUnderTest.Controllers;

using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Threading.Tasks;
using WebNativeDEV.SINUS.SystemUnderTest.Services.Abstractions;

/// <summary>
/// Public controller that allows access to operations depending on time.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="TimeController"/> class.
/// </remarks>
/// <param name="timeProvider">Provider managing the external dependency to the clock.</param>
/// <param name="logger">Logger created to write further information.</param>
[Route("/time/")]
[ApiController]
public class TimeController(ITimeProvider timeProvider, ILogger<TimeController> logger) : ControllerBase
{
    private readonly ITimeProvider timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));

    /// <summary>
    /// Calculates the square of an integer without further control of overflows.
    /// </summary>
    /// <returns>The integer result of the mathematical calculation.</returns>
    [HttpGet("sec")]
    public int GetSeconds()
    {
        logger.LogInformation(
            "Check for seconds in provider {Provider} (TaskId: {TaskId}, ThreadId: {ThreadId})",
            this.timeProvider.ToString(),
            Task.CurrentId?.ToString(CultureInfo.InvariantCulture) ?? " <null>",
            Environment.CurrentManagedThreadId);

        return this.timeProvider.GetCurrentSeconds();
    }
}
