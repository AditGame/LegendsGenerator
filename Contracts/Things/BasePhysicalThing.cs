// -------------------------------------------------------------------------------------------------
// <copyright file="BasePhysicalThing.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Contracts.Things
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using LegendsGenerator.Contracts.Definitions;

    /// <summary>
    /// A thing which has a physical location.
    /// </summary>
    public abstract record BasePhysicalThing : BaseThing
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasePhysicalThing"/> class.
        /// </summary>
        /// <param name="definition">The definition.</param>
        protected BasePhysicalThing(BaseThingDefinition definition)
            : base(definition)
        {
        }

        /// <summary>
        /// Gets or sets the X coordinate of this thing.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the Y coordinate of this thing.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Gets or sets the quests on this object.
        /// </summary>
        public ImmutableList<Quest> Quests { get; set; } = ImmutableList<Quest>.Empty;
    }
}
