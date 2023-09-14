// <copyright file="ITestBaseResult.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.MsTest.Contracts;

/// <summary>
/// Result of the main test method.
/// </summary>
public interface ITestBaseResult
{
    /// <summary>
    /// Gets a value indicating whether the state is successful.
    /// </summary>
    bool Success { get; }
}