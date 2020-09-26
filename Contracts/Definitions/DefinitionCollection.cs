// <copyright file="DefinitionCollections.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions
{
    using System.Collections.Generic;
    using System.Linq;

    using LegendsGenerator.Contracts.Compiler;
    using LegendsGenerator.Contracts.Definitions.Events;

    /// <summary>
    /// A collection of all definitions.
    /// </summary>
    public class DefinitionCollection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefinitionCollection"/> class.
        /// </summary>
        /// <param name="definitions">The list of definitions.</param>
        public DefinitionCollection(IEnumerable<BaseDefinition> definitions)
        {
            // We store all definitions in case a new one is added and we forget to add it to the below lists.
            this.AllDefinitions = definitions.ToList();

            ILookup<System.Type, BaseDefinition>? byType = definitions.ToLookup(d => d.GetType());
            this.Events = byType[typeof(EventDefinition)].OfType<EventDefinition>().ToList();
            this.SiteDefinitions = byType[typeof(SiteDefinition)].OfType<SiteDefinition>().ToList();
            this.NotablePersonDefinitions = byType[typeof(NotablePersonDefinition)].OfType<NotablePersonDefinition>().ToList();
            this.WorldSquareDefinitions = byType[typeof(WorldSquareDefinition)].OfType<WorldSquareDefinition>().ToList();
        }

        /// <summary>
        /// Gets all definitions.
        /// </summary>
        public IReadOnlyList<EventDefinition> Events { get; }

        /// <summary>
        /// Gets the list of all site definitions.
        /// </summary>
        public IReadOnlyList<SiteDefinition> SiteDefinitions { get; }

        /// <summary>
        /// Gets the list of all notable person definitions.
        /// </summary>
        public IReadOnlyList<NotablePersonDefinition> NotablePersonDefinitions { get; }

        /// <summary>
        /// Gets the list of all world square definitions.
        /// </summary>
        public IReadOnlyList<WorldSquareDefinition> WorldSquareDefinitions { get; }

        /// <summary>
        /// Gets the list of all definitions.
        /// </summary>
        public IReadOnlyList<BaseDefinition> AllDefinitions { get; }

        /// <summary>
        /// Creates a new collection by combining this collection with additional collection.
        /// </summary>
        /// <param name="additionalCollections">Collections to combine with this collection.</param>
        /// <returns>A combination of the two collections.</returns>
        public DefinitionCollection Combine(params DefinitionCollection[] additionalCollections)
        {
            IEnumerable<BaseDefinition> combinedDefinitions = this.AllDefinitions;

            foreach (DefinitionCollection collection in additionalCollections)
            {
                combinedDefinitions = combinedDefinitions.Concat(collection.AllDefinitions);
            }

            return new DefinitionCollection(combinedDefinitions);
        }

        /// <summary>
        /// Attaches the compiler to all definitions.
        /// </summary>
        /// <param name="compiler">The csmpiler.</param>
        public void Attach(IConditionCompiler compiler)
        {
            foreach (var def in this.AllDefinitions)
            {
                def.Attach(compiler);
            }
        }
    }
}
