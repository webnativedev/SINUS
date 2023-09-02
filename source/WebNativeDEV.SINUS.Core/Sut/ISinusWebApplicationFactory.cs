// <copyright file="ISinusWebApplicationFactory.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Sut;

/// <summary>
/// Non generic interface that can be used as a reference for generated systems.
/// </summary>
public interface ISinusWebApplicationFactory : IDisposable
{
    /// <summary>
    /// Creates a client related to the web application that is created in memory.
    /// </summary>
    /// <returns>An http client communicating with the SUT.</returns>
    HttpClient? CreateClient();

    /// <summary>
    /// Closes the additional public hosts created.
    /// </summary>
    void CloseCreatedHost();
}