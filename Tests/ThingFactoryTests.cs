// <copyright file="ThingFactoryTests.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Tests
{
    using System;
    using System.Collections.Generic;

    using LegendsGenerator.Compiler.CSharp;
    using LegendsGenerator.Contracts.Definitions;
    using LegendsGenerator.Contracts.Definitions.Events;
    using LegendsGenerator.Contracts.Things;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the Thing factory.
    /// </summary>
    [TestClass]
    public class ThingFactoryTests
    {
        /// <summary>
        /// Tests successfully creating a site.
        /// </summary>
        [TestMethod]
        public void Happypath()
        {
            var popCenter = new SiteDefinition()
            {
                Name = "Population Center",
                Description = "A center of population.",
            };
            popCenter.Attributes["Evil"] = new AttributeDefinition() { BaseValue = "Rand.Next(-5, 5)" };
            popCenter.Attributes["Population"] = new AttributeDefinition() { BaseValue = "Rand.Next(0, 1000000)" };

            var cityDef = new SiteDefinition()
            {
                Name = "City",
                Description = "A moderate sized settlement",
                InheritsFrom = "Population Center",
            };
            cityDef.Attributes["Population"] = new AttributeDefinition() { BaseValue = "Rand.Next(20000, 100000)" };

            IList<SiteDefinition> sites = new List<SiteDefinition>
            {
                popCenter,
                cityDef,
            };

            var definitions = new DefinitionCollection(sites);

            int worldSeed = 915434125;
            Random rdm = new Random(worldSeed);

            ConditionCompiler<BaseGlobalVariables> processor = new ConditionCompiler<BaseGlobalVariables>(new BaseGlobalVariables()
            {
                World = new Contracts.World(),
            });

            definitions.Attach(processor);
            ThingFactory factory = new ThingFactory(definitions);

            for (int i = 0; i < 100; i++)
            {
                Site city = factory.CreateSite(rdm, 0, 0, "City");
                city.FinalizeConstruction(rdm);
                Console.WriteLine($"City created: {city.EffectiveAttribute("Population")} {city.EffectiveAttribute("Evil")}");

                Assert.AreEqual(ThingType.Site, city.ThingType);
                Assert.AreEqual(cityDef, city.Definition);
                int population = city.EffectiveAttribute("Population");
                Assert.IsTrue(population <= 100000);
                Assert.IsTrue(population >= 20000);
                int evil = city.EffectiveAttribute("Evil");
                Assert.IsTrue(evil <= 5);
                Assert.IsTrue(evil >= -5);
            }
        }

        /// <summary>
        /// Tests attempting to construct a site with broken inheritance.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void InheritanceMissing()
        {
            var cityDef = new SiteDefinition()
            {
                Name = "City",
                Description = "A moderate sized settlement",
                InheritsFrom = "Population Center",
            };
            cityDef.Attributes["Population"] = new AttributeDefinition() { BaseValue = "rand.Next(20000, 100000)" };

            IList<SiteDefinition> sites = new List<SiteDefinition>
            {
                cityDef,
            };

            var definitions = new DefinitionCollection(sites);

            int worldSeed = 915434125;

            ThingFactory factory = new ThingFactory(definitions);

            //factory.CreateSite(new Random(worldSeed), 0, 0, "City");
        }
    }
}
