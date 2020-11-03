// <copyright file="Unit.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Things
{
    using LegendsGenerator.Contracts.Definitions;
    using LegendsGenerator.Contracts.Definitions.Events;

    public record Unit : BaseThing
    {
        public Unit(BaseThingDefinition definition)
            : base(definition)
        {
        }

        /// <inheritdoc/>
        public override ThingType ThingType => ThingType.Unit;
    }
}
