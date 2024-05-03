// <copyright file="TechnicalRequirementsAttribute.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Requirements;

using System;

/// <summary>
/// An attribute that represents technical requirements that need to be implemented.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="TechnicalRequirementsAttribute"/> class.
/// </remarks>
/// <param name="capability">The main capability.</param>
/// <param name="requirements">A description that shows the purpose.</param>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class TechnicalRequirementsAttribute(string capability, params string[] requirements) : Attribute
{
    /// <summary>
    /// Gets the name of the main capability.
    /// </summary>
    public string Capability { get; } = capability;

    /// <summary>
    /// Gets the requirements that show what is needed to enable the capability.
    /// </summary>
    public string[] Requirements { get; } = requirements;
}
