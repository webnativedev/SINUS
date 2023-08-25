// <copyright file="RegisteredType.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Ioc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebNativeDEV.SINUS.Core.ArgumentValidation;
using WebNativeDEV.SINUS.Core.Ioc.Contracts;

/// <summary>
/// RegisteredType is supposed to be a short lived object tying an item to its container
/// and allowing users to mark it as a singleton or per-scope item.
/// </summary>
public sealed class RegisteredType : IRegisteredType
{
    private readonly Type itemType;
    private readonly Action<Func<ILifetime, object?>> registerFactory;
    private readonly Func<ILifetime, object?> factory;

    /// <summary>
    /// Initializes a new instance of the <see cref="RegisteredType"/> class.
    /// </summary>
    /// <param name="itemType"></param>
    /// <param name="registerFactory"></param>
    /// <param name="factory"></param>
    public RegisteredType(Type itemType, Action<Func<ILifetime, object?>> registerFactory, Func<ILifetime, object?> factory)
    {
        this.itemType = itemType;
        this.registerFactory = registerFactory;
        this.factory = Ensure.NotNull(factory);

        this.registerFactory(this.factory);
    }

    /// <inheritdoc/>
    public void AsSingleton()
        => this.registerFactory(lifetime => lifetime.GetServiceAsSingleton(this.itemType, this.factory));

    /// <inheritdoc/>
    public void PerScope()
        => this.registerFactory(lifetime => lifetime.GetServicePerScope(this.itemType, this.factory));
}
