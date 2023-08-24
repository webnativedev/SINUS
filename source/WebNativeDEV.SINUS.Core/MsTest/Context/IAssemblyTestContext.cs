// <copyright file="IAssemblyTestContext.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.MsTest.Context;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Interface that represents a store that holds the assembly context in an immutable way.
/// </summary>
public interface IAssemblyTestContext
{
    /// <summary>
    /// Gets the test context.
    /// </summary>
    TestContext TestContext { get; }
}
