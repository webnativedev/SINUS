// <copyright file="TestBaseResultAssertions.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Assertions;

using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using System.Diagnostics.CodeAnalysis;
using WebNativeDEV.SINUS.Core.FluentAPI.Model;
using WebNativeDEV.SINUS.Core.MsTest.Contracts;

/// <summary>
/// Fluent Assertions context for TestBaseResult providing check methods.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="TestBaseResultAssertions"/> class.
/// </remarks>
/// <param name="instance">The instance to operate on.</param>
public class TestBaseResultAssertions(ITestBaseResult instance) :
    ReferenceTypeAssertions<ITestBaseResult, TestBaseResultAssertions>(instance)
{
    /// <inheritdoc/>
    [ExcludeFromCodeCoverage]
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
         .ForCondition(this.Subject.Outcome == TestOutcome.Success)
         .FailWith("Expected success, but Actual '{0}'", this.Subject.Outcome);

        return new AndConstraint<TestBaseResultAssertions>(this);
    }
}