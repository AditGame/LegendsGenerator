// -------------------------------------------------------------------------------------------------
// <copyright file="DefinitionPropertyInfo.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.ContractsGenerator
{
    using System.Linq;

    using CompiledDefinitionSourceGenerator;

    using Microsoft.CodeAnalysis;

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
        /// Gets or sets a value indicating whether the underlying type of this property needs upstream parameters.
        /// </summary>
        public bool UsesAdditionalParametersForHoldingClass { get; set; }
    }
}
