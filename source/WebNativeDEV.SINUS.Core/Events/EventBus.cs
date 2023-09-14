// <copyright file="EventBus.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Events;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebNativeDEV.SINUS.Core.Events.Contracts;
using WebNativeDEV.SINUS.Core.Events.EventArguments;

/// <summary>
/// Implementation of a simple event bus.
/// </summary>
internal class EventBus : IEventBus
{
    private readonly Dictionary<Type, List<Action<object, EventBusEventArgs>>> handlers = new();

    /// <inheritdoc/>
    public void Publish<TEventBusEventArgs>(object sender, TEventBusEventArgs e)
            where TEventBusEventArgs : EventBusEventArgs
    {
        if(!this.handlers.ContainsKey(typeof(TEventBusEventArgs)))
        {
            return;
        }

        foreach (Action<object, EventBusEventArgs> handler in this.handlers[typeof(TEventBusEventArgs)])
        {
            handler(sender, e);
        }
    }

    /// <inheritdoc/>
    public void Subscribe<TEventBusEventArgs>(Action<object, EventBusEventArgs> handler)
            where TEventBusEventArgs : EventBusEventArgs
    {
        if (!this.handlers.ContainsKey(typeof(TEventBusEventArgs)))
        {
            this.handlers.Add(typeof(TEventBusEventArgs), new List<Action<object, EventBusEventArgs>>());
        }

        this.handlers[typeof(TEventBusEventArgs)].Add(handler);
    }
}
