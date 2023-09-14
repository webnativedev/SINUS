// <copyright file="AssemblyTestContext.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.MsTest;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebNativeDEV.SINUS.Core.MsTest.Contracts;

/// <summary>
/// Store that holds the assembly context in an immutable way.
/// </summary>
internal class AssemblyTestContext : IAssemblyTestContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AssemblyTestContext"/> class.
    /// </summary>
    /// <param name="testContext">The object to store.</param>
    public AssemblyTestContext(TestContext testContext)
    {
        this.TestContext = testContext;
    }

    /// <summary>
    /// Gets the test context.
    /// </summary>
    public TestContext TestContext { get; }
}
