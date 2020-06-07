// -------------------------------------------------------------------------------------------------
// <copyright file="InheritanceNode.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;

    using LegendsGenerator.Contracts;
    using LegendsGenerator.Contracts.Definitions;
    using LegendsGenerator.Contracts.Definitions.Events;
    using LegendsGenerator.Editor.ContractParsing;

    /// <summary>
    /// A single node of inheritance.
    /// </summary>
    public class InheritanceNode : INotifyPropertyChanged
    {
        /// <summary>
        /// The nameof nodes with orphans.
        /// </summary>
        public static readonly string OrphanNodeName = "__Orphans__";

        /// <summary>
        /// The name.
        /// </summary>
        private string name;

        /// <summary>
        /// What this node inherits from.
        /// </summary>
        private string? inherits;

        /// <summary>
        /// The definition.
        /// </summary>
        private Definition? definition;

        /// <summary>
        /// If true, will hide this node if it's empty.
        /// </summary>
        private bool hideIfEmpty;

        /// <summary>
        /// Initializes a new instance of the <see cref="InheritanceNode"/> class.
        /// </summary>
        /// <param name="name">The name of this node.</param>
        /// <param name="nodes">The underlying nodes.</param>
        public InheritanceNode(string name, IEnumerable<InheritanceNode> nodes)
        {
            this.name = name;

            foreach (InheritanceNode node in nodes)
            {
                this.AddNode(node);
            }
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
                this.AddNode(new InheritanceNode(GetHeader(def.BaseDefinition), def, inheritanceList));
            }

            // Wire up that, if the collection is modified, we may need to hide this node.
            this.Nodes.CollectionChanged += (s, e) =>
            {
                if (this.HideIfEmpty)
                {
                    this.OnPropertyChanged(nameof(this.Visible));
                }
            };
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

            // Wire up property changed notifier.
            if (definition != null)
            {
                foreach (PropertyNode node in definition.Nodes.Where(d => d.ControlsDefinitionName))
                {
                    node.PropertyChanged += (s, e) =>
                    {
                        if (e.PropertyName?.Equals("Content") == true)
                        {
                            this.Name = GetHeader(definition.BaseDefinition);
                        }
                    };
                }

                PropertyNode? inheritsNode = definition.Nodes.FirstOrDefault(n => n.Name.Equals("InheritsFrom"));
                if (inheritsNode != null)
                {
                    inheritsNode.PropertyChanged += (s, e) =>
                    {
                        if (e.PropertyName?.Equals("Content") == true)
                        {
                            this.InheritsFrom = inheritsNode.Content as string;
                        }
                    };
                }
            }

            // Continue with inherited nodes.
            if (inheritanceList != null)
            {
                foreach (var entry in inheritanceList[name])
                {
                    this.AddNode(new InheritanceNode(GetHeader(entry.BaseDefinition), entry, inheritanceList));
                }
            }

            // Wire up that, if the collection is modified, we may need to hide this node.
            this.Nodes.CollectionChanged += (s, e) =>
            {
                if (this.HideIfEmpty)
                {
                    this.OnPropertyChanged(nameof(this.Visible));
                }
            };
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
        /// Gets or sets the name of what this inherits.
        /// </summary>
        public string? InheritsFrom
        {
            get => this.inherits;

            set
            {
                this.inherits = value;
                this.OnPropertyChanged(nameof(this.InheritsFrom));
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
        /// Gets or sets a value indicating whether this node should be hidden if it's empty.
        /// </summary>
        public bool HideIfEmpty
        {
            get => this.hideIfEmpty;

            set
            {
                this.hideIfEmpty = value;
                this.OnPropertyChanged(nameof(this.HideIfEmpty));
                this.OnPropertyChanged(nameof(this.Visible));
            }
        }

        /// <summary>
        /// Gets a value indicating whether this node should be hidden.
        /// </summary>
        public bool Visible => !this.HideIfEmpty || this.Nodes.Any();

        /// <summary>
        /// Gets a value indicating whether this node can be deleted.
        /// </summary>
        public bool CanDelete => this.definition != null;

        /// <summary>
        /// Gets a value indicating whether this node can be created.
        /// </summary>
        public bool CanCreate => this.definition == null && this.name != OrphanNodeName;

        /// <summary>
        /// Gets or sets a value indicating whether the name can be changed in the list.
        /// </summary>
        public bool NameCanBeChanged => false;

        /// <summary>
        /// Gets or sets the upstream node.
        /// </summary>
        public InheritanceNode? Upstream { get; protected set; }

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

            nodes.Add(new InheritanceNode(OrphanNodeName, orphans, inheritanceList)
            {
                HideIfEmpty = true,
            });

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
        /// Adds the node to the child nodes.
        /// </summary>
        /// <param name="node">The node to add.</param>
        public void AddNode(InheritanceNode node)
        {
            node.Upstream = this;
            this.Nodes.Add(node);
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
            if (def is ITopLevelDefinition thing)
            {
                return thing.DefinitionName;
            }
            else
            {
                return "idk";
            }
        }
    }
}
