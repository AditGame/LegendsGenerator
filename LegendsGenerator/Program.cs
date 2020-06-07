// <copyright file="Program.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using LegendsGenerator.Compiler.CSharp;
    using LegendsGenerator.Contracts;
    using LegendsGenerator.Contracts.Definitions;
    using LegendsGenerator.Contracts.Definitions.Events;

    /// <summary>
    /// Main entry point.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Main entry point to the application.
        /// </summary>
        public static void Main()
        {
            Log.Ts.Listeners.Add(new ConsoleTraceListener());
            /*
            var popCenter = new SiteDefinition()
            {
                Name = "Population Center",
                Description = "A center of population.",
            };
            popCenter.DefaultAttributes["Evil"] = "Rand.Next(-5, 5)";
            popCenter.DefaultAttributes["Population"] = "Rand.Next(0, 1000000)";

            var city = new SiteDefinition()
            {
                Name = "City",
                Description = "A moderate sized settlement",
                InheritsFrom = "Population Center",
            };
            city.DefaultAttributes["Population"] = "Rand.Next(20000, 100000)";

            IList<SiteDefinition> sites = new List<SiteDefinition>();
            sites.Add(popCenter);
            sites.Add(city);

            EventDefinition ev = new EventDefinition()
            {
                Description = "The city {Subject.Name} has suffered massive population loss.",
                Chance = "50",
                Subject = new SubjectDefinition()
                {
                    Type = ThingType.Site,
                    Condition = "Subject->Population > 50000",
                },
                Results = new EventResultDefinition[]
                {
                    new EventResultDefinition()
                    {
                        Default = true,
                        Effects = new EffectDefinition[]
                        {
                            new EffectDefinition()
                            {
                                Title = "A marginal ammount have died.",
                                AffectedAttribute = "Population",
                                Magnitude = "(int)(-1 * Subject->Population * (Rand.NextDouble() * .1))",
                            }
                        }
                    },
                    new EventResultDefinition()
                    {
                        Chance = "75",
                        Effects = new EffectDefinition[]
                        {
                            new EffectDefinition()
                            {
                                Title = "Most of the population has died.",
                                AffectedAttribute = "Population",
                                Magnitude = "(int)(-1 * Subject->Population * (.5 + (Rand.NextDouble() * .25)))",
                            }
                        }
                    }
                }
            };

            IList<EventDefinition> events = new List<EventDefinition>();
            events.Add(ev);

            var definitions = new DefinitionCollections(sites);
            var eventDefinitions = new EventCollection(events);

            DefinitionSerializer.SerializeToFile(definitions, eventDefinitions, "test.json");
            */
            var definitions = DefinitionSerializer.DeserializeFromDirectory("Definitions");

            int worldSeed = 915434125;
            Random rdm = new Random(worldSeed);

            ConditionCompiler processor = new ConditionCompiler(new Dictionary<string, object>());
            definitions.Attach(processor);

            ThingFactory factory = new ThingFactory(definitions);

            var cities = new List<Site>();
            for (int i = 0; i < 100; i++)
            {
                Site cityInst = factory.CreateSite(rdm, "City");
                cities.Add(cityInst);
                Console.WriteLine($"City created: {cityInst.EffectiveAttribute("Population")} {cityInst.EffectiveAttribute("Evil")}");
            }

            World world = new World()
            {
                WorldSeed = worldSeed,
                StepCount = 1,
                Events = new List<EventDefinition>(definitions.Events),
                Sites = cities,
            };

            for (int i = 0; i < 100; i++)
            {
                HistoryMachine.Step(world);
                world.StepCount++;
            }

            foreach (var site in world.Sites)
            {
                Console.WriteLine(site.ToString());
                Console.WriteLine($"Final Population: {site.EffectiveAttribute("Population")}");
            }
        }
    }
}
