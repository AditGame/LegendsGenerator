// <copyright file="NotablePerson.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Things
{
    using System;
    using LegendsGenerator.Contracts.Definitions;
    using LegendsGenerator.Contracts.Definitions.Events;

    /// <summary>
    /// Represents a notable person.
    /// </summary>
    public record NotablePerson : BaseMovingThing, ICanEnterSites
    {
        public NotablePerson(NotablePersonDefinition definition)
            : base(definition)
        {
        }

        /// <summary>
        /// Gets the name of this Thing Type.
        /// </summary>
        public override ThingType ThingType => ThingType.NotablePerson;

        /// <summary>
        /// Gets or sets the thing this notable person is in.
        /// </summary>
        public Guid? InSiteId { get; set; }

        /// <summary>
        /// Gets or sets the thing this thing leads.
        /// </summary>
        public Guid? LeadsId { get; set; }
    }
}
