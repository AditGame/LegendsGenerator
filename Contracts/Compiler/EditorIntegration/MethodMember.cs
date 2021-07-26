// -------------------------------------------------------------------------------------------------
// <copyright file="MethodMember.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Contracts.Compiler.EditorIntegration
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// A method member of a class.
    /// </summary>
    public class MethodMember : BaseTypeMember
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MethodMember"/> class.
        /// </summary>
        /// <param name="method">The method.</param>
        public MethodMember(MethodInfo method)
            : base(
                  ToStringWithGenerics(method),
                  method.ReturnType)
        {
            this.Parameters = method.GetParameters().Select(p => new MethodParameter(p.Name!, p.ParameterType)).ToList();
        }

        /// <summary>
        /// Gets the parameters on the method.
        /// </summary>
        public IList<MethodParameter> Parameters { get; }

        /// <inheritdoc/>
        public override bool RequiresParens => true;

        /// <summary>
        /// Converts a method into it's name with generics.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <returns>The string form.</returns>
        private static string ToStringWithGenerics(MethodInfo method)
        {
            string name = method.Name.Split("`", 2).First();

            if (method.ContainsGenericParameters)
            {
                name += "<";

                name += string.Join(", ", method.GetGenericArguments().Select(x => x.Name));

                name += ">";
            }

            return name;
        }
    }
}
