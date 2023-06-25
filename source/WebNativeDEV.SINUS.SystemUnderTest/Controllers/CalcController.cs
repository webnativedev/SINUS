// <copyright file="CalcController.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.SystemUnderTest.Controllers;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Calculation Controller.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class CalcController : ControllerBase
{
    /// <summary>
    /// Calculates the square of an integer without further control of overflows.
    /// </summary>
    /// <param name="a">Number that should be squared.</param>
    /// <returns>The integer result of the mathematical calculation.</returns>
    [HttpGet("/calc/{a}")]
    public int CalculateSquare(int a) => a * a;
}
