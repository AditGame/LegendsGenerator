// <copyright file="WorldThing.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Things
{
    using LegendsGenerator.Contracts.Definitions;
    using LegendsGenerator.Contracts.Definitions.Events;

    public record WorldThing : BaseThing
    {
        public WorldThing(WorldDefinition definition)
            : base(definition)
        {
        }

        public override ThingType ThingType => ThingType.World;
    }
}
