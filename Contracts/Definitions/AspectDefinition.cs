// <copyright file="AspectDefinition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions
{
    using LegendsGenerator.Contracts.Compiler;
    using LegendsGenerator.Contracts.Definitions.Validation;
    using LegendsGenerator.Contracts.Things;
    using System;

    /// <summary>
    /// The definition of an Aspect.
    /// </summary>
    public partial class AspectDefinition : BaseDefinition
    {
        /// <summary>
        /// Gets or sets the base value.
        /// </summary>
        [Compiled(typeof(string), Protected = true)]
        [HideInEditor("value.Dynamic")]
        public string Value { get; set; } = UnsetString;

        /// <summary>
        /// Gets or sets the base value.
        /// </summary>
        [Compiled(typeof(string), Protected = true)]
        [CompiledVariable("Subject", typeof(BaseDefinition))]
        [HideInEditor("!value.Dynamic")]
        public string DynamicValue { get; set; } = UnsetString;

        /// <summary>
        /// Gets or sets a value indicating whether the value is dynamic.
        /// </summary>
        public bool Dynamic { get; set; }

        /// <summary>
        /// Evaluates the expressions in the Value property with the given parameters.
        /// </summary>
        /// <param name="rdm">The random number generator</param>
        /// <returns>The result of evaluation.</returns>
        public string EvalValueSafe(Random rdm)
        {
            if (this.Dynamic)
            {
                throw new InvalidOperationException("Can only call EvalValue if not Dynamic.");
            }

            return this.EvalValue(rdm);
        }

        /// <summary>
        /// Evaluates the expressions in the Value property with the given parameters.
        /// </summary>
        /// <param name="rdm">The random number generator</param>
        /// <param name="thing">The thing.</param>
        /// <returns>The result of evaluation.</returns>
        public string EvalDynamicValueSafe(Random rdm, BaseThing thing)
        {
            if (!this.Dynamic)
            {
                throw new InvalidOperationException("Can only call EvalDynamicValue if Dynamic.");
            }

            return this.EvalDynamicValue(rdm, thing);
        }
    }
}
