// -------------------------------------------------------------------------------------------------
// <copyright file="NotablePersonPres.cs" company="Tom Luppi">
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
    /// Presentation layer for a notable person.
    /// </summary>
    public class NotablePersonPres : BaseMovingThingPres
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotablePersonPres"/> class.
        /// </summary>
        /// <param name="inner">The inner thing.</param>
        /// <param name="world">The world.</param>
        public NotablePersonPres(NotablePerson inner, World world)
            : base(inner, world)
        {
        }

        /// <summary>
        /// Gets the site that this thing is in, if it's in a site.
        /// </summary>
        public SitePres? InSite =>
            this.Inner.InSiteId != null ? new SitePres((Site)this.World.FindThing(this.Inner.InSiteId.Value), this.World) : null;

        /// <inheritdoc/>
        protected override NotablePerson Inner => (NotablePerson)base.Inner;
    }
}
