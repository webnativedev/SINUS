// <copyright file="BusinessRequirementAttribute.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Requirements;

using System;

/// <summary>
/// Attribute to describe business requirements.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="BusinessRequirementAttribute"/> class.
/// </remarks>
/// <param name="description">The requirement description.</param>
[AttributeUsage(AttributeTargets.Method)]
public sealed class BusinessRequirementAttribute(params string[] description) : Attribute
{
    /// <summary>
    /// Gets the description of the requirement.
    /// </summary>
    public string[] Description { get; } = description;
}
