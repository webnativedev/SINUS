// <copyright file="ExecutionSetupParameters.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Execution;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Parameter container for the setup phase used in execution engine.
/// </summary>
internal sealed class ExecutionSetupParameters
{
    /// <summary>
    /// Gets or sets the endpoint.
    /// </summary>
    public string? Endpoint { get; set; }
}
