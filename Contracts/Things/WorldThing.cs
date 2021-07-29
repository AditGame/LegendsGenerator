// <copyright file="WorldThing.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Things
{
    using System;
    using LegendsGenerator.Contracts.Definitions;
    using LegendsGenerator.Contracts.Definitions.Events;

    /// <summary>
    /// The world.
    /// </summary>
    public record WorldThing : BaseThing
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorldThing"/> class.
        /// </summary>
        /// <param name="definition">The world definition.</param>
        /// <param name="rdm">Random number generator.</param>
        public WorldThing(WorldDefinition definition, Random rdm)
            : base(definition, rdm)
        {
        }

        /// <inheritdoc/>
        public override ThingType ThingType => ThingType.World;
    }
}
