// <copyright file="SupportsSubSites.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Gets the number of subsites a site supports.
    /// </summary>
    public class SupportsSubSites
    {
        /// <summary>
        /// Gets or sets the condition which decides if subsites are allowed at all.
        /// </summary>
        public string Condition { get; set; } = "false";

        /// <summary>
        /// Gets or sets the condition which returns the number of subsites this site supports.
        /// </summary>
        public string NumberOfSubSites { get; set; } = "1";
    }
}
