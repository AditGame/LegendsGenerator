// -------------------------------------------------------------------------------------------------
// <copyright file="DefinitionNode.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor.ContractParsing
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Represents a node of the contract.
    /// </summary>
    public abstract class DefinitionNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefinitionNode"/> class.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="nullable">If the record can be set to null or not.</param>
        public DefinitionNode(
            string description,
            bool nullable)
        {
            this.Description = description;
            this.Nullable = nullable;
        }

        /// <summary>
        /// Gets the name of the node, typically either the property name or the dictionary key.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the description of this node.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets a value indicating whether this node can be set to null.
        /// </summary>
        public bool Nullable { get; }

        /// <summary>
        /// Gets or sets the contents of the node; null if there's no contents beyond sub nodes.
        /// </summary>
        public object? Content
        {
            get
            {
                return this.GetContentsFunc?.Invoke();
            }

            set
            {
                if (!this.ContentsModifiable)
                {
                    throw new InvalidOperationException("Can not modify contents");
                }

                if (this.SetContentsFunc == null)
                {
                    throw new InvalidOperationException($"{nameof(this.SetContentsFunc)} msut be attached to set contents.");
                }

                this.SetContentsFunc.Invoke(value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this node can be renamed.
        /// </summary>
        public bool Renamable { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether the contents are modifiable.
        /// </summary>
        public bool ContentsModifiable { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether the subnodes can be modified.
        /// </summary>
        public bool SubNodesModifiable { get; protected set; }

        /// <summary>
        /// Gets the list of additional options which can be added to this node.
        /// </summary>
        public List<DefinitionNode> Options { get; } = new List<DefinitionNode>();

        /// <summary>
        /// Gets the list of subnodes on this node.
        /// </summary>
        public List<DefinitionNode> SubNodes { get; } = new List<DefinitionNode>();

        /// <summary>
        /// Gets the list of validation failures on this node.
        /// </summary>
        public IList<string> ValidationFailures
        {
            get
            {
                List<string> failures = new List<string>();
                failures.AddRange(this.ValidateNodeFunc());
                this.Options.ForEach(node => failures.AddRange(node.ValidationFailures));
                this.SubNodes.ForEach(node => failures.AddRange(node.ValidationFailures));
                return failures;
            }
        }

        /// <summary>
        /// Gets or sets a function which returns the contents of this node.
        /// </summary>
        protected Func<object>? GetContentsFunc { get; set; }

        /// <summary>
        /// Gets or sets a function which sets the contents of this node.
        /// </summary>
        protected Action<object?>? SetContentsFunc { get; set; }

        /// <summary>
        /// Gets or sets a function which validates that the contents are valid.
        /// </summary>
        protected Func<IList<string>> ValidateNodeFunc { get; set; } = () => Array.Empty<string>();

        /// <summary>
        /// Renames this instance.
        /// </summary>
        /// <param name="name">The new name to apply.</param>
        public virtual void Rename(string name)
        {
            throw new NotImplementedException($"{nameof(this.Rename)} is not implemented.");
        }

        /// <summary>
        /// gets the UI element to display as the content.
        /// </summary>
        /// <returns>The UI element.</returns>
        public virtual UIElement GetContentElement()
        {
            return new TextBlock()
            {
                Text = this.Content?.ToString(),
            };
        }

        /// <summary>
        /// Converts an action to be an object action.
        /// </summary>
        /// <typeparam name="T">The typical input type.</typeparam>
        /// <param name="action">The action to convert.</param>
        /// <returns>The action, which now takes object? as input.</returns>
        protected static Action<object?> ConvertAction<T>(Action<T> action)
        {
            return new Action<object?>(input =>
            {
                if (input is T asT)
                {
                    action(asT);
                }
                else
                {
                    throw new InvalidOperationException($"Must set as {typeof(T).Name}.");
                }
            });
        }
    }
}
