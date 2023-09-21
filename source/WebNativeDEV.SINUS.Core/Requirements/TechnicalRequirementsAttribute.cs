// <copyright file="TechnicalRequirementsAttribute.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Requirements;

using System;

/// <summary>
/// An attribute that represents technical requirements that need to be implemented.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class TechnicalRequirementsAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TechnicalRequirementsAttribute"/> class.
    /// </summary>
    /// <param name="capability">The main capability.</param>
    /// <param name="requirements">A description that shows the purpose.</param>
    public TechnicalRequirementsAttribute(string capability, params string[] requirements)
    {
        this.Capability = capability;
        this.Requirements = requirements;
    }

    /// <summary>
    /// Gets the name of the main capability.
    /// </summary>
    public string Capability { get; }

    /// <summary>
    /// Gets the requirements that show what is needed to enable the capability.
    /// </summary>
    public string[] Requirements { get; }
}
