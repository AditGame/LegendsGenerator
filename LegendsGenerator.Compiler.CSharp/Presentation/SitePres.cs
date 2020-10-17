// -------------------------------------------------------------------------------------------------
// <copyright file="SitePres.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Compiler.CSharp.Presentation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using LegendsGenerator.Contracts;
    using LegendsGenerator.Contracts.Things;

    /// <summary>
    /// Presentation of a Site.
    /// </summary>
    public class SitePres : BaseThingPres
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SitePres"/> class.
        /// </summary>
        /// <param name="inner">The inner thing.</param>
        /// <param name="world">The world.</param>
        public SitePres(Site inner, World world)
            : base(inner, world)
        {
        }

        /// <inheritdoc/>
        protected override Site Inner => (Site)base.Inner;
    }
}
