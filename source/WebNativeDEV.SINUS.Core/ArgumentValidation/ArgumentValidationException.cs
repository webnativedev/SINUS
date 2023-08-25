// <copyright file="ArgumentValidationException.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.ArgumentValidation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Exception in case an argument is not valid.
/// </summary>
public class ArgumentValidationException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ArgumentValidationException"/> class.
    /// </summary>
    /// <param name="validationMethod">Name of the Ensure Method.</param>
    /// <param name="item">The item validated.</param>
    /// <param name="name">The argument name.</param>
    public ArgumentValidationException(string validationMethod, object? item, string? name)
        : base($"Validation Exception: {validationMethod} for {name ?? "unknown"} with value {item ?? "<null>"}")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ArgumentValidationException"/> class.
    /// </summary>
    public ArgumentValidationException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ArgumentValidationException"/> class.
    /// </summary>
    /// <param name="message">A plain text message to add.</param>
    public ArgumentValidationException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ArgumentValidationException"/> class.
    /// </summary>
    /// <param name="message">A plain text message to add.</param>
    /// <param name="innerException">An inner exception.</param>
    public ArgumentValidationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
