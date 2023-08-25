// <copyright file="TechnicalRequirementAttribute.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Requirements;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// An attribute that represents a technical requirement that needs to be implemented.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class TechnicalRequirementAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TechnicalRequirementAttribute"/> class.
    /// </summary>
    /// <param name="description">A description that shows the purpose.</param>
    public TechnicalRequirementAttribute(string description)
    {
        this.Description = description;
    }

    /// <summary>
    /// Gets the description of the requirement.
    /// </summary>
    public string Description { get; }
}
