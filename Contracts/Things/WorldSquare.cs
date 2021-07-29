// <copyright file="WorldSquare.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Things
{
    using System;

    using LegendsGenerator.Contracts.Definitions;
    using LegendsGenerator.Contracts.Definitions.Events;

    /// <summary>
    /// A square on the world stage.
    /// </summary>
    public record WorldSquare : BasePhysicalThing
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorldSquare"/> class.
        /// </summary>
        /// <param name="definition">The world square definition.</param>
        /// <param name="rdm">Random number generator.</param>
        public WorldSquare(WorldSquareDefinition definition, Random rdm)
            : base(definition, rdm)
        {
        }

        /// <summary>
        /// Gets the site definition.
        /// </summary>
        public WorldSquareDefinition Definition => this.BaseDefinition as WorldSquareDefinition ?? throw new InvalidOperationException("Definition is wrong type.");

        /// <inheritdoc/>
        public override ThingType ThingType => ThingType.WorldSquare;
    }
}
