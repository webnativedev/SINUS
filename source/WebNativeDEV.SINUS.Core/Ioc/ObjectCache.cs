// <copyright file="ObjectCache.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Ioc;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebNativeDEV.SINUS.Core.Ioc.Contracts;

/// <summary>
/// ObjectCache provides common caching logic for lifetimes.
/// </summary>
public abstract class ObjectCache : IDisposable
{
    /// <summary>
    /// Instance cache.
    /// </summary>
    private readonly ConcurrentDictionary<Type, object?> instanceCache = new();

    /// <summary>
    /// Represents the disposal state.
    /// </summary>
    private bool disposedValue;

    [ExcludeFromCodeCoverage]
    ~ObjectCache()
    {
        this.Dispose(disposing: false);
    }

    /// <summary>
    /// Disposes the object.
    /// </summary>
    public void Dispose()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Get from cache or create and cache object.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="factory"></param>
    /// <param name="lifetime"></param>
    /// <returns></returns>
    protected object? GetCached(Type type, Func<ILifetime, object?> factory, ILifetime lifetime)
        => this.instanceCache.GetOrAdd(type, _ => factory(lifetime));

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing)
            {
                this.instanceCache.Values.OfType<IDisposable>().ToList().ForEach(instance => instance.Dispose());
            }

            this.disposedValue = true;
        }
    }
}
