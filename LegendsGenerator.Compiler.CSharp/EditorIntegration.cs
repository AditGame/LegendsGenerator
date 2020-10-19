// -------------------------------------------------------------------------------------------------
// <copyright file="EditorIntegration.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Compiler.CSharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LegendsGenerator.Compiler.CSharp.Presentation;
    using LegendsGenerator.Contracts.Compiler.EditorIntegration;

    /// <summary>
    /// Integration with the editor.
    /// </summary>
    internal class EditorIntegration : IEditorIntegration
    {
        /// <summary>
        /// Methods to not display.
        /// </summary>
        private IList<string> ignoredMethods = new List<string>()
        {
            "ToString",
            "GetHashCode",
            "GetType",
            "Equals",
            "<Clone>$",
        };

        /// <inheritdoc/>
        public IList<BaseTypeMember> GetPublicMembers(Type type)
        {
            // Convert normal types to presentation types.
            if (PresentationConverters.TryGetPresentationType(type, out Type? presType))
            {
                type = presType;
            }

            List<BaseTypeMember> options = new List<BaseTypeMember>();

            foreach (var property in type.GetProperties().OrderBy(x => x.Name))
            {
                options.Add(new PropertyMember(property.Name, property.PropertyType));
            }

            foreach (System.Reflection.MethodInfo method in type.GetMethods().OrderBy(x => x.Name).ThenBy(x => x.GetParameters().Length).Where(m => !m.IsSpecialName && !this.ignoredMethods.Contains(m.Name)))
            {
                options.Add(new MethodMember(method.Name, method.ReturnType, method.GetParameters().Select(p => new MethodParameter(p.Name!, p.ParameterType)).ToList()));
            }

            return options;
        }
    }
}
