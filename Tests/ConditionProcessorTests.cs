// <copyright file="ConditionProcessorTests.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using LegendsGenerator.Compiler.CSharp;
    using LegendsGenerator.Contracts.Compiler;
    using LegendsGenerator.Contracts.Definitions;
    using LegendsGenerator.Contracts.Things;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for the conditional processor.
    /// </summary>
    [TestClass]
    public class ConditionProcessorTests
    {
        /// <summary>
        /// Tests simple conditions.
        /// </summary>
        [TestMethod]
        public void SimpleCondition()
        {
            var globals = new BaseGlobalVariables()
            {
                World = new Contracts.World(),
            };

            Random rdm = new Random(5);

            ConditionCompiler<BaseGlobalVariables> processor = new ConditionCompiler<BaseGlobalVariables>(globals);

            Site site = new Site(new SiteDefinition());
            site.BaseAttributes["Health"] = 5;
            site.BaseAttributes["Fear"] = 23;
            site.BaseAttributes["Strength"] = 1;
            site.FinalizeConstruction(rdm);

            IDictionary<string, BaseThing> paramList = new Dictionary<string, BaseThing>()
            {
                { "Subject", site },
            };


            var variables = paramList.Select(x => new CompiledVariable(x.Key, x.Value.GetType())).ToList();

            var condition = processor.AsSimple<int>("(Subject->Health + Subject->Fear) / 2", variables);

            Assert.AreEqual(14, condition.Evaluate(rdm, paramList));

            var condition2 = processor.AsSimple<int>("Subject->Health * Subject->Strength", variables);

            Assert.AreEqual(5, condition2.Evaluate(rdm, paramList));
            Stopwatch watch = Stopwatch.StartNew();
            var boolCondition = processor.AsSimple<bool>("Subject->Health <= 0", variables);
            watch.Stop();
            Console.WriteLine($"Compiling took {watch.Elapsed} uncached.");

            Assert.IsFalse(boolCondition.Evaluate(rdm, paramList));

            var conditions = new List<ICompiledCondition<bool>>();
            for (int i = 0; i < 500; i++)
            {
                watch = Stopwatch.StartNew();
                conditions.Add(processor.AsSimple<bool>("Subject->Health <= 0", variables));
                watch.Stop();
                Console.WriteLine($"Compiling took {watch.Elapsed} cached.");
            }
        }
    }
}
