// <copyright file="ITimeProvider.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.SystemUnderTest.Services.Abstractions;

/// <summary>
/// Interface that abstracts the dependency to the clock.
/// </summary>
public interface ITimeProvider
{
    /// <summary>
    /// Retruns the amount of seconds of the current timestamp.
    /// </summary>
    /// <returns>Seconds as integer between 0 and 59.</returns>
    int GetCurrentSeconds();
}
