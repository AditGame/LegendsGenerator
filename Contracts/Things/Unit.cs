// <copyright file="Unit.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Things
{
    using LegendsGenerator.Contracts.Definitions;
    using LegendsGenerator.Contracts.Definitions.Events;

    /// <summary>
    /// Represents a Unit.
    /// </summary>
    public record Unit : BaseMovingThing
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Unit"/> class.
        /// </summary>
        /// <param name="definition">The thing definition.</param>
        public Unit(BaseThingDefinition definition)
            : base(definition)
        {
        }

        /// <inheritdoc/>
        public override ThingType ThingType => ThingType.Unit;
    }
}
