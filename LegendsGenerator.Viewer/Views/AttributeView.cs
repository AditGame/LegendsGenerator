// -------------------------------------------------------------------------------------------------
// <copyright file="AttributeView.cs" company="Tom Luppi">
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

    /// <summary>
    /// The attribute view.
    /// </summary>
    public class AttributeView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeView"/> class.
        /// </summary>
        /// <param name="thing">The thing to get the attribute info for.</param>
        /// <param name="name">The name of the attribute.</param>
        public AttributeView(BaseThing thing, string name)
        {
            this.Title = $"{name} (Base {thing.BaseAttributes[name]})";
            this.Effective = thing.EffectiveAttribute(name);

            StringBuilder sb = new StringBuilder();
            foreach (Effect effect in thing.GetEffectsModifying(name))
            {
                EffectView view = new EffectView(effect);
                sb.AppendLine($"{view.Title} {view.EffectString}");
            }

            this.Tooltip = sb.ToString();
        }

        /// <summary>
        /// Gets the title for the attribute.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets the effective value.
        /// </summary>
        public int Effective { get; }

        /// <summary>
        /// Gets the tooltip.
        /// </summary>
        public string Tooltip { get; }
    }
}
