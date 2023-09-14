// <copyright file="EventBusEventArgs.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Events.EventArguments;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// EventArg class used in Eventbus.
/// </summary>
public class EventBusEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EventBusEventArgs"/> class.
    /// </summary>
    public EventBusEventArgs()
    {
    }

    /// <summary>
    /// Gets an empty object to reduce allocations.
    /// </summary>
    public static new EventBusEventArgs Empty { get; } = new();
}
