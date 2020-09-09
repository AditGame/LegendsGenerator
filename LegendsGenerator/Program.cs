// <copyright file="Program.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
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
                Results = new List<EventResultDefinition>()
                {
                    new EventResultDefinition()
                    {
                        Default = true,
                        Effects = new List<EffectDefinition>()
                        {
                            new EffectDefinition()
                            {
                                Title = "A marginal ammount have died.",
                                AffectedAttribute = "Population",
                                Magnitude = "(int)(-1 * Subject->Population * (Rand.NextDouble() * .1))",
                            },
                        },
                    },
                    new EventResultDefinition()
                    {
                        Chance = "75",
                        Effects = new List<EffectDefinition>()
                        {
                            new EffectDefinition()
                            {
                                Title = "Most of the population has died.",
                                AffectedAttribute = "Population",
                                Magnitude = "(int)(-1 * Subject->Population * (.5 + (Rand.NextDouble() * .25)))",
                            },
                        },
                    },
                },
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
            HistoryMachine history = new HistoryMachine(factory);

            World world = new World()
            {
                WorldSeed = worldSeed,
                StepCount = 1,
                Events = new List<EventDefinition>(definitions.Events),
                Grid = new Grid(20, 20),
            };

            for (int i = 0; i < 100; i++)
            {
                int x = rdm.Next(0, 19);
                int y = rdm.Next(0, 19);
                Site cityInst = factory.CreateSite(rdm, x, y, "City");
                world.Grid.AddThing(cityInst);
                Console.WriteLine($"City created: {cityInst.EffectiveAttribute("Population")} {cityInst.EffectiveAttribute("Evil")}");
            }

            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine(world.StepCount.ToString());
                world = history.Step(world);
            }

            Console.WriteLine(world.Grid.ToString());

            foreach (var site in world.Grid.GetAllGridEntries().SelectMany(x => x.Square.ThingsInGrid))
            {
                Console.WriteLine(site.ToString());
                Console.WriteLine($"Final Population: {site.EffectiveAttribute("Population", -1)}");
            }

            Console.WriteLine();
            Console.WriteLine("Final world map:");

            for (int x = 0; x < world.Grid.Width; x++)
            {
                for (int y = 0; y < world.Grid.Height; y++)
                {
                    if (world.Grid.GetSquare(x, y).ThingsInGrid.Count == 0)
                    {
                        Console.Write("  ");
                    }
                    else
                    {
                        Console.Write($" {world.Grid.GetSquare(x, y).ThingsInGrid.Count:D1}");
                    }
                }

                Console.WriteLine();
            }
        }
    }
}
