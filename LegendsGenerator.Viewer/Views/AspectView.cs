// -------------------------------------------------------------------------------------------------
// <copyright file="AspectView.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Viewer.Views
{
    using System.Collections.Generic;
    using System.Text;
    using LegendsGenerator.Contracts;
    using LegendsGenerator.Contracts.Definitions;
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
            this.Title = name;
            this.Effective = thing.EffectiveAspect(name, "None");

            if (thing.BaseAspects.TryGetValue(name, out string? @base) && !string.IsNullOrEmpty(@base))
            {
                this.Base = @base;
            }

            if (thing.DynamicAspects.TryGetValue(name, out string? dynamic) && !string.IsNullOrEmpty(dynamic))
            {
                this.Dynamic = dynamic;
            }

            if (thing.BaseDefinition.Aspects.TryGetValue(name, out AspectDefinition? definition) && definition.Dynamic)
            {
                this.DynamicScript = definition.DynamicValue;
            }

            var current = thing.GetCurrentAspectEffect(name);
            if (current != null)
            {
                this.CurrentEffect = current.Value;
            }

            StringBuilder sb = new StringBuilder();
            if (this.DynamicScript != null)
            {
                sb.AppendLine($"Dynamic Script: {this.DynamicScript}");
            }

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
        /// Gets the base value.
        /// </summary>
        public string? Base { get; }

        /// <summary>
        /// Gets the dynamic value.
        /// </summary>
        public string? Dynamic { get; }

        /// <summary>
        /// Gets the script called for dynamic computation.
        /// </summary>
        public string? DynamicScript { get; }

        /// <summary>
        /// Gets the value of the current effect, if there is one.
        /// </summary>
        public string? CurrentEffect { get; }

        /// <summary>
        /// Gets the value provided by the effects on this attribute.
        /// </summary>
        public string Effective { get; }

        /// <summary>
        /// Gets the tooltip.
        /// </summary>
        public string Tooltip { get; }

        /// <summary>
        /// Gets the value which details the value of this thing.
        /// </summary>
        public string ValueStatement
        {
            get
            {
                List<string> bts = new List<string>();
                if (this.Base != null)
                {
                    bts.Add($"{this.Base} (Base)");
                }

                if (this.Dynamic != null)
                {
                    bts.Add($"{this.Dynamic} (Dynamic)");
                }

                if (this.CurrentEffect != null)
                {
                    bts.Add($"{this.CurrentEffect} (Effects)");
                }

                return string.Join(" | ", bts);
            }
        }
    }
}
