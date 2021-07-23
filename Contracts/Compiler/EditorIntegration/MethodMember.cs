// -------------------------------------------------------------------------------------------------
// <copyright file="MethodMember.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Contracts.Compiler.EditorIntegration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// A method member of a class.
    /// </summary>
    public class MethodMember : BaseTypeMember
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MethodMember"/> class.
        /// </summary>
        /// <param name="name">The name of the method.</param>
        /// <param name="returnType">The return type.</param>
        /// <param name="parameters">The method parameters.</param>
        public MethodMember(string name, Type returnType, IList<MethodParameter> parameters)
            : base(name, returnType)
        {
            this.Parameters = parameters;
        }

        /// <summary>
        /// Gets the parameters on the method.
        /// </summary>
        public IList<MethodParameter> Parameters { get; }

        /// <inheritdoc/>
        public override bool RequiresParens => true;
    }
}
