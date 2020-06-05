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
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using LegendsGenerator.Contracts.Definitions;
    using LegendsGenerator.Editor.ContractParsing;

    public class Context : INotifyPropertyChanged
    {
        private Definition selectedDefinition;

        private DefinitionNode selectedNode;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<Definition> Definitions { get; } = new ObservableCollection<Definition>();

        public IEnumerable<InheritanceNode> InheritanceGraph => InheritanceNode.ParseWithHeaders(this.Definitions);

        public Definition SelectedDefinition
        {
            get => this.selectedDefinition;

            set
            {
                this.selectedDefinition = value;
                this.OnPropertyChanged(nameof(this.SelectedDefinition));
            }
        }

        public DefinitionNode SelectedNode
        {
            get => this.selectedNode;

            set
            {
                this.selectedNode = value;
                this.OnPropertyChanged(nameof(this.SelectedNode));
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

    /// <summary>
    /// The definition of a thing.
    /// </summary>
    public class Definition
    {
        public Definition(BaseDefinition definition)
        {
            this.BaseDefinition = definition;
            this.Nodes = new ObservableCollection<DefinitionNode>(DefinitionParser.ParseToNodes(definition.GetType(), definition));
        }

        /// <summary>
        /// Gets or sets the underlying definition.
        /// </summary>
        public BaseDefinition BaseDefinition { get; set; }

        /// <summary>
        /// Gets the parsed nodes of this definition.
        /// </summary>
        public ObservableCollection<DefinitionNode> Nodes { get; private set; }
            = new ObservableCollection<DefinitionNode>();
    }
}
