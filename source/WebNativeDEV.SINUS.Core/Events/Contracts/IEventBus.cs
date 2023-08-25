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
        /// <param name="sender">The sender of the event.</param>
        /// <param name="eventName">The eventname.</param>
        /// <param name="e">The eventargs used to transport data.</param>
        void Publish(object sender, string eventName, EventBusEventArgs e);

        /// <summary>
        /// Subscribe to an event.
        /// </summary>
        /// <param name="eventName">Eventname to subscribe to.</param>
        /// <param name="handler">The handler that is called when a corresponding event is raised.</param>
        void Subscribe(string eventName, EventHandler<EventBusEventArgs> handler);
    }
}