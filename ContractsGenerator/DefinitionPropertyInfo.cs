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

        public string Name { get; }

        public bool UsesAdditionalParametersForHoldingClass { get; set; }
    }
}
