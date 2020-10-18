// <copyright file="NotablePersonDefinition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

using LegendsGenerator.Contracts.Definitions.Events;
using System;

namespace LegendsGenerator.Contracts.Definitions
{
    /// <summary>
    /// Definition of a notable person type.
    /// </summary>
    public partial class NotablePersonDefinition : BaseMovingThingDefinition
    {
        public override ThingType ThingType => ThingType.NotablePerson;
    }
}
