// <copyright file="ExceptionStore.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.FluentAPI;

using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using WebNativeDEV.SINUS.Core.FluentAPI.Contracts;
using WebNativeDEV.SINUS.Core.FluentAPI.Events;
using WebNativeDEV.SINUS.Core.FluentAPI.Model;
using WebNativeDEV.SINUS.Core.MsTest;

/// <summary>
/// The exception store.
/// </summary>
internal class ExceptionStore : IExceptionStore
{
    private readonly List<ExceptionStoreItem> list = new();
    private readonly TestBaseScopeContainer scope;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionStore"/> class.
    /// </summary>
    /// <param name="scope">Dependency container.</param>
    public ExceptionStore(TestBaseScopeContainer scope)
    {
        this.scope = scope;
    }

    /// <inheritdoc/>
    public int Count => this.list.Count;

    /// <inheritdoc/>
    public bool HasUncheckedElements => this.list.Exists(e => !e.IsCheckedInThenClause);

    /// <inheritdoc/>
    public void Add(RunCategory runCategory, Exception exception)
    {
        var item = new ExceptionStoreItem(runCategory, exception);

        if (exception is AggregateException aggException)
        {
            this.scope.EventBus.Publish(this, new ExceptionChangedEventBusEventArgs(item));

            foreach (var e in aggException.InnerExceptions)
            {
                this.Add(runCategory, e);
            }

            return;
        }

        this.list.Add(item);
        this.scope.EventBus.Publish(this, new ExceptionChangedEventBusEventArgs(item));
    }

    /// <inheritdoc/>
    public bool Any() => this.list.Any();

    /// <inheritdoc/>
    public int CountOfType<T>()
        where T : Exception
    {
        return this.list.Count(x => x.Exception is T);
    }

    /// <inheritdoc/>
    public string? GetContentAsString()
    {
        return string.Join(", ", this.list.Select(x => x.GetTupleString()));
    }

    /// <inheritdoc/>
    public void SetAllChecked()
    {
        this.list.ForEach(item =>
        {
            item.IsCheckedInThenClause = true;
            this.scope.EventBus.Publish(this, new ExceptionChangedEventBusEventArgs(item));
        });
    }

    /// <inheritdoc/>
    public void SetAllCheckedOfType<T>()
        where T : Exception
    {
        this.list.ForEach(item =>
        {
            if (item.Exception is T)
            {
                item.IsCheckedInThenClause = true;
                this.scope.EventBus.Publish(this, new ExceptionChangedEventBusEventArgs(item));
            }
        });
    }
}
