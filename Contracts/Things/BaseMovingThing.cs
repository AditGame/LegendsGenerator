// <copyright file="BaseMovingThing.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Things
{
    using System;

    /// <summary>
    /// A thing which can move on the world map.
    /// </summary>
    public abstract record BaseMovingThing : BaseThing
    {
        /// <summary>
        /// Gets or sets the type of movement expected.
        /// </summary>
        public MoveType MoveType { get; set; }

        /// <summary>
        /// Gets or sets the ID of a thing to travel towards. If null, this is not traveling towards anything.
        /// </summary>
        public Guid? MoveToThing { get; set; }

        /// <summary>
        /// Gets or sets the X coord to move towards.
        /// </summary>
        public int? MoveToCoordX { get; set; }

        /// <summary>
        /// Gets or sets the Y coord to move towards.
        /// </summary>
        public int? MoveToCoordY { get; set; }

        /// <summary>
        /// Gets a value indicating whether this thing is moving towards something.
        /// </summary>
        public bool IsMoving => this.MoveType != MoveType.Unknown;
    }
}
