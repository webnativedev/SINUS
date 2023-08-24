// <copyright file="BusinessRequirementsAttribute.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Requirements;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class BusinessRequirementsAttribute : Attribute
{
    public BusinessRequirementsAttribute(string capability, params string[] requirements)
    {
        this.Capability = capability;
        this.Requirements = requirements;
    }

    public string Capability { get; }
    public string[] Requirements { get; }
}
