// <copyright file="BusinessRequirementsAttribute.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Requirements;

using System;

/// <summary>
/// Attribute to describe business requirements.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class BusinessRequirementsAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BusinessRequirementsAttribute"/> class.
    /// </summary>
    /// <param name="capability">The main capability.</param>
    /// <param name="requirements">The requirements related to provide the capability.</param>
    public BusinessRequirementsAttribute(string capability, params string[] requirements)
    {
        this.Capability = capability;
        this.Requirements = requirements;
    }

    /// <summary>
    /// Gets the capability.
    /// </summary>
    public string Capability { get; }

    /// <summary>
    /// Gets the description of the requirement.
    /// </summary>
    public string[] Requirements { get; }
}
