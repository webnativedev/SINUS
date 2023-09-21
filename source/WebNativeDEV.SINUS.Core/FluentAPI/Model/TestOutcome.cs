// <copyright file="TestOutcome.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.FluentAPI.Model;

/// <summary>
/// Test outcomes similar to Microsoft.VisualStudio.TestPlatform.ObjectModel.TestOutcome.
/// </summary>
public enum TestOutcome
{
    /// <summary>
    /// Test run successfully.
    /// </summary>
    Success,

    /// <summary>
    /// Test has no result or not enough data to be executed.
    /// </summary>
    Inconclusive,

    /// <summary>
    /// Test fails.
    /// </summary>
    Failure,
}
