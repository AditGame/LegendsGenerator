// <copyright file="ThingFactory.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using LegendsGenerator.Contracts;
    using LegendsGenerator.Contracts.Definitions;

    /// <summary>
    /// Creates things.
    /// </summary>
    public class ThingFactory
    {
        /// <summary>
        /// The definitions.
        /// </summary>
        private DefinitionCollections definitions;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThingFactory"/> class.
        /// </summary>
        /// <param name="definitions">The definitions.</param>
        public ThingFactory(
            DefinitionCollections definitions)
        {
            this.definitions = definitions;
        }

        /// <summary>
        /// Creates a site.
        /// </summary>
        /// <param name="rdm">The random nubmer generator.</param>
        /// <param name="siteDefinitionName">The name of the site's definition.</param>
        /// <returns>The site.</returns>
        public Site CreateSite(Random rdm, string siteDefinitionName)
        {
            return CreateThing<Site, SiteDefinition>(rdm, this.definitions.SiteDefinitions, siteDefinitionName);
        }

        /// <summary>
        /// Creates a thing.
        /// </summary>
        /// <typeparam name="TThing">The type of the thing to create.</typeparam>
        /// <typeparam name="TDefinition">The type of definition.</typeparam>
        /// <param name="rdm">The random nubmer generator.</param>
        /// <param name="definitions">The list of all definitions for this thing type.</param>
        /// <param name="thingName">The name of the thing to make.</param>
        /// <returns>The created thing.</returns>
        private static TThing CreateThing<TThing, TDefinition>(
            Random rdm,
            IReadOnlyList<TDefinition> definitions,
            string thingName)
            where TDefinition : BaseThingDefinition
            where TThing : BaseThing, new()
        {
            IList<TDefinition> inheritanceList = new List<TDefinition>();

            string? thingToSearchFor = thingName;
            while (thingToSearchFor != null)
            {
                TDefinition? definition =
                    definitions.FirstOrDefault(d => d.Name.Equals(thingToSearchFor, StringComparison.OrdinalIgnoreCase));

                if (definition == null)
                {
                    throw new InvalidOperationException(
                        $"{typeof(TDefinition).Name} missing definition for {thingToSearchFor} " +
                        $"(required by {typeof(TDefinition).Name} {thingName})");
                }

                inheritanceList.Add(definition);

                thingToSearchFor = definition.InheritsFrom;
            }

            TThing thing = new TThing()
            {
                BaseDefinition = inheritanceList.First(),
            };

            foreach (var inheritedDefinition in inheritanceList.Reverse())
            {
                foreach (var (attributeName, _) in inheritedDefinition.DefaultAttributes)
                {
                    int value = inheritedDefinition.EvalDefaultAttributes(attributeName, rdm);

                    thing.BaseAttributes[attributeName] = value;
                }
            }

            foreach (var inheritedDefinition in inheritanceList.Reverse())
            {
                foreach (var (aspectName, _) in inheritedDefinition.DefaultAspects)
                {
                    string value = inheritedDefinition.EvalDefaultAspects(aspectName, rdm);

                    // TODO: Wire in aspects.
                }
            }

            return thing;
        }
    }
}
