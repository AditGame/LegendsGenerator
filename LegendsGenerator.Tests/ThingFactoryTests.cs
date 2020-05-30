// <copyright file="ThingFactoryTests.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Tests
{
    using System;
    using System.Collections.Generic;

    using LegendsGenerator.Contracts;
    using LegendsGenerator.Contracts.Definitions;
    using LegendsGenerator.Contracts.Definitions.Events;
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
            popCenter.DefaultAttributes["Evil"] = "rand.Next(-5, 5)";
            popCenter.DefaultAttributes["Population"] = "rand.Next(0, 1000000)";

            var cityDef = new SiteDefinition()
            {
                Name = "City",
                Description = "A moderate sized settlement",
                InheritsFrom = "Population Center",
            };
            cityDef.DefaultAttributes["Population"] = "rand.Next(20000, 100000)";

            IList<SiteDefinition> sites = new List<SiteDefinition>();
            sites.Add(popCenter);
            sites.Add(cityDef);

            var definitions = new DefinitionCollections(sites);

            DefinitionSerializer.SerializeToFile(definitions, new EventCollection(new List<EventDefinition>()), "test.json");

            int worldSeed = 915434125;
            Random rdm = new Random(worldSeed);

            ConditionCompiler processor = new ConditionCompiler(new Dictionary<string, object>());

            ThingFactory factory = new ThingFactory(processor, definitions);

            for (int i = 0; i < 100; i++)
            {
                Site city = factory.CreateSite(rdm, "City");
                Console.WriteLine($"City created: {city.EffectiveAttribute("Population")} {city.EffectiveAttribute("Evil")}");

                Assert.AreEqual("Site", city.ThingTypeName);
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
            cityDef.DefaultAttributes["Population"] = "rand.Next(20000, 100000)";

            IList<SiteDefinition> sites = new List<SiteDefinition>();
            sites.Add(cityDef);

            var definitions = new DefinitionCollections(sites);

            DefinitionSerializer.SerializeToFile(definitions, new EventCollection(new List<EventDefinition>()), "test.json");

            int worldSeed = 915434125;

            ConditionCompiler processor = new ConditionCompiler(new Dictionary<string, object>());

            ThingFactory factory = new ThingFactory(processor, definitions);

            factory.CreateSite(new Random(worldSeed), "City");
        }
    }
}
