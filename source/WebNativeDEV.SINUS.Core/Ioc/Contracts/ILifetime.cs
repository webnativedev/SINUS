// <copyright file="ILifetime.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Ioc.Contracts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// ILifetime management adds resolution strategies to an IScope.
/// </summary>
public interface ILifetime : IScope
{
    object? GetServiceAsSingleton(Type type, Func<ILifetime, object?> factory);

    object? GetServicePerScope(Type type, Func<ILifetime, object?> factory);
}
