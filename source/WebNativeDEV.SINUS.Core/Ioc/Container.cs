﻿// <copyright file="Container.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Ioc;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WebNativeDEV.SINUS.Core.ArgumentValidation;
using WebNativeDEV.SINUS.Core.Ioc.Contracts;

/// <summary>
/// Inversion of control container handles dependency injection for registered types.
/// </summary>
/// <remarks>
/// based on Microsoft's MinIoc https://github.com/microsoft/MinIoC/blob/main/Container.cs.
/// </remarks>
public sealed class Container : IContainer, IDisposable
{
    /// <summary>
    /// Map of registered types.
    /// </summary>
    private readonly Dictionary<Type, Func<ILifetime, object?>> registeredTypes = new();

    /// <summary>
    /// Lifetime management.
    /// </summary>
    private readonly ContainerLifetime lifetime;
    private bool disposedValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="Container"/> class.
    /// </summary>
    public Container()
    {
        this.lifetime = new ContainerLifetime(t => this.registeredTypes[t]);
    }

    [ExcludeFromCodeCoverage]
    ~Container()
    {
        this.Dispose(disposing: false);
    }

    public IRegisteredType Register(Type interfaceType, Func<object> factory)
        => this.RegisterType(interfaceType, _ => factory());

    /// <summary>
    /// Registers an implementation type for the specified interface.
    /// </summary>
    /// <param name="interfaceType">Interface to register.</param>
    /// <param name="implementation">Implementing type.</param>
    /// <returns>
    /// An object pointing to a <see cref="IRegisteredType"/>
    /// that allows further configuration.
    /// </returns>
    public IRegisteredType Register(Type interfaceType, Type implementation)
        => this.RegisterType(
            Ensure.NotNull(interfaceType, nameof(interfaceType)),
            FactoryFromType(Ensure.NotNull(implementation, nameof(implementation))));

    public IRegisteredType RegisterType(Type itemType, Func<ILifetime, object?> factory)
        => new RegisteredType(itemType, f => this.registeredTypes[itemType] = f, factory);

    /// <summary>
    /// Registers an implementation type for the specified interface.
    /// </summary>
    /// <typeparam name="T">Interface to register.</typeparam>
    /// <param name="type">Implementing type.</param>
    /// <returns>IRegisteredType object.</returns>
    public IRegisteredType Register<T>(Type type)
        => this.Register(typeof(T), type);

    /// <summary>
    /// Registers an implementation type for the specified interface.
    /// </summary>
    /// <typeparam name="TInterface">Interface to register.</typeparam>
    /// <typeparam name="TImplementation">Implementing type.</typeparam>
    /// <returns>IRegisteredType object.</returns>
    public IRegisteredType Register<TInterface, TImplementation>()
        where TImplementation : TInterface
        => this.Register(typeof(TInterface), typeof(TImplementation));

    /// <summary>
    /// Registers a factory function which will be called to resolve the specified interface.
    /// </summary>
    /// <typeparam name="T">Interface to register.</typeparam>
    /// <param name="factory">Factory method.</param>
    /// <returns>IRegisteredType object.</returns>
    public IRegisteredType Register<T>(Func<T> factory)
        => this.Register(
            typeof(T),
            () => Ensure.NotNull(factory).Invoke() ?? throw new InvalidDataException());

    /// <summary>
    /// Registers a type.
    /// </summary>
    /// <typeparam name="T">Type to register.</typeparam>
    /// <returns>IRegisteredType object.</returns>
    public IRegisteredType Register<T>()
        => this.Register(typeof(T), typeof(T));

    /// <summary>
    /// Returns an implementation of the specified interface.
    /// </summary>
    /// <typeparam name="T">Interface type.</typeparam>
    /// <returns>Object implementing the interface.</returns>
    public T Resolve<T>()
        => ((T?)this.GetService(typeof(T)))
            ?? throw new InvalidDataException("Service can not be resolved");

    /// <summary>
    /// Returns the object registered for the given type, if registered.
    /// </summary>
    /// <param name="serviceType">Type as registered with the container.</param>
    /// <returns>Instance of the registered type, if registered; otherwise <see langword="null"/>.</returns>
    public object? GetService(Type serviceType)
    {
        if (this.registeredTypes.TryGetValue(serviceType, out Func<ILifetime, object?>? registeredType))
        {
            return registeredType?.Invoke(this.lifetime);
        }

        return null;
    }

    /// <summary>
    /// Creates a new scope.
    /// </summary>
    /// <returns>Scope object.</returns>
    public IScope CreateScope() => new ScopeLifetime(this.lifetime);

    /// <summary>
    /// Disposes any <see cref="IDisposable"/> objects owned by this container.
    /// </summary>
    public void Dispose()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    private static Func<ILifetime, object?> FactoryFromType(Type itemType)
    {
        // Get first constructor for the type
        var constructors = itemType.GetConstructors();
        if (constructors.Length == 0)
        {
            // If no public constructor found, search for an internal constructor
            constructors = itemType.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
        }

        var constructor = constructors.First();

        // Compile constructor call as a lambda expression
        var arg = Expression.Parameter(typeof(ILifetime));
        return (Func<ILifetime, object?>)Expression.Lambda(
            Expression.New(constructor, constructor.GetParameters().Select(
                param =>
                {
                    var resolve = new Func<ILifetime, object?>(
                        lifetime => lifetime.GetService(param.ParameterType));
                    return Expression.Convert(
                        Expression.Call(Expression.Constant(resolve.Target), resolve.Method, arg),
                        param.ParameterType);
                })),
            arg).Compile();
    }

    private void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing)
            {
                this.lifetime.Dispose();
            }

            this.disposedValue = true;
        }
    }
}
