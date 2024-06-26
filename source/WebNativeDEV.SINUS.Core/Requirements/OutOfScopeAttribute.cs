﻿// <copyright file="OutOfScopeAttribute.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Requirements;

using System;

/// <summary>
/// Used for documentation and defines that no Requirement is backing the implementation.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="OutOfScopeAttribute"/> class.
/// </remarks>
/// <param name="description">A description that shows the purpose.</param>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class OutOfScopeAttribute(string description) : Attribute
{
    /// <summary>
    /// Gets the description why this implementation is out of scope.
    /// </summary>
    public string Description { get; } = description;
}
