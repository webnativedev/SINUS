// <copyright file="BusinessRequirementsAttribute.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Requirements;

using System;

/// <summary>
/// Attribute to describe business requirements.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="BusinessRequirementsAttribute"/> class.
/// </remarks>
/// <param name="capability">The main capability.</param>
/// <param name="requirements">The requirements related to provide the capability.</param>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class BusinessRequirementsAttribute(string capability, params string[] requirements) : Attribute
{
    /// <summary>
    /// Gets the capability.
    /// </summary>
    public string Capability { get; } = capability;

    /// <summary>
    /// Gets the description of the requirement.
    /// </summary>
    public string[] Requirements { get; } = requirements;
}
