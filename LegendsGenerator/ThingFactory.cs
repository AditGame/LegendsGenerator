// <copyright file="ThingFactory.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using LegendsGenerator.Contracts.Definitions;
    using LegendsGenerator.Contracts.Definitions.Events;
    using LegendsGenerator.Contracts.Things;

    /// <summary>
    /// Creates things.
    /// </summary>
    public class ThingFactory : IThingFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThingFactory"/> class.
        /// </summary>
        /// <param name="definitions">The definitions.</param>
        public ThingFactory(
            DefinitionCollection definitions)
        {
            this.Definitions = definitions;
            this.Definitions.CompileInheritance();
        }

        /// <summary>
        /// Gets the definitions.
        /// </summary>
        public DefinitionCollection Definitions { get; }

        /// <inheritdoc/>
        public BaseThing CreateThing(Random rdm, int x, int y, ThingType type, string definitionName)
        {
            return type switch
            {
                ThingType.Site => this.CreateSite(rdm, x, y, definitionName),
                ThingType.NotablePerson => this.CreateNotablePerson(rdm, x, y, definitionName),
                ThingType.WorldSquare => this.CreateWorldSquare(rdm, x, y, definitionName),
                _ => throw new InvalidOperationException($"Can not handle {type}."),
            };
        }

        /// <inheritdoc/>
        public Site CreateSite(Random rdm, int x, int y, string siteDefinitionName)
        {
            Site site = CreateThing<Site, SiteDefinition>(
                rdm,
                x,
                y,
                d => new Site(d),
                this.Definitions.SiteDefinitions,
                siteDefinitionName);
            site.Name = new RandomNameGeneratorLibrary.PlaceNameGenerator(rdm).GenerateRandomPlaceName();
            return site;
        }

        /// <inheritdoc/>
        public NotablePerson CreateNotablePerson(Random rdm, int x, int y, string personDefinitionName)
        {
            NotablePerson person = CreateThing<NotablePerson, NotablePersonDefinition>(
                rdm,
                x,
                y,
                d => new NotablePerson(d),
                this.Definitions.NotablePersonDefinitions,
                personDefinitionName);
            person.Name = new RandomNameGeneratorLibrary.PersonNameGenerator(rdm).GenerateRandomFirstAndLastName();
            return person;
        }

        /// <inheritdoc/>
        public WorldSquare CreateWorldSquare(Random rdm, int x, int y, string worldSquareDefinitionName)
        {
            WorldSquare square = CreateThing<WorldSquare, WorldSquareDefinition>(
                rdm,
                x,
                y,
                d => new WorldSquare(d),
                this.Definitions.WorldSquareDefinitions,
                worldSquareDefinitionName);
            square.Name = $"{x},{y}";
            return square;
        }

        /// <summary>
        /// Creates a thing.
        /// </summary>
        /// <typeparam name="TThing">The type of the thing to create.</typeparam>
        /// <typeparam name="TDefinition">The type of definition.</typeparam>
        /// <param name="rdm">The random number generator.</param>
        /// <param name="x">The X position.</param>
        /// <param name="y">The Y position.</param>
        /// <param name="createFunc">The function to use to create the thing.</param>
        /// <param name="definitions">The list of all definitions for this thing type.</param>
        /// <param name="thingName">The name of the thing to make.</param>
        /// <returns>The created thing.</returns>
        private static TThing CreateThing<TThing, TDefinition>(
            Random rdm,
            int x,
            int y,
            Func<TDefinition, TThing> createFunc,
            IReadOnlyList<TDefinition> definitions,
            string thingName)
            where TDefinition : BaseThingDefinition
            where TThing : BaseThing
        {
            TDefinition? definition =
                definitions.FirstOrDefault(d => d.Name.Equals(thingName, StringComparison.OrdinalIgnoreCase));

            if (definition == null)
            {
                throw new InvalidOperationException(
                    $"{typeof(TDefinition).Name} missing definition for {thingName}");
            }

            TThing thing = createFunc(definition);

            thing.X = x;
            thing.Y = y;

            foreach (var (attributeName, attributeDef) in definition.Attributes)
            {
                int value = attributeDef.EvalBaseValue(rdm);

                thing.BaseAttributes[attributeName] = value;
            }

            foreach (var (aspectName, aspectDef) in definition.Aspects)
            {
                string value = aspectDef.Dynamic ? string.Empty : aspectDef.EvalValueSafe(rdm);

                thing.BaseAspects[aspectName] = value;
            }

            return thing;
        }
    }
}
