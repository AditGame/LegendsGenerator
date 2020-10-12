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
    public record WorldSquare : BaseThing
    {
        /// <summary>
        /// Gets the site definition.
        /// </summary>
        public WorldSquareDefinition Definition => BaseDefinition as WorldSquareDefinition ?? throw new InvalidOperationException("Definition time is wrong type.");

        /// <inheritdoc/>
        public override ThingType ThingType => ThingType.WorldSquare;

        /// <summary>
        /// Gets a value indicating whether this square is water.
        /// </summary>
        public bool IsWater => this.Definition.IsWater;
    }
}
