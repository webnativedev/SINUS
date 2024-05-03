// <copyright file="RunStorePrintOrder.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.FluentAPI.Model;

/// <summary>
/// Run Store can be sorted by the following values.
/// It is recommended to sort after key.
/// </summary>
public enum RunStorePrintOrder
{
    /// <summary>
    /// No sorting.
    /// </summary>
    Unsorted = 0,

    /// <summary>
    /// Sort RunStore data after key before printing.
    /// </summary>
    KeySorted = 1,

    /// <summary>
    /// Sort RunStore data after value before printing.
    /// </summary>
    ValueSorted = 2,
}
