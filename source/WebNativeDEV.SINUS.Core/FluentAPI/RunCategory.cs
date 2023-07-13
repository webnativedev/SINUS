// <copyright file="RunCategory.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.FluentAPI
{
    /// <summary>
    /// Categories of a test run.
    /// </summary>
    internal enum RunCategory
    {
        /// <summary>
        /// Arrange part constructs a situation.
        /// </summary>
        Given,

        /// <summary>
        /// Act part that executes a test.
        /// </summary>
        When,

        /// <summary>
        /// Asserts for an expected value.
        /// </summary>
        Then,

        /// <summary>
        /// Can be used to display further information for debugging.
        /// </summary>
        Debug,

        /// <summary>
        /// Removes all elements (especially required for unmanaged resources like the WebDriver).
        /// </summary>
        Dispose,
    }
}
