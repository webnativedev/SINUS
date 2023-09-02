// <copyright file="ContainerLifetime.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Ioc;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebNativeDEV.SINUS.Core.ArgumentValidation;
using WebNativeDEV.SINUS.Core.Ioc.Contracts;

/// <summary>
/// Container lifetime management.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class ContainerLifetime : ObjectCache, ILifetime
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ContainerLifetime"/> class.
    /// </summary>
    /// <param name="getFactory">Function to get the factory to be called.</param>
    public ContainerLifetime(Func<Type, Func<ILifetime, object?>> getFactory)
        => this.GetFactory = Ensure.NotNull(getFactory);

    /// <summary>
    /// Gets the factory function from the given type, provided by owning container.
    /// </summary>
    public Func<Type, Func<ILifetime, object?>> GetFactory { get; private set; }

    /// <summary>
    /// Gets the service factory and calls it.
    /// </summary>
    /// <param name="serviceType">The service type specifying the interface.</param>
    /// <returns>A created instance by using the factory.</returns>
    public object? GetService(Type serviceType)
        => this.GetFactory(serviceType)(this);

    /// <inheritdoc/>
    public object? GetServiceAsSingleton(Type type, Func<ILifetime, object?> factory)
        => this.GetCached(type, factory, this);

    /// <inheritdoc/>
    public object? GetServicePerScope(Type type, Func<ILifetime, object?> factory)
        => this.GetServiceAsSingleton(type, factory);
}
