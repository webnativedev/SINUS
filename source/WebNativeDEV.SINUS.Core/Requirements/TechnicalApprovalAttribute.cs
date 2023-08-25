// <copyright file="TechnicalApprovalAttribute.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Requirements;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Used for documentation and defines that no Requirement is backing the implementation.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class TechnicalApprovalAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TechnicalApprovalAttribute"/> class.
    /// </summary>
    /// <param name="description">A description that shows the purpose.</param>
    public TechnicalApprovalAttribute(string description)
    {
        this.Description = description;
    }

    /// <summary>
    /// Gets the description why this implementation is out of scope.
    /// </summary>
    public string Description { get; }
}
