// -------------------------------------------------------------------------------------------------
// <copyright file="BaseTypeMember.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Contracts.Compiler.EditorIntegration
{
    using System;
    using System.Linq;

    /// <summary>
    /// Members on a call.
    /// </summary>
    public abstract class BaseTypeMember
    {
        /// <summary>
        /// The return type.
        /// </summary>
        private readonly Type returnType;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseTypeMember"/> class.
        /// </summary>
        /// <param name="name">The name of the member.</param>
        /// <param name="returnType">The return type.</param>
        protected BaseTypeMember(string name, Type returnType)
        {
            this.Name = name;
            this.returnType = returnType;
        }

        /// <summary>
        /// Gets or sets the name of the member.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the return type.
        /// </summary>
        public string ReturnType => ToStringWithGenerics(this.returnType);

        /// <summary>
        /// Gets a value indicating whether this member uses paremethis.
        /// </summary>
        public abstract bool RequiresParens { get; }

        /// <summary>
        /// Converts a type to a string with proper generic arguments.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The tostring.</returns>
        protected static string ToStringWithGenerics(Type type)
        {
            string name = type.Name.Split("`", 2).First();

            if (type.IsGenericType)
            {
                name += "<";

                name += string.Join(", ", type.GetGenericArguments().Select(x => ToStringWithGenerics(x)));

                name += ">";
            }

            return name;
        }
    }
}
