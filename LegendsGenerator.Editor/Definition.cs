// -------------------------------------------------------------------------------------------------
// <copyright file="Definition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor
{
    using System.Collections.ObjectModel;

    using LegendsGenerator.Contracts.Definitions;
    using LegendsGenerator.Editor.ContractParsing;

    /// <summary>
    /// The definition of a thing.
    /// </summary>
    public class Definition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Definition"/> class.
        /// </summary>
        /// <param name="definition">The definition.</param>
        public Definition(BaseDefinition definition)
        {
            this.BaseDefinition = definition;
            this.Nodes = new ObservableCollection<PropertyNode>(DefinitionParser.ParseToNodes(definition.GetType(), definition));
        }

        /// <summary>
        /// Gets or sets the underlying definition.
        /// </summary>
        public BaseDefinition BaseDefinition { get; set; }

        /// <summary>
        /// Gets the parsed nodes of this definition.
        /// </summary>
        public ObservableCollection<PropertyNode> Nodes { get; private set; }
            = new ObservableCollection<PropertyNode>();
    }
}
