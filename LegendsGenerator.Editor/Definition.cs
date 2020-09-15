// -------------------------------------------------------------------------------------------------
// <copyright file="Definition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using LegendsGenerator.Contracts.Definitions;
    using LegendsGenerator.Editor.ChangeHistory;
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

        /// <summary>
        /// Gets the history of this definition.
        /// </summary>
        public History History { get; } = new History();

        /// <summary>
        /// Gets the command which undoes the last change to this definition.
        /// </summary>
        public ICommand Undo
        {
            get
            {
                return new ActionCommand(() => this.History.Undo());
            }
        }

        /// <summary>
        /// Gets the command which redoes a change to this definition.
        /// </summary>
        public ICommand Redo
        {
            get
            {
                return new ActionCommand(() => this.History.Redo());
            }
        }

        /// <summary>
        /// A command which runs an action.
        /// </summary>
        private class ActionCommand : ICommand
        {
            /// <summary>
            /// The action to run.
            /// </summary>
            private Action act;

            /// <summary>
            /// Initializes a new instance of the <see cref="ActionCommand"/> class.
            /// </summary>
            /// <param name="act">The action to run.</param>
            public ActionCommand(Action act)
            {
                this.act = act;
                this.CanExecuteChanged += (s, e) => { };
            }

            /// <inheritdoc/>
#pragma warning disable CS0067 // Remove unused members.
            public event EventHandler? CanExecuteChanged;
#pragma warning restore CS0067 // Remove unsued members.

            /// <inheritdoc/>
            public bool CanExecute(object? parameter)
            {
                return true;
            }

            /// <inheritdoc/>
            public void Execute(object? parameter)
            {
                this.act();
            }
        }
    }
}
