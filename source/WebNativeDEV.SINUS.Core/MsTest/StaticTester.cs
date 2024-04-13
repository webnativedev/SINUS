using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WebNativeDEV.SINUS.Core.ArgumentValidation;
using WebNativeDEV.SINUS.Core.FluentAPI.Contracts.Runner;
using WebNativeDEV.SINUS.Core.MsTest.Context;
using WebNativeDEV.SINUS.Core.MsTest.Contracts;
using WebNativeDEV.SINUS.Core.MsTest.Model;
using WebNativeDEV.SINUS.MsTest;

namespace WebNativeDEV.SINUS.Core.MsTest
{
    public class StaticTester : TestBase
    {
        private StaticTester()
        {
            this.TestContext = new StaticTesterContext();
        }

        /// <summary>
        /// Creates a runner and uses the action to execute the test (including TestName overwrite).
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <returns>The current object for usage as Fluent API.</returns>
        public static ITestBaseResult Test(Action<IRunnerSystemAndBrowser> action)
            => new StaticTester().PublicTest(action);

        /// <summary>
        /// Creates a runner and uses the action to execute the test.
        /// </summary>
        /// <param name="scenario">The name of the scenario.</param>
        /// <param name="action">The action to execute.</param>
        /// <returns>The current object for usage as Fluent API.</returns>
        public static ITestBaseResult Test(string? scenario, Action<IRunnerSystemAndBrowser> action)
            => new StaticTester().PublicTest(scenario, action);

        /// <summary>
        /// Creates a runner and uses the action to execute the test.
        /// </summary>
        /// <param name="scenario">The name of the scenario.</param>
        /// <param name="strategy">The strategy that runs the test.</param>
        /// <param name="action">The action to execute.</param>
        /// <returns>The current object for usage as Fluent API.</returns>
        public static ITestBaseResult Test(string? scenario, ITestBaseStrategy strategy, Action<IRunnerSystemAndBrowser> action)
            => new StaticTester().PublicTest(scenario, strategy, action);

        /// <summary>
        /// Minimal execution of a maintenance task as action.
        /// It is a safe method that registers the maintenance task for the different statistics.
        /// </summary>
        /// <param name="action">The action to execute safely.</param>
        /// <returns>Test outcome and exception if any.</returns>
        public static ITestBaseResult Maintenance(Action action)
            => new StaticTester().PublicMaintenance(action);


        /// <summary>
        /// Creates a runner and uses the action to execute the test (including TestName overwrite).
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <returns>The current object for usage as Fluent API.</returns>
        public ITestBaseResult PublicTest(Action<IRunnerSystemAndBrowser> action)
            => base.Test(null, action);

        /// <summary>
        /// Creates a runner and uses the action to execute the test.
        /// </summary>
        /// <param name="scenario">The name of the scenario.</param>
        /// <param name="action">The action to execute.</param>
        /// <returns>The current object for usage as Fluent API.</returns>
        public ITestBaseResult PublicTest(string? scenario, Action<IRunnerSystemAndBrowser> action)
            => base.Test(scenario, action);

        /// <summary>
        /// Creates a runner and uses the action to execute the test.
        /// </summary>
        /// <param name="scenario">The name of the scenario.</param>
        /// <param name="strategy">The strategy that runs the test.</param>
        /// <param name="action">The action to execute.</param>
        /// <returns>The current object for usage as Fluent API.</returns>
        public ITestBaseResult PublicTest(string? scenario, ITestBaseStrategy strategy, Action<IRunnerSystemAndBrowser> action)
            => base.Test(scenario, strategy, action);

        /// <summary>
        /// Minimal execution of a maintenance task as action.
        /// It is a safe method that registers the maintenance task for the different statistics.
        /// </summary>
        /// <param name="action">The action to execute safely.</param>
        /// <returns>Test outcome and exception if any.</returns>
        public ITestBaseResult PublicMaintenance(Action action)
            => base.Maintenance(action);
    }
}
