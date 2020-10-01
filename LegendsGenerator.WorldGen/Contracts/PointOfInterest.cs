// -------------------------------------------------------------------------------------------------
// <copyright file="PointOfInterest.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.WorldGen.Contracts
{
    /// <summary>
    /// A point of interest on a single tile.
    /// </summary>
    public enum PointOfInterest
    {
        /// <summary>
        /// There is no point of interest in this tile.
        /// </summary>
        None,

        /// <summary>
        /// An open pit of magma which spews out smoke.
        /// </summary>
        Volcano,

        /// <summary>
        /// A small patch of water in an otherwise dry land.
        /// </summary>
        Oasis,

        /// <summary>
        /// A small patch of water.
        /// </summary>
        Pond,

        /// <summary>
        /// A large patch of water.
        /// </summary>
        Lake,
    }
}
