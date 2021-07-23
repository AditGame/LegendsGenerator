// -------------------------------------------------------------------------------------------------
// <copyright file="WorldDefinition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Contracts.Definitions
{
    using LegendsGenerator.Contracts.Definitions.Events;

    /// <summary>
    /// The definition of the world.
    /// </summary>
    public partial class WorldDefinition : BaseThingDefinition
    {
        /// <inheritdoc/>
        public override ThingType ThingType => ThingType.World;
    }
}
