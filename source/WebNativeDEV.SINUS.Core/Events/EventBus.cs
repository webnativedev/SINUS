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

/// <summary>
/// Implementation of a simple event bus.
/// </summary>
public class EventBus : IEventBus
{
    private readonly Dictionary<string, List<EventHandler<EventBusEventArgs>>> handlers = new();

    /// <inheritdoc/>
    public void Publish(object sender, string eventName, EventBusEventArgs e)
    {
        foreach (EventHandler<EventBusEventArgs> handler in this.handlers[eventName])
        {
            handler(sender, e);
        }
    }

    /// <inheritdoc/>
    public void Subscribe(string eventName, EventHandler<EventBusEventArgs> handler)
    {
        if (!this.handlers.ContainsKey(eventName))
        {
            this.handlers.Add(eventName, new List<EventHandler<EventBusEventArgs>>());
        }

        this.handlers[eventName].Add(handler);
    }
}
