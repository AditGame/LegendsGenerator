// <copyright file="NotablePerson.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Things
{
    using System;
    using LegendsGenerator.Contracts.Definitions.Events;

    /// <summary>
    /// Represents a notable person.
    /// </summary>
    public record NotablePerson : BaseMovingThing
    {
        /// <summary>
        /// Gets the name of this Thing Type.
        /// </summary>
        public override ThingType ThingType => ThingType.NotablePerson;
    }
}
