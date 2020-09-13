// <copyright file="AffectsMovement.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>


namespace LegendsGenerator.Contracts.Definitions.Events
{
    using LegendsGenerator.Contracts.Definitions.Validation;

    /// <summary>
    /// The effect an event has on movement.
    /// </summary>
    public enum AffectsMovement
    {
        /// <summary>
        /// An unknown effect on movement.
        /// </summary>
        [InvalidEnumValue]
        Unknown,

        /// <summary>
        /// This event has no effect and movement will continue on.
        /// </summary>
        NoEffect,

        /// <summary>
        /// This event blocks movement for this step, but it will continue the next step.
        /// </summary>
        Pauses,

        /// <summary>
        /// The event ends movement altogether.
        /// </summary>
        Ends,
    }
}
