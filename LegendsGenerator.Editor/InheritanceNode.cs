// -------------------------------------------------------------------------------------------------
// <copyright file="InheritanceNode.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;

    using LegendsGenerator.Contracts.Definitions;
    using LegendsGenerator.Contracts.Definitions.Events;

    /// <summary>
    /// A single node of inheritance.
    /// </summary>
    public class InheritanceNode : INotifyPropertyChanged
    {
        /// <summary>
        /// The name.
        /// </summary>
        private string name;

        /// <summary>
        /// The definition.
        /// </summary>
        private Definition? definition;

        /// <summary>
        /// Initializes a new instance of the <see cref="InheritanceNode"/> class.
        /// </summary>
        /// <param name="name">The name of this node.</param>
        /// <param name="nodes">The underlying nodes.</param>
        public InheritanceNode(string name, IEnumerable<InheritanceNode> nodes)
        {
            this.name = name;

            this.Nodes = new ObservableCollection<InheritanceNode>(nodes);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InheritanceNode"/> class.
        /// </summary>
        /// <param name="name">The name of this node.</param>
        /// <param name="definitions">The underlying definitions.</param>
        /// <param name="inheritanceList">The inheritance list.</param>
        public InheritanceNode(string name, IEnumerable<Definition> definitions, ILookup<string?, Definition>? inheritanceList)
        {
            this.name = name;

            foreach (Definition def in definitions)
            {
                this.Nodes.Add(new InheritanceNode(GetHeader(def.BaseDefinition), def, inheritanceList));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InheritanceNode"/> class.
        /// </summary>
        /// <param name="name">The name of this node.</param>
        /// <param name="definition">The definition of this node.</param>
        /// <param name="inheritanceList">The inheritance list.</param>
        public InheritanceNode(string name, Definition? definition, ILookup<string?, Definition>? inheritanceList)
        {
            this.name = name;
            this.Definition = definition;

            if (inheritanceList == null)
            {
                return;
            }

            foreach (var entry in inheritanceList[name])
            {
                this.Nodes.Add(new InheritanceNode(GetHeader(entry.BaseDefinition), entry, inheritanceList));
            }
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets or sets the name of this node.
        /// </summary>
        public string Name
        {
            get => this.name;

            set
            {
                this.name = value;
                this.OnPropertyChanged(nameof(this.Name));
            }
        }

        /// <summary>
        /// Gets or sets the definition of this inheritance node.
        /// </summary>
        public Definition? Definition
        {
            get => this.definition;

            set
            {
                this.definition = value;
                this.OnPropertyChanged(nameof(this.Definition));
            }
        }

        /// <summary>
        /// Gets the underlying nodes.
        /// </summary>
        public ObservableCollection<InheritanceNode> Nodes { get; private set; }
            = new ObservableCollection<InheritanceNode>();

        /// <summary>
        /// Parses te definition list, creating headers per type.
        /// </summary>
        /// <param name="definitions">The definitions.</param>
        /// <returns>The parsed headers.</returns>
        public static IEnumerable<InheritanceNode> ParseWithHeaders(IEnumerable<Definition> definitions)
        {
            IList<InheritanceNode> nodes = new List<InheritanceNode>();
            nodes.Add(new InheritanceNode("Events", Parse(definitions.Where(d => d.BaseDefinition is EventDefinition))));
            nodes.Add(new InheritanceNode("Sites", Parse(definitions.Where(d => d.BaseDefinition is SiteDefinition))));
            return nodes;
        }

        /// <summary>
        /// Parses a list of definitions into an inheritance list.
        /// </summary>
        /// <param name="definitions">The Definitons.</param>
        /// <returns>The list of inheritance nodes.</returns>
        public static IEnumerable<InheritanceNode> Parse(IEnumerable<Definition> definitions)
        {
            IEnumerable<Definition> thingDefs = definitions.Where(d => d.BaseDefinition is BaseThingDefinition);
            ILookup<string?, Definition> inheritanceList =
                thingDefs.ToLookup(s => (s.BaseDefinition as BaseThingDefinition)!.InheritsFrom);

            IEnumerable<Definition> nonThingDefs = definitions.Where(d => !(d.BaseDefinition is BaseThingDefinition));

            List<Definition> orphans = new List<Definition>();
            foreach (var item in inheritanceList)
            {
                if (item.Key == null)
                {
                    continue;
                }

                if (!thingDefs.Any(x => (x.BaseDefinition as BaseThingDefinition)!.Name.Equals(item.Key)))
                {
                    orphans.AddRange(item);
                }
            }

            IList<InheritanceNode> nodes = new List<InheritanceNode>();

            if (orphans.Any())
            {
                nodes.Add(new InheritanceNode("__Orphans__", orphans, inheritanceList));
            }

            foreach (Definition nonThing in nonThingDefs)
            {
                nodes.Add(new InheritanceNode(GetHeader(nonThing.BaseDefinition), nonThing, null));
            }

            foreach (Definition thing in inheritanceList[null])
            {
                nodes.Add(new InheritanceNode(GetHeader(thing.BaseDefinition), thing, inheritanceList));
            }

            return nodes;
        }

        /// <summary>
        /// Fires property changed events.
        /// </summary>
        /// <param name="propertyName">True when property changed.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Gets the header of the definition.
        /// </summary>
        /// <param name="def">The definition.</param>
        /// <returns>the string header.</returns>
        private static string GetHeader(BaseDefinition def)
        {
            if (def is BaseThingDefinition thing)
            {
                return thing.Name;
            }
            else if (def is EventDefinition eve)
            {
                return eve.Description;
            }
            else
            {
                return "idk";
            }
        }
    }
}
