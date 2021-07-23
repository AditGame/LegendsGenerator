// -------------------------------------------------------------------------------------------------
// <copyright file="DefinitionsCollection.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Contracts.Definitions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using LegendsGenerator.Contracts.Compiler;
    using LegendsGenerator.Contracts.Definitions.Events;

    /// <summary>
    /// A collection of all definitions.
    /// </summary>
#pragma warning disable CA1010 // Generic interface should also be implemented. Only RO collection.
    public sealed class DefinitionsCollection : ICollection, IReadOnlyCollection<BaseDefinition>
#pragma warning restore CA1010 // Generic interface should also be implemented
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefinitionsCollection"/> class.
        /// </summary>
        /// <param name="definitions">The list of definitions.</param>
        public DefinitionsCollection(IEnumerable<BaseDefinition> definitions)
        {
            // We store all definitions in case a new one is added and we forget to add it to the below lists.
            this.AllDefinitions = definitions.ToList();

            ILookup<System.Type, BaseDefinition>? byType = definitions.ToLookup(d => d.GetType());
            this.Events = byType[typeof(EventDefinition)].OfType<EventDefinition>().ToList();
            this.SiteDefinitions = byType[typeof(SiteDefinition)].OfType<SiteDefinition>().ToList();
            this.NotablePersonDefinitions = byType[typeof(NotablePersonDefinition)].OfType<NotablePersonDefinition>().ToList();
            this.WorldSquareDefinitions = byType[typeof(WorldSquareDefinition)].OfType<WorldSquareDefinition>().ToList();
            this.WorldDefinitions = byType[typeof(WorldDefinition)].OfType<WorldDefinition>().ToList();
            this.UnitDefinitions = byType[typeof(UnitDefinition)].OfType<UnitDefinition>().ToList();
            this.QuestDefinitions = byType[typeof(QuestDefinition)].OfType<QuestDefinition>().ToList();
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
        /// Gets the list of all world definitions (only one should really be needed).
        /// </summary>
        public IReadOnlyList<WorldDefinition> WorldDefinitions { get; }

        /// <summary>
        /// Gets the list of all quest definitions.
        /// </summary>
        public IReadOnlyList<QuestDefinition> QuestDefinitions { get; }

        /// <summary>
        /// Gets the list of all unit definitions.
        /// </summary>
        public IReadOnlyList<UnitDefinition> UnitDefinitions { get; }

        /// <summary>
        /// Gets the list of all definitions.
        /// </summary>
        public IReadOnlyList<BaseDefinition> AllDefinitions { get; }

        /// <summary>
        /// Gets a value indicating whether inheritance has been compiled.
        /// </summary>
        public bool IsInheritanceCompiled { get; private set; }

        /// <inheritdoc/>
        public int Count => this.AllDefinitions.Count;

        /// <inheritdoc/>
        bool ICollection.IsSynchronized => true;

        /// <inheritdoc/>
        object ICollection.SyncRoot { get; } = new object();

        /// <summary>
        /// Creates a new collection by combining this collection with additional collection.
        /// </summary>
        /// <param name="additionalCollections">Collections to combine with this collection.</param>
        /// <returns>A combination of the two collections.</returns>
        public DefinitionsCollection Combine(params DefinitionsCollection[] additionalCollections)
        {
            IEnumerable<BaseDefinition> combinedDefinitions = this.AllDefinitions;

            foreach (DefinitionsCollection collection in additionalCollections)
            {
                combinedDefinitions = combinedDefinitions.Concat(collection.AllDefinitions);
            }

            return new DefinitionsCollection(combinedDefinitions);
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

        /// <summary>
        /// Follows all inheritance to combine attribute and aspect lists.
        /// </summary>
        public void CompileInheritance()
        {
            this.IsInheritanceCompiled = true;

            List<BaseThingDefinition> allThingDefinitions = this.AllDefinitions.OfType<BaseThingDefinition>().ToList();
            foreach (BaseThingDefinition definition in allThingDefinitions)
            {
                IList<BaseThingDefinition> inheritanceList = new List<BaseThingDefinition>();

                // Get the inheritance list, from top to bottom.
                string? thingToSearchFor = definition.Name;
                while (thingToSearchFor != null)
                {
                    BaseThingDefinition? matchingDef =
                        allThingDefinitions.FirstOrDefault(d => d.Name.Equals(thingToSearchFor, StringComparison.OrdinalIgnoreCase));

                    if (matchingDef == null)
                    {
                        throw new InvalidOperationException(
                            $"{definition.GetType().Name} missing definition for {thingToSearchFor} " +
                            $"(required by {definition.GetType().Name} {definition.Name})");
                    }

                    inheritanceList.Add(matchingDef);

                    thingToSearchFor = matchingDef.InheritsFrom;
                }

                // Iterate from the top of the inheritance list to the bottom, adding in all aspects and attributes.
                foreach (BaseThingDefinition inherited in inheritanceList)
                {
                    foreach (var (name, attribute) in inherited.Attributes)
                    {
                        if (!definition.Attributes.ContainsKey(name))
                        {
                            definition.Attributes[name] = attribute;
                        }
                    }

                    foreach (var (name, aspect) in inherited.Aspects)
                    {
                        if (!definition.Aspects.ContainsKey(name))
                        {
                            definition.Aspects[name] = aspect;
                        }
                    }
                }

                definition.InheritedDefinitionNames = inheritanceList.Select(x => x.Name).ToList();
            }
        }

        /// <inheritdoc/>
        void ICollection.CopyTo(Array array, int index)
        {
            ((ICollection)this.AllDefinitions).CopyTo(array, index);
        }

        /// <inheritdoc/>
        public IEnumerator<BaseDefinition> GetEnumerator()
        {
            return this.AllDefinitions.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.AllDefinitions.GetEnumerator();
        }
    }
}
