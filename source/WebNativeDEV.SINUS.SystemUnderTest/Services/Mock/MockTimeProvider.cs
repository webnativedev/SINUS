// <copyright file="MockTimeProvider.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.SystemUnderTest.Services.Mock;

using WebNativeDEV.SINUS.SystemUnderTest.Services.Abstractions;

/// <summary>
/// Mocked provider for the external dependency clock.
/// </summary>
public class MockTimeProvider : ITimeProvider
{
    private static readonly DateTime MockedTimeStamp = new (2023, 4, 4, 17, 08, 59);

    /// <inheritdoc/>
    public int GetCurrentSeconds() => MockedTimeStamp.Second;

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"MockTimeProvider ({MockedTimeStamp.ToLongTimeString()})";
    }
}
