// -------------------------------------------------------------------------------------------------
// <copyright file="ActionHistoryItem.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor.ChangeHistory
{
    using System;

    /// <summary>
    /// A history item which uses Actions to traverse time.
    /// </summary>
    public class ActionHistoryItem : HistoryItem
    {
        /// <summary>
        /// The undo action.
        /// </summary>
        private readonly Action undo;

        /// <summary>
        /// The redo action.
        /// </summary>
        private readonly Action redo;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionHistoryItem"/> class.
        /// </summary>
        /// <param name="name">The name of what this affects.</param>
        /// <param name="previousName">The string to show as a previous value.</param>
        /// <param name="nextName">The string to show as the next value.</param>
        /// <param name="undoAction">The action which undoes this history item.</param>
        /// <param name="redoAction">The action which redoes this history item.</param>
        public ActionHistoryItem(
            string name,
            string previousName,
            string nextName,
            Action undoAction,
            Action redoAction)
        {
            this.Name = name;
            this.PreviousString = previousName;
            this.NextString = nextName;
            this.undo = undoAction;
            this.redo = redoAction;
        }

        /// <inheritdoc/>
        public override string PreviousString { get; }

        /// <inheritdoc/>
        public override string NextString { get; }

        /// <inheritdoc/>
        public override string Name { get; }

        /// <inheritdoc/>
        protected override void UndoInner()
        {
            this.undo();
        }

        /// <inheritdoc/>
        protected override void RedoInner()
        {
            this.redo();
        }
    }
}
