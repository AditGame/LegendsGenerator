// <copyright file="ICanEnterSites.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Things
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface ICanEnterSites
    {
        /// <summary>
        /// Gets or sets the site this notable person is in.
        /// </summary>
        Guid? InSiteId { get; set; }

        /// <summary>
        /// Gets the site this thing is in.
        /// </summary>
        /// <param name="world">The world object.</param>
        /// <returns>The site if this is in something, null otherwise.</returns>
        public Site? GetInSite(World world)
        {
            if (this.InSiteId == null)
            {
                return null;
            }

            return world.FindThing(this.InSiteId.Value) as Site;
        }

        /// <summary>
        /// Gets the site this thing is in.
        /// </summary>
        /// <param name="site">The site. Null if no longer in a site.</param>
        public void SetInSite(Site? site)
        {
            this.InSiteId = site?.ThingId;
        }
    }
}
