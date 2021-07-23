// <copyright file="NotablePersonDefinition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions
{
    using System;
    using LegendsGenerator.Contracts.Definitions.Events;

    /// <summary>
    /// Definition of a notable person type.
    /// </summary>
    public partial class NotablePersonDefinition : BaseMovingThingDefinition
    {
        /// <inheritdoc/>
        public override ThingType ThingType => ThingType.NotablePerson;
    }
}
