// <copyright file="ScopeLifetime.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Ioc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebNativeDEV.SINUS.Core.Ioc.Contracts;

/// <summary>
/// Per-scope lifetime management.
/// </summary>
public sealed class ScopeLifetime : ObjectCache, ILifetime
{
    /// <summary>
    /// Singletons come from parent container's lifetime.
    /// </summary>
    private readonly ContainerLifetime parentLifetime;

    /// <summary>
    /// Initializes a new instance of the <see cref="ScopeLifetime"/> class.
    /// </summary>
    /// <param name="parentContainer"></param>
    public ScopeLifetime(ContainerLifetime parentContainer)
        => this.parentLifetime = parentContainer;

    /// <summary>
    /// Gets the service.
    /// </summary>
    /// <param name="serviceType">Type identifier.</param>
    /// <returns>Created or cached object.</returns>
    public object? GetService(Type serviceType)
        => this.parentLifetime.GetFactory(serviceType)(this);

    ///<inheritdoc/>
    public object? GetServiceAsSingleton(Type type, Func<ILifetime, object?> factory)
        => this.parentLifetime.GetServiceAsSingleton(type, factory);

    /// <inheritdoc/>
    public object? GetServicePerScope(Type type, Func<ILifetime, object?> factory)
        => this.GetCached(type, factory, this);
}
