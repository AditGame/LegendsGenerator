// <copyright file="TerrainSquare.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// The types of terrain.
    /// </summary>
    public enum TerrainType
    {
        Unknown,

        SaltWater,

        FreshWater,

        Plains,

        Desert,
    }

    /// <summary>
    /// All the info about a square of terrain.
    /// </summary>
    public record TerrainSquare
    {
        public TerrainType TerrainType { get; set; }
    }
}
