// <copyright file="RunStoreAssertions.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Assertions;

using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using System.Diagnostics.CodeAnalysis;
using WebNativeDEV.SINUS.Core.FluentAPI.Contracts;

/// <summary>
/// Fluent Assertions context for RunStore providing check methods.
/// </summary>
public class RunStoreAssertions :
    ReferenceTypeAssertions<IRunStore, RunStoreAssertions>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RunStoreAssertions"/> class.
    /// </summary>
    /// <param name="instance">The instance to operate on.</param>
    public RunStoreAssertions(IRunStore instance)
        : base(instance)
    {
    }

    /// <inheritdoc/>
    [ExcludeFromCodeCoverage]
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
        var actual = this.Subject.ReadActualObject();
        if (actual is not T)
        {
            actual = null;
        }

        Execute.Assertion
         .BecauseOf(because, becauseArgs)
         .ForCondition(
            (actual == null && expected == null) ||
            (expected?.Equals(actual) ?? false))
         .FailWith(
            "Expected '{0}', but Actual '{1}'",
            expected?.ToString() ?? "null",
            actual?.ToString() ?? "null");

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

    /// <summary>
    /// Checks the Actual value and makes an equal operation for expected null.
    /// </summary>
    /// <param name="because">Message to print on assertion.</param>
    /// <param name="becauseArgs">Arguments of the assertion message.</param>
    /// <returns>Fluent API driven AndConstraint object.</returns>
    public AndConstraint<RunStoreAssertions> ActualBeNotNull(string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
         .BecauseOf(because, becauseArgs)
         .ForCondition(this.Subject.Actual != null)
         .FailWith("Expected non-null, but Actual '{0}'", this.Subject.Actual ?? "null");

        return new AndConstraint<RunStoreAssertions>(this);
    }
}