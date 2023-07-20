// <copyright file="TimeProvider.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.SystemUnderTest.Services;

using WebNativeDEV.SINUS.SystemUnderTest.Services.Abstractions;

/// <summary>
/// Provides data dependent on external dependency "clock".
/// </summary>
public class TimeProvider : ITimeProvider
{
    /// <inheritdoc/>
    public int GetCurrentSeconds()
        => DateTime.Now.Second;

    /// <inheritdoc/>
    public override string ToString()
    {
        return "TimeProvider: " + DateTime.Now.ToLongTimeString();
    }
}
