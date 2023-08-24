// <copyright file="IContainer.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Ioc.Contracts;

/// <summary>
/// Interface representing an IoC container.
/// </summary>
public interface IContainer : IScope
{
    /// <summary>
    /// Registers a factory function which will be called to resolve the specified interface.
    /// </summary>
    /// <param name="interfaceType">Interface to register.</param>
    /// <param name="factory">Factory function.</param>
    /// <returns>The registered type instance.</returns>
    IRegisteredType Register(Type interfaceType, Func<object> factory);

    /// <summary>
    /// Registers a type which will be instantiated to resolve the specified interface.
    /// </summary>
    /// <param name="interfaceType">Interface to register.</param>
    /// <param name="implementation">Type to implementation.</param>
    /// <returns>The registered type instance.</returns>
    IRegisteredType Register(Type interfaceType, Type implementation);

    /// <summary>
    /// Registers a type which will be instantiated..
    /// </summary>
    /// <param name="type">Type of implementation that is not abstracted.</param>
    /// <returns>The registered type instance.</returns>
    IRegisteredType Register<T>(Type type);

    /// <summary>
    /// Registers a type which will be instantiated to resolve the specified interface.
    /// </summary>
    /// <typeparam name="TInterface">Interface to register.</typeparam>
    /// <typeparam name="TImplementation">Type to implementation.</typeparam>
    /// <returns>The registered type instance.</returns>
    IRegisteredType Register<TInterface, TImplementation>()
        where TImplementation : TInterface;

    /// <summary>
    /// Registers a factory method based on a single type.
    /// </summary>
    /// <typeparam name="T">Registered abstraction or concrete type.</typeparam>
    /// <param name="factory">The factory method.</param>
    /// <returns>The registered type instance.</returns>
    IRegisteredType Register<T>(Func<T> factory);

    /// <summary>
    /// Registers a type that is instanciated (only).
    /// </summary>
    /// <typeparam name="T">Type to register.</typeparam>
    /// <returns>The registered type instance.</returns>
    IRegisteredType Register<T>();

    /// <summary>
    /// Returns an implementation of the specified interface.
    /// </summary>
    /// <typeparam name="T">Interface type.</typeparam>
    /// <returns>Object implementing the interface.</returns>
    T Resolve<T>();
}