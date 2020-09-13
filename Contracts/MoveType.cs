// <copyright file="MoveType.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts
{
    using LegendsGenerator.Contracts.Definitions.Validation;

    /// <summary>
    /// The type of movement.
    /// </summary>
    public enum MoveType
    {
        /// <summary>
        /// The move type is unknown.
        /// </summary>
        [InvalidEnumValue]
        Unknown,

        /// <summary>
        /// Movement towards a thing.
        /// </summary>
        ToThing,

        /// <summary>
        /// Movement towards a specific coords.
        /// </summary>
        ToCoords,
    }
}
