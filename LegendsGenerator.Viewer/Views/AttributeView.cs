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
    using LegendsGenerator.Contracts.Definitions;
    using LegendsGenerator.Contracts.Things;

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
            this.Title = name;
            if (thing.BaseAttributes.TryGetValue(name, out int @base))
            {
                this.Base = @base;
            }

            if (thing.DynamicAttributes.TryGetValue(name, out int dynamic))
            {
                this.Dynamic = dynamic;
            }

            if (thing.BaseDefinition.Attributes.TryGetValue(name, out AttributeDefinition? def))
            {
                this.DynamicScript = def.DynamicValue;
            }

            this.Effective = thing.EffectiveAttribute(name);

            StringBuilder sb = new StringBuilder();
            if (this.DynamicScript != null)
            {
                sb.AppendLine($"Dynamic Script: {this.DynamicScript}");
            }

            foreach (AttributeEffect effect in thing.GetAttributeEffectsModifying(name))
            {
                this.EffectValue += effect.Manitude;
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
        /// Gets the base value.
        /// </summary>
        public int Base { get; }

        /// <summary>
        /// Gets the dynamic value.
        /// </summary>
        public int Dynamic { get; }

        /// <summary>
        /// Gets the script called for dynamic computation.
        /// </summary>
        public string? DynamicScript { get; }

        /// <summary>
        /// Gets the value provided by the effects on this attribute.
        /// </summary>
        public int EffectValue { get; }

        /// <summary>
        /// Gets the value which details the value of this thing.
        /// </summary>
        public string ValueStatement
        {
            get
            {
                List<string> bts = new List<string>();
                if (this.Base != 0)
                {
                    bts.Add($"{this.Base} (Base)");
                }

                if (this.Dynamic != 0)
                {
                    bts.Add($"{this.Dynamic} (Dynamic)");
                }

                if (this.EffectValue != 0)
                {
                    bts.Add($"{this.EffectValue} (Effects)");
                }

                return string.Join(" + ", bts);
            }
        }

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
