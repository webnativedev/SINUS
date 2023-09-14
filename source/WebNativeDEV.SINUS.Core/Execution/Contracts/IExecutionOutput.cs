// <copyright file="IExecutionOutput.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Execution.Contracts;

using WebNativeDEV.SINUS.Core.FluentAPI.Contracts;

public interface IExecutionOutput
{
    IList<Exception> Exceptions { get; }
    bool IsPreparedOnly { get; set; }
    RunCategory RunCategory { get; set; }
}