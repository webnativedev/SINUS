// <copyright file="RequirementAttribute.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Requirements;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[AttributeUsage(AttributeTargets.Method)]
public sealed class BusinessRequirementAttribute : Attribute
{
    public BusinessRequirementAttribute(params string[] description)
    {
        this.Description = description;
    }

    public string[] Description { get; }
}
