// -------------------------------------------------------------------------------------------------
// <copyright file="AspectView.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Viewer.Views
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using LegendsGenerator.Contracts;
    using LegendsGenerator.Contracts.Things;

    /// <summary>
    /// The aspect view.
    /// </summary>
    public class AspectView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AspectView"/> class.
        /// </summary>
        /// <param name="thing">The thing to get the Aspect info for.</param>
        /// <param name="name">The name of the Aspect.</param>
        public AspectView(BaseThing thing, string name)
        {
            this.Title = $"{name} (Base {thing.BaseAspects[name]})";
            this.Effective = thing.EffectiveAspect(name, "None");

            var current = thing.GetCurrentAspectEffect(name);

            StringBuilder sb = new StringBuilder();
            foreach (AspectEffect effect in thing.GetAspectEffectsModifying(name))
            {
                EffectView view = new EffectView(effect);
                string str = $"{view.Title} {view.EffectString}";
                if (effect == current)
                {
                    str += $" (Current)";
                }

                sb.AppendLine(str);
            }

            this.Tooltip = sb.ToString();
        }

        /// <summary>
        /// Gets the title for the Aspect.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets the effective value.
        /// </summary>
        public string Effective { get; }

        /// <summary>
        /// Gets the tooltip.
        /// </summary>
        public string Tooltip { get; }
    }
}
