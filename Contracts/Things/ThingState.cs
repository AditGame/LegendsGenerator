// <copyright file="ThingState.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Things
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// THe state of a thing.
    /// </summary>
    public enum ThingState
    {
        /// <summary>
        /// An invalid state.
        /// </summary>
        Unknown,

        /// <summary>
        /// The Thing is in a write-only state and is being constructed.
        /// </summary>
        Constructing,

        /// <summary>
        /// The Thing has finished construction and dynamic values are being calculated.
        /// </summary>
        FinalizingConstruction,

        /// <summary>
        /// The Thing is finalized and ready for reading.
        /// </summary>
        Finalized,
    }
}
