// <copyright file="IRegisteredType.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Ioc.Contracts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// IRegisteredType is return by Container.Register and allows further configuration for the registration.
/// </summary>
public interface IRegisteredType
{
    /// <summary>
    /// Make registered type a singleton.
    /// </summary>
    void AsSingleton();

    /// <summary>
    /// Make registered type a per-scope type (single instance within a Scope).
    /// </summary>
    void PerScope();
}
