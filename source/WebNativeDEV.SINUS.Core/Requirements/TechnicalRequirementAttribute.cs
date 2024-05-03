// <copyright file="TechnicalRequirementAttribute.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Requirements;

using System;

/// <summary>
/// An attribute that represents a technical requirement that needs to be implemented.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="TechnicalRequirementAttribute"/> class.
/// </remarks>
/// <param name="description">A description that shows the purpose.</param>
[AttributeUsage(AttributeTargets.Method)]
public sealed class TechnicalRequirementAttribute(params string[] description) : Attribute
{
    /// <summary>
    /// Gets the description of the requirement.
    /// </summary>
    public string[] Description { get; } = description;
}
