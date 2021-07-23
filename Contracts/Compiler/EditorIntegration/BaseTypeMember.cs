// -------------------------------------------------------------------------------------------------
// <copyright file="BaseTypeMember.cs" company="Tom Luppi">
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
    /// Members on a call.
    /// </summary>
    public abstract class BaseTypeMember
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseTypeMember"/> class.
        /// </summary>
        /// <param name="name">The name of the member.</param>
        /// <param name="returnType">The return type.</param>
        protected BaseTypeMember(string name, Type returnType)
        {
            this.Name = name;
            this.ReturnType = returnType;
        }

        /// <summary>
        /// Gets or sets the name of the member.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the return type.
        /// </summary>
        public Type ReturnType { get; }

        /// <summary>
        /// Gets a value indicating whether this member uses paremethis.
        /// </summary>
        public abstract bool RequiresParens { get; }
    }
}
