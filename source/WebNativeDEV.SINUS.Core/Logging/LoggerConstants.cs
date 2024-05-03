// <copyright file="LoggerConstants.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Logging;

/// <summary>
/// Container for globally repeating constants.
/// </summary>
public static class LoggerConstants
{
    /// <summary>
    /// Separation line string constant.
    /// </summary>
    public const string SeparationLine = "+--------------------------------";

    /// <summary>
    /// Indented separation line string constant.
    /// </summary>
    public const string IndentedSeparationLine = "    +-----------------------------";

    /// <summary>
    /// Null string.
    /// </summary>
    public const string NullString = "<null>";

    /// <summary>
    /// None string.
    /// </summary>
    public const string NoneString = "<none>";

    /// <summary>
    /// Gets a new line sign.
    /// </summary>
    public static string NewLine => Environment.NewLine;

    /// <summary>
    /// Gets a separation and a new line sign.
    /// </summary>
    public static string SeparationLineWithNewLine => SeparationLine + Environment.NewLine;
}
