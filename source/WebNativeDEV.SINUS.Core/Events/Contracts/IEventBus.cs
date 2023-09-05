// <copyright file="IEventBus.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Events.Contracts
{
    /// <summary>
    /// Interface for the event bus.
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// Publishes an event.
        /// </summary>
        /// <typeparam name="TEventBusEventArgs">Type of event.</typeparam>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The eventargs used to transport data.</param>
        void Publish<TEventBusEventArgs>(object sender, TEventBusEventArgs e)
            where TEventBusEventArgs : EventBusEventArgs;

        /// <summary>
        /// Subscribe to an event.
        /// </summary>
        /// <typeparam name="TEventBusEventArgs">Type of event.</typeparam>
        /// <param name="handler">The handler that is called when a corresponding event is raised.</param>
        void Subscribe<TEventBusEventArgs>(Action<object, EventBusEventArgs> handler)
            where TEventBusEventArgs : EventBusEventArgs;
    }
}