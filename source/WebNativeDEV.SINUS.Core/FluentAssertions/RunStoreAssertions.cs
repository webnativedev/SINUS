// <copyright file="RunStoreAssertions.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.FluentAssertions;

using FluentAssertions;
using global::FluentAssertions;
using global::FluentAssertions.Execution;
using global::FluentAssertions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebNativeDEV.SINUS.Core.FluentAPI;

/// <summary>
/// Fluent Assertions context for RunStore providing check methods.
/// </summary>
public class RunStoreAssertions :
    ReferenceTypeAssertions<RunStore, RunStoreAssertions>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RunStoreAssertions"/> class.
    /// </summary>
    /// <param name="instance">The instance to operate on.</param>
    public RunStoreAssertions(RunStore instance)
        : base(instance)
    {
    }

    /// <inheritdoc/>
    protected override string Identifier => "runStore";

    /// <summary>
    /// Checks the Actual value and makes an equal operation on the data.
    /// </summary>
    /// <typeparam name="T">Type of actual.</typeparam>
    /// <param name="expected">Value that should be met.</param>
    /// <param name="because">Message to print on assertion.</param>
    /// <param name="becauseArgs">Arguments of the assertion message.</param>
    /// <returns>Fluent API driven AndConstraint object.</returns>
    public AndConstraint<RunStoreAssertions> ActualBe<T>(T expected, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
         .BecauseOf(because, becauseArgs)
         .ForCondition(this.Subject?.ReadActual<T>()?.Equals(expected) ?? false)
         .FailWith("Expected '{0}', but Actual '{1}'", expected, this.Subject?.Actual ?? "null");

        return new AndConstraint<RunStoreAssertions>(this);
    }

    /// <summary>
    /// Checks the Actual value and makes an equal operation for expected null.
    /// </summary>
    /// <param name="because">Message to print on assertion.</param>
    /// <param name="becauseArgs">Arguments of the assertion message.</param>
    /// <returns>Fluent API driven AndConstraint object.</returns>
    public AndConstraint<RunStoreAssertions> ActualBeNull(string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
         .BecauseOf(because, becauseArgs)
         .ForCondition(this.Subject.Actual == null)
         .FailWith("Expected null, but Actual '{0}'", this.Subject.Actual ?? "null");

        return new AndConstraint<RunStoreAssertions>(this);
    }
}