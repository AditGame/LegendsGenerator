// -------------------------------------------------------------------------------------------------
// <copyright file="GraveyardEntryView.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Viewer.Views
{
    using LegendsGenerator.Contracts;

    /// <summary>
    /// A view into a graveyard entry.
    /// </summary>
    public class GraveyardEntryView
    {
        /// <summary>
        /// The inner graveyard entry.
        /// </summary>
        private readonly GraveyardEntry inner;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraveyardEntryView"/> class.
        /// </summary>
        /// <param name="entry">The graveyard entry.</param>
        /// <param name="world">The world.</param>
        public GraveyardEntryView(GraveyardEntry entry, World world)
        {
            this.inner = entry;
            this.Thing = new ThingView(entry.Thing, world);
        }

        /// <summary>
        /// Gets the last step this thing existed.
        /// </summary>
        public int StepDestroyed => this.inner.StepDestroyed;

        /// <summary>
        /// Gets the thing that was destroyed.
        /// </summary>
        public ThingView Thing { get; }

        /// <summary>
        /// Gets the display name of this thing.
        /// </summary>
        public string Name => $"{this.Thing.DefinitionName} {this.Thing.Name} (Destroyed Step {this.StepDestroyed})";
    }
}
