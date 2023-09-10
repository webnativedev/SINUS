// <copyright file="IEventBus.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Events.Contracts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Interface that covers subscribing and publishing of events.
/// </summary>
public interface IEventBus : IEventBusPublisher, IEventBusSubscriber
{
}
