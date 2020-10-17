// -------------------------------------------------------------------------------------------------
// <copyright file="BaseGlobalVariables.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Compiler.CSharp
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    using LegendsGenerator.Contracts;

    /// <summary>
    /// Base global variables to use for all global variables.
    /// </summary>
    public class BaseGlobalVariables
    {
        /// <summary>
        /// Gets or sets the world object.
        /// </summary>
        public World? World { get; set; }

        /// <summary>
        /// Gets the properties of this class.
        /// </summary>
        /// <returns>All non-null properties.</returns>
        internal Dictionary<string, object> ToDictionary()
        {
            Dictionary<string, object> props = new Dictionary<string, object>();
            foreach (PropertyInfo property in this.GetType().GetProperties())
            {
                object? val = property.GetValue(this);

                if (val != null)
                {
                    props.Add(property.Name, val);
                }
            }

            return props;
        }
    }
}
