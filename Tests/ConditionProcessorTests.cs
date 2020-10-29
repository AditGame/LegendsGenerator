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
            site.BaseAspects["Gender"] = "Male";
            site.BaseAspects["IsInsane"] = "true";
            site.BaseAspects["IsMoral"] = "false";
            site.FinalizeConstruction(rdm);

            IDictionary<string, BaseThing> paramList = new Dictionary<string, BaseThing>()
            {
                { "Subject", site },
            };

            var variables = paramList.Select(x => new CompiledVariable(x.Key, x.Value.GetType())).ToList();

            // int conditions
            new[]
            {
                new { condition = "(Subject->Health + Subject->Fear) / 2", output = 14 },
                new { condition = "Subject->Health * Subject->Strength", output = 5 },
            }.ToList().ForEach(a =>
            {
                var condition = processor.AsSimple<int>(a.condition, variables);
                Assert.AreEqual(a.output, condition.Evaluate(rdm, paramList), a.condition);
            });

            // bool conditions
            new[]
            {
                new { condition = "Subject->Health <= 0", output = false },
                new { condition = "Subject-<bool>IsInsane", output = true },
                new { condition = "Subject-<bool>IsMoral", output = false },
            }.ToList().ForEach(a =>
            {
                var condition = processor.AsSimple<bool>(a.condition, variables);
                Assert.AreEqual(a.output, condition.Evaluate(rdm, paramList), a.condition);
            });

            // string conditions
            new[]
            {
                new { condition = "Subject-<>Gender", output = "Male" },
            }.ToList().ForEach(a =>
            {
                var condition = processor.AsSimple<string>(a.condition, variables);
                Assert.AreEqual(a.output, condition.Evaluate(rdm, paramList), a.condition);
            });

            var conditions = new List<ICompiledCondition<bool>>();
            Stopwatch watch = new Stopwatch();
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
