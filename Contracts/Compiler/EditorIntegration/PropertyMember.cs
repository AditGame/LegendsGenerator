// -------------------------------------------------------------------------------------------------
// <copyright file="PropertyMember.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Contracts.Compiler.EditorIntegration
{
    using System;

    /// <summary>
    /// A preoprty on a class.
    /// </summary>
    public class PropertyMember : BaseTypeMember
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMember"/> class.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="returnType">The return type.</param>
        public PropertyMember(string name, Type returnType)
            : base(name, returnType)
        {
        }

        /// <inheritdoc/>
        public override bool RequiresParens => false;
    }
}
