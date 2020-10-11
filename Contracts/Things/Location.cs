// <copyright file="Location.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Things
{
    public record Location
    {
        public int X { get; set; }

        public int Y { get; set; }

        public override string ToString()
        {
            return $"{this.X},{this.Y}";
        }
    }
}
