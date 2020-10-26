// <copyright file="AttributeDefinition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions
{
    using LegendsGenerator.Contracts.Compiler;

    /// <summary>
    /// The definition of an attribute.
    /// </summary>
    public partial class AttributeDefinition : BaseDefinition
    {
        /// <summary>
        /// Gets the base value.
        /// </summary>
        [Compiled(typeof(int))]
        public string BaseValue { get; set; } = "0";

        /// <summary>
        /// Gets the dynamic value.
        /// </summary>
        [Compiled(typeof(int))]
        [CompiledVariable("Subject", typeof(BaseDefinition))]
        public string DynamicValue { get; set; } = "0";
    }
}
