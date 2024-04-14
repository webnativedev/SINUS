// <copyright file="IEventBusPublisher.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Events.Contracts;

using WebNativeDEV.SINUS.Core.Events.EventArguments;

/// <summary>
/// Interface for the event bus.
/// </summary>
public interface IEventBusPublisher
{
    /// <summary>
    /// Publishes an event.
    /// </summary>
    /// <typeparam name="TEventBusEventArgs">Type of event.</typeparam>
    /// <param name="sender">The sender of the event.</param>
    /// <param name="e">The eventargs used to transport data.</param>
    void Publish<TEventBusEventArgs>(object sender, TEventBusEventArgs e)
        where TEventBusEventArgs : EventBusEventArgs;
}
