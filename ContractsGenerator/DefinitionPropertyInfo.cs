using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CompiledDefinitionSourceGenerator;

using Microsoft.CodeAnalysis;

namespace LegendsGenerator.ContractsGenerator
{
    /// <summary>
    /// Information about a property with a definition in it.
    /// </summary>
    public class DefinitionPropertyInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefinitionPropertyInfo"/> class.
        /// </summary>
        /// <param name="property">The property symbol to parse.</param>
        public DefinitionPropertyInfo(IPropertySymbol property)
        {
            this.Name = property.Name;

            if (property.Type.GetTypeArguments().Any())
            {
                this.UsesAdditionalParametersForHoldingClass = property
                    .Type
                    .GetTypeArguments()
                    .Any(t => t.HasAttribute("UsesAdditionalParametersForHoldingClassAttribute"));
            }
            else
            {
                this.UsesAdditionalParametersForHoldingClass = property
                    .Type
                    .HasAttribute("UsesAdditionalParametersForHoldingClassAttribute");
            }
        }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets or sets if the underlying type of this property needs upstream parameters.
        /// </summary>
        public bool UsesAdditionalParametersForHoldingClass { get; set; }
    }
}
