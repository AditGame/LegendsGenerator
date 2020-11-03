// <copyright file="UnitDefinition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions
{
    using LegendsGenerator.Contracts.Definitions.Events;

    /// <summary>
    /// The definition of a Unit.
    /// </summary>
    public partial class UnitDefinition : BaseThingDefinition
    {
        /// <inheritdoc/>
        public override ThingType ThingType => ThingType.Unit;
    }
}
