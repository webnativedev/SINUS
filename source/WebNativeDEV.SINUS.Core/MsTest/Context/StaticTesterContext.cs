using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebNativeDEV.SINUS.Core.MsTest.Model;

namespace WebNativeDEV.SINUS.Core.MsTest.Context
{
    internal class StaticTesterContext : TestContext
    {
        private readonly IDictionary properties = new Dictionary<object, object>();
        public override IDictionary Properties => properties;

        public StaticTesterContext()
        {
            this.properties.Add(nameof(this.TestName), "Given_StaticTest_When_Executed_Then_UniqueExecutionStarts-" + Guid.NewGuid().ToString());
            this.properties.Add(nameof(this.FullyQualifiedTestClassName), "StaticTests");
            this.properties.Add(nameof(this.TestRunResultsDirectory), ".");
            this.properties.Add(nameof(this.TestRunDirectory), ".");
        }

        /// <inheritdoc />
        public override void AddResultFile(string fileName)
        {
        }

        public override void Write(string? message)
        {
        }

        public override void Write(string format, params object?[] args)
        {
        }

        public override void WriteLine(string? message)
        {
        }

        public override void WriteLine(string format, params object?[] args)
        {
        }
    }
}
