// <copyright file="IEventBus.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Events.Contracts;

/// <summary>
/// Interface that covers subscribing and publishing of events.
/// </summary>
public interface IEventBus : IEventBusPublisher, IEventBusSubscriber
{
}
