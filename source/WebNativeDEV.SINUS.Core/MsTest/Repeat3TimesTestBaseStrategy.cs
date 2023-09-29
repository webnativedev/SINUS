// <copyright file="Repeat3TimesTestBaseStrategy.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.MsTest;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebNativeDEV.SINUS.Core.FluentAPI.Contracts.Runner;
using WebNativeDEV.SINUS.Core.MsTest.Abstractions;
using WebNativeDEV.SINUS.Core.MsTest.Contracts;
using WebNativeDEV.SINUS.MsTest;

/// <summary>
/// Implements a concrete version of abstract test base strategy that repeats failed tests.
/// </summary>
internal class Repeat3TimesTestBaseStrategy : AbstractTestBaseStrategy
{
    private const int RepetitionCounter = 3;

    /// <inheritdoc/>
    public override ITestBaseResult Test(TestBase testBase, string? scenario, Action<IRunnerSystemAndBrowser> action)
    {
        ITestBaseResult? result = null;

        for (int runCounter = 0; runCounter < RepetitionCounter; runCounter++)
        {
            try
            {
                result = this.TestImplementation(testBase, scenario, action);
                break;
            }
            catch (Exception exc)
            {
                if (runCounter + 1 >= RepetitionCounter)
                {
                    throw new InvalidOperationException("repeat strategy still throw an error after repition", exc);
                }
            }
        }

        return result ?? throw new InvalidDataException("result was not set by test execution");
    }
}
