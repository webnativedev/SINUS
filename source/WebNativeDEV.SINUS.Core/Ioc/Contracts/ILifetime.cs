// <copyright file="ILifetime.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Ioc.Contracts;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// ILifetime management adds resolution strategies to an IScope.
/// </summary>
public interface ILifetime : IScope
{
    /// <summary>
    /// Gets the service as singleton.
    /// </summary>
    /// <param name="type">Defines which object should be generated or queried.</param>
    /// <param name="factory">Creation factory.</param>
    /// <returns>An instance to a service.</returns>
    object? GetServiceAsSingleton(Type type, Func<ILifetime, object?> factory);

    /// <summary>
    /// Get the service driven by the given scope.
    /// </summary>
    /// <param name="type">Defines which object should be generated or queried.</param>
    /// <param name="factory">Creation factory.</param>
    /// <returns>An instance to a service.</returns>
    object? GetServicePerScope(Type type, Func<ILifetime, object?> factory);
}
