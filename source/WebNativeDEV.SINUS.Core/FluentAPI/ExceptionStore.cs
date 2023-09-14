// <copyright file="ExceptionStore.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.FluentAPI;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebNativeDEV.SINUS.Core.FluentAPI.Contracts;
using WebNativeDEV.SINUS.Core.FluentAPI.Model;

/// <summary>
/// The exception store.
/// </summary>
internal class ExceptionStore : IExceptionStore
{
    private readonly List<ExceptionStoreItem> list = new();

    /// <inheritdoc/>
    public int Count => this.list.Count;

    /// <inheritdoc/>
    public bool HasUncheckedElements => this.list.Exists(e => !e.IsCheckedInThenClause);

    /// <inheritdoc/>
    public void Add(RunCategory runCategory, Exception exception)
    {
        this.list.Add(new ExceptionStoreItem(runCategory, exception));
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
        this.list.ForEach(item => item.IsCheckedInThenClause = true);
    }

    /// <inheritdoc/>
    public void SetAllCheckedOfType<T>()
        where T : Exception
    {
        this.list.ForEach(item =>
        {
            if(item.Exception is T)
            {
                item.IsCheckedInThenClause = true;
            }
        });
    }
}
