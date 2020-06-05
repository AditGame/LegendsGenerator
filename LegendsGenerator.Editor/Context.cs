// -------------------------------------------------------------------------------------------------
// <copyright file="Context.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using LegendsGenerator.Compiler.CSharp;
    using LegendsGenerator.Contracts.Compiler;
    using LegendsGenerator.Contracts.Definitions;
    using LegendsGenerator.Editor.ContractParsing;

    /// <summary>
    /// The underlying data context.
    /// </summary>
    public class Context : INotifyPropertyChanged
    {
        /// <summary>
        /// The currently selected definition.
        /// </summary>
        private Definition? selectedDefinition;

        /// <summary>
        /// The currently selected definition node.
        /// </summary>
        private DefinitionNode? selectedNode;

        /// <summary>
        /// Handles when a property is changed.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets the list of all definitions.
        /// </summary>
        public ObservableCollection<Definition> Definitions { get; } = new ObservableCollection<Definition>();

        /// <summary>
        /// Gets the inheritance graph.
        /// </summary>
        public IEnumerable<InheritanceNode> InheritanceGraph => InheritanceNode.ParseWithHeaders(this.Definitions);

        /// <summary>
        /// Gets the condition compiler.
        /// </summary>
        public IConditionCompiler Compiler { get; private set; } = new ConditionCompiler(new Dictionary<string, object>());

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
        public DefinitionNode? SelectedNode
        {
            get => this.selectedNode;

            set
            {
                this.selectedNode = value;
                this.OnPropertyChanged(nameof(this.SelectedNode));
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
        /// Fires property changed events.
        /// </summary>
        /// <param name="propertyName">True when property changed.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
