// <copyright file="GraveyardEntry.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts
{
    using LegendsGenerator.Contracts.Things;

    /// <summary>
    /// A thing which was destroyed in a prior step.
    /// </summary>
    public class GraveyardEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GraveyardEntry"/> class.
        /// </summary>
        /// <param name="thing">The thing which was destroyed.</param>
        /// <param name="step">The step it was destroyed.</param>
        public GraveyardEntry(BaseThing thing, int step)
        {
            this.Thing = thing;
            this.StepDestroyed = step;
        }

        /// <summary>
        /// Gets or sets the thing that was destroyed.
        /// </summary>
        public BaseThing Thing { get; set; }

        /// <summary>
        /// Gets or sets the last step this thing was in.
        /// </summary>
        public int StepDestroyed { get; set; }
    }
}
