// <copyright file="TestBaseResultAssertions.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Assertions;

using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebNativeDEV.SINUS.Core.FluentAPI;
using WebNativeDEV.SINUS.Core.MsTest;

/// <summary>
/// Fluent Assertions context for TestBaseResult providing check methods.
/// </summary>
public class TestBaseResultAssertions :
    ReferenceTypeAssertions<TestBaseResult, TestBaseResultAssertions>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestBaseResultAssertions"/> class.
    /// </summary>
    /// <param name="instance">The instance to operate on.</param>
    public TestBaseResultAssertions(TestBaseResult instance)
        : base(instance)
    {
    }

    /// <inheritdoc/>
    protected override string Identifier => "testBaseResult";

    /// <summary>
    /// Checks the test result value and makes a check for success.
    /// </summary>
    /// <param name="because">Message to print on assertion.</param>
    /// <param name="becauseArgs">Arguments of the assertion message.</param>
    /// <returns>Fluent API driven AndConstraint object.</returns>
    public AndConstraint<TestBaseResultAssertions> BeSuccessful(string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
         .BecauseOf(because, becauseArgs)
         .ForCondition(this.Subject.Success)
         .FailWith("Expected true, but Actual '{0}'", this.Subject.Success);

        return new AndConstraint<TestBaseResultAssertions>(this);
    }
}