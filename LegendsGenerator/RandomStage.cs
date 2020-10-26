// -------------------------------------------------------------------------------------------------
// <copyright file="RandomStage.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator
{
    /// <summary>
    /// The stage the random numbe generator is being used in.
    /// </summary>
    public enum RandomStage
    {
        /// <summary>
        /// An unknown stage.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The event finding stage.
        /// </summary>
        EventFinding = 1,

        /// <summary>
        /// The finalization stage.
        /// </summary>
        Finalization = 2,
    }
}
