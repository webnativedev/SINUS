// <copyright file="BusinessRequirementAttribute.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Requirements;

using System;

/// <summary>
/// Attribute to describe business requirements.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class BusinessRequirementAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BusinessRequirementAttribute"/> class.
    /// </summary>
    /// <param name="description">The requirement description.</param>
    public BusinessRequirementAttribute(params string[] description)
    {
        this.Description = description;
    }

    /// <summary>
    /// Gets the description of the requirement.
    /// </summary>
    public string[] Description { get; }
}
