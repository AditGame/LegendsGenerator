// -------------------------------------------------------------------------------------------------
// <copyright file="PropertyNodeHistoryItem.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor.ChangeHistory
{
    using LegendsGenerator.Editor.ContractParsing;

    /// <summary>
    /// The history node for a property item change.
    /// </summary>
    public class PropertyNodeHistoryItem : HistoryItem
    {
        /// <summary>
        /// The property node.
        /// </summary>
        private readonly PropertyNode node;

        /// <summary>
        /// The previous value.
        /// </summary>
        private readonly object? previousValue;

        /// <summary>
        /// The next value.
        /// </summary>
        private readonly object? nextValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyNodeHistoryItem"/> class.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="previousValue">The previous value.</param>
        /// <param name="nextValue">The next value.</param>
        public PropertyNodeHistoryItem(PropertyNode node, object? previousValue, object? nextValue)
        {
            this.node = node;
            this.previousValue = previousValue;
            this.nextValue = nextValue;
        }

        /// <inheritdoc/>
        public override string PreviousString => this.previousValue?.ToString() ?? "<null>";

        /// <inheritdoc/>
        public override string NextString => this.nextValue?.ToString() ?? "<null>";

        /// <inheritdoc/>
        public override string Name => this.node.FullName;

        /// <inheritdoc/>
        protected override void UndoInner()
        {
            this.node.SetContentBypassingHistory(this.previousValue);
        }

        /// <inheritdoc/>
        protected override void RedoInner()
        {
            this.node.SetContentBypassingHistory(this.nextValue);
        }
    }
}
