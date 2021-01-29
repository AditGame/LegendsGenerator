// -------------------------------------------------------------------------------------------------
// <copyright file="Context.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using LegendsGenerator.Compiler.CSharp;
    using LegendsGenerator.Contracts;
    using LegendsGenerator.Contracts.Compiler;
    using LegendsGenerator.Contracts.Definitions;
    using LegendsGenerator.Contracts.Definitions.Events;
    using LegendsGenerator.Editor.ContractParsing;
    using LegendsGenerator.Editor.DefinitionSelector;

    /// <summary>
    /// The underlying data context.
    /// </summary>
    public class Context : INotifyPropertyChanged
    {
        /// <summary>
        /// The string for all definition files.
        /// </summary>
        private const string AllDefinitionFiles = "<All>";

        /// <summary>
        /// The last loaded instance.
        /// </summary>
        private static Context? lastLoadedInstance;

        /// <summary>
        /// The currently selected definition.
        /// </summary>
        private Definition? selectedDefinition;

        /// <summary>
        /// The currently selected definition node.
        /// </summary>
        private PropertyNode? selectedNode;

        /// <summary>
        /// The currently opened directory.
        /// </summary>
        private string? openedDirectory;

        /// <summary>
        /// The filter on the definition file.
        /// </summary>
        private string? definitionFileFilter;

        /// <summary>
        /// Initializes a new instance of the <see cref="Context"/> class.
        /// </summary>
        public Context()
        {
        }

        /// <summary>
        /// Handles when a property is changed.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets a value indicating whether Context has ever been loaded.
        /// </summary>
        public static bool InstanceEverLoaded
        {
            get
            {
                return lastLoadedInstance != null;
            }
        }

        /// <summary>
        /// Gets the current instance if one exists.
        /// </summary>
        public static Context LastLoadedInstance
        {
            get
            {
                return lastLoadedInstance ?? throw new InvalidOperationException("Context has never been loaded.");
            }

            private set
            {
                lastLoadedInstance = value;
            }
        }

        /// <summary>
        /// Gets the list of all definitions.
        /// </summary>
        public ObservableCollection<Definition> Definitions { get; } = new ObservableCollection<Definition>();

        /// <summary>
        /// Gets the list of all definitions.
        /// </summary>
        public ObservableCollection<Definition> FilteredDefinitions { get; } = new ObservableCollection<Definition>();

        /// <summary>
        /// Gets the inheritance graph.
        /// </summary>
        public ObservableCollection<InheritanceNode> InheritanceGraph { get; } = new ObservableCollection<InheritanceNode>();

        /// <summary>
        /// Gets the condition compiler.
        /// </summary>
        public IConditionCompiler Compiler { get; private set; } = new ConditionCompiler<BaseGlobalVariables>(new BaseGlobalVariables()
        {
            World = new World(),
        });

        /// <summary>
        /// Gets the list of all opened files.
        /// </summary>
        public ObservableCollection<string> OpenedFilesSelector { get; } = new ObservableCollection<string>();

        /// <summary>
        /// Gets or sets the selected definition.
        /// </summary>
        public Definition? SelectedDefinition
        {
            get => this.selectedDefinition;

            set
            {
                this.selectedDefinition = value;
                this.OnPropertyChanged(nameof(this.SelectedDefinition));
            }
        }

        /// <summary>
        /// Gets or sets the selected definition node.
        /// </summary>
        public PropertyNode? SelectedNode
        {
            get => this.selectedNode;

            set
            {
                this.selectedNode = value;
                this.OnPropertyChanged(nameof(this.SelectedNode));
            }
        }

        /// <summary>
        /// Gets or sets the selected definition node.
        /// </summary>
        public string? OpenedDirectory
        {
            get => this.openedDirectory;

            set
            {
                this.openedDirectory = value;
                this.SetDefinitions(value);
                this.OnPropertyChanged(nameof(this.OpenedDirectory));
            }
        }

        /// <summary>
        /// Gets or sets the selected definition node.
        /// </summary>
        public string? DefinitionFileFilter
        {
            get => this.definitionFileFilter;

            set
            {
                this.FilteredDefinitions.Clear();
                this.definitionFileFilter = value;

                if (value == AllDefinitionFiles)
                {
                    value = null;
                }

                if (value != null)
                {
                    foreach (var def in this.Definitions)
                    {
                        if (this.MatchesFileFilter(def))
                        {
                            this.FilteredDefinitions.Add(def);
                        }
                    }
                }
                else
                {
                    foreach (var def in this.Definitions)
                    {
                        this.FilteredDefinitions.Add(def);
                    }
                }

                this.RegenerateInheritanceGraph();
                this.OnPropertyChanged(nameof(this.DefinitionFileFilter));
            }
        }

        /// <summary>
        /// Moves the inheritance node from it's current position into the proper position.
        /// </summary>
        /// <param name="node">The node.</param>
        public static void FixInheritanceNode(InheritanceNode node)
        {
            // Find the correct section
            InheritanceNode section = node;
            if (section.Upstream != null)
            {
                do
                {
                    section = section.Upstream;
                }
                while (section.Upstream != null);
            }
            else
            {
                throw new InvalidOperationException("Section headers cannot be moved.");
            }

            // Remove the node from it's current position.
            if (node.Upstream != null)
            {
                node.Upstream.Nodes.Remove(node);
            }

            // If it doesn't inherit anything, place at the root of it's section.
            if (node.InheritsFrom == null)
            {
                section.AddNode(node);
                return;
            }

            // If the new parent exists, add it to that node.
            InheritanceNode? newParent = FindNode(node.InheritsFrom, section);
            if (newParent != null)
            {
                newParent.AddNode(node);
            }
            else
            {
                // Otherwise put it with the orphans.
                InheritanceNode? orphanParent = FindNode(InheritanceNode.OrphanNodeName, section);
                if (orphanParent != null)
                {
                    orphanParent.AddNode(node);
                }
                else
                {
                    throw new InvalidOperationException("The node is an orphan and there is no orphan section.");
                }
            }
        }

        /// <summary>
        /// Attaches and initializes the compilation process.
        /// Must be called again every time a node is added or removed.
        /// </summary>
        public void Attach()
        {
            foreach (Definition def in this.Definitions)
            {
                def.BaseDefinition.Attach(this.Compiler);
            }
        }

        /// <summary>
        /// Adds a definition to the definition list.
        /// </summary>
        /// <param name="definition">The definition.</param>
        public void AddDefinition(Definition definition)
        {
            this.Definitions.Add(definition);
            definition.BaseDefinition.Attach(this.Compiler);

            if (this.MatchesFileFilter(definition))
            {
                this.FilteredDefinitions.Add(definition);
            }
        }

        /// <summary>
        /// Adds a definition to the definition list.
        /// </summary>
        /// <param name="definition">The definition.</param>
        public void RemoveDefinition(Definition definition)
        {
            this.Definitions.Remove(definition);
            this.FilteredDefinitions.Remove(definition);

            if (this.SelectedDefinition == definition)
            {
                this.SelectedDefinition = null;
                this.SelectedNode = null;
            }
        }

        /// <summary>
        /// Regenerates the inheritance graph.
        /// </summary>
        public void RegenerateInheritanceGraph()
        {
            this.InheritanceGraph.Clear();

            foreach (InheritanceNode entry in InheritanceNode.ParseWithHeaders(this.FilteredDefinitions, this.Definitions))
            {
                this.InheritanceGraph.Add(entry);
                this.AttachPropertyChanged(entry);
            }
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
        /// Traverses the tree looking for the node, or null if the node does not exist in the tree.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="from">The node to start from.</param>
        /// <returns>The node, or null.</returns>
        private static InheritanceNode? FindNode(string name, InheritanceNode from)
        {
            foreach (InheritanceNode subNode in from.Nodes)
            {
                if (subNode.Name.Equals(name, StringComparison.Ordinal))
                {
                    return subNode;
                }

                InheritanceNode? findResult = FindNode(name, subNode);
                if (findResult != null)
                {
                    return findResult;
                }
            }

            return null;
        }

        /// <summary>
        /// Sets the definitions.
        /// </summary>
        /// <param name="path">The path to the definitions.</param>
        private void SetDefinitions(string? path)
        {
            this.Definitions.Clear();
            this.InheritanceGraph.Clear();
            this.OpenedFilesSelector.Clear();

            if (path != null)
            {
                Definitions? definitions =
                    DefinitionSerializer.DeserializeFromDirectory(this.Compiler, path);

                foreach (var def in definitions.AllDefinitions)
                {
                    this.Definitions.Add(new Definition(def));
                }

                if (this.OpenedDirectory != null)
                {
                    this.OpenedFilesSelector.Add(AllDefinitionFiles);

                    foreach (string sourceName in definitions.AllDefinitions.OfType<ITopLevelDefinition>().Select(x => x.SourceFile).Distinct())
                    {
                        this.OpenedFilesSelector.Add(Path.ChangeExtension(Path.GetRelativePath(this.OpenedDirectory, sourceName), null));
                    }
                }
            }

            // This will fill the filtered list with entries.
            this.DefinitionFileFilter = null;

            LastLoadedInstance = this;

            this.RegenerateInheritanceGraph();
            this.Attach();
        }

        /// <summary>
        /// Attaches the property changed to this node and all child nodes.
        /// </summary>
        /// <param name="node">The node.</param>
        private void AttachPropertyChanged(InheritanceNode node)
        {
            node.PropertyChanged -= this.HandleInheritanceNodeChanged;
            node.PropertyChanged += this.HandleInheritanceNodeChanged;
            foreach (InheritanceNode inner in node.Nodes)
            {
                this.AttachPropertyChanged(inner);
            }
        }

        /// <summary>
        /// Handles events when the inheritance node has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The args.</param>
        private void HandleInheritanceNodeChanged(object? sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName?.Equals("InheritsFrom", StringComparison.Ordinal) == true && sender is InheritanceNode node)
            {
                FixInheritanceNode(node);
            }
        }

        /// <summary>
        /// Checks if the provided definition matches the file filter.
        /// </summary>
        /// <param name="def">The definition.</param>
        /// <returns>True if the definition matches.</returns>
        private bool MatchesFileFilter(Definition def)
        {
            string fullPath = $"{this.OpenedDirectory}\\{this.DefinitionFileFilter}.json";
            return def.BaseDefinition is ITopLevelDefinition topLevelDef && topLevelDef.SourceFile.Equals(fullPath, StringComparison.OrdinalIgnoreCase);
        }
    }
}
