// <copyright file="EventBus.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Events;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class EventBus
{
    private static readonly Dictionary<string, List<EventHandler<EventBusEventArgs>>> Handlers = new();

    public static void Publish(object sender, string eventName, EventBusEventArgs e)
    {
        foreach (EventHandler<EventBusEventArgs> handler in Handlers[eventName])
        {
            handler(sender, e);
        }
    }

    public static void Subscribe(string eventName, EventHandler<EventBusEventArgs> handler)
    {
        if (!Handlers.ContainsKey(eventName))
        {
            Handlers.Add(eventName, new List<EventHandler<EventBusEventArgs>>());
        }

        Handlers[eventName].Add(handler);
    }
}
