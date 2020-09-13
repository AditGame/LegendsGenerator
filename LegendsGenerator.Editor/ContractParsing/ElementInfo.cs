// -------------------------------------------------------------------------------------------------
// <copyright file="ElementInfo.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor.ContractParsing
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using CSScriptLib;
    using LegendsGenerator.Compiler.CSharp;
    using LegendsGenerator.Contracts.Compiler;

    /// <summary>
    /// Information about an element of a definition.
    /// </summary>
    public class ElementInfo
    {
        /// <summary>
        /// The delegate to decide if this should be hidden in the editor.
        /// </summary>
        private MethodDelegate<bool>? hiddenInEditorDelegate;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementInfo"/> class.
        /// </summary>
        /// <param name="name">The element name.</param>
        /// <param name="description">In informative description of the element.</param>
        /// <param name="propertyType">The type of the property.</param>
        /// <param name="nullable">If this element can be set to null.</param>
        /// <param name="getValue">A method which returns the value.</param>
        /// <param name="setValue">A method which sets the value.</param>
        /// <param name="getCompiledParameters">A method which gets the parameters to pass into a compiled method.</param>
        /// <param name="compiled">The compiled attribute, if it exists.</param>
        /// <param name="hiddenInEditorCondition">A string condition to eval if this should be shown in the editor.</param>
        public ElementInfo(
            string name,
            string description,
            Type propertyType,
            bool nullable,
            Func<PropertyNode, object?> getValue,
            Action<PropertyNode, object?> setValue,
            Func<PropertyNode, IList<string>>? getCompiledParameters,
            CompiledAttribute? compiled,
            (object OutterObject, string Condition)? hiddenInEditorCondition)
        {
            this.Name = name;
            this.Description = description;
            this.PropertyType = propertyType;
            this.Nullable = nullable;
            this.GetValue = getValue;
            this.SetValue = setValue;
            this.GetCompiledParameters = getCompiledParameters != null ? getCompiledParameters : prop => Array.Empty<string>();
            this.Compiled = compiled;

            if (hiddenInEditorCondition != null)
            {
                this.hiddenInEditorDelegate =
                    GenerateMethodDelegate(hiddenInEditorCondition.Value.OutterObject.GetType(), hiddenInEditorCondition.Value.Condition);
                this.HiddenInEditor = () => this.hiddenInEditorDelegate(hiddenInEditorCondition.Value.OutterObject);
            }
            else
            {
                this.HiddenInEditor = () => false;
            }
        }

        /// <summary>
        /// Gets or sets the name of the element.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a method which gets the value of this element.
        /// </summary>
        public Func<PropertyNode, object?> GetValue { get; set; }

        /// <summary>
        /// Gets or sets a method which sets the value of this element.
        /// </summary>
        public Action<PropertyNode, object?> SetValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this element can be set to null.
        /// </summary>
        public bool Nullable { get; set; }

        /// <summary>
        /// Gets or sets a description of the element.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the element type.
        /// </summary>
        public Type PropertyType { get; set; }

        /// <summary>
        /// Gets or sets a method which gets the compiled parameters of this element.
        /// </summary>
        public Func<PropertyNode, IList<string>> GetCompiledParameters { get; set; }

        /// <summary>
        /// Gets or sets the compiled attribute.
        /// </summary>
        public CompiledAttribute? Compiled { get; set; }

        /// <summary>
        /// Gets or sets a method delegate which controls if this is visible in the editor.
        /// </summary>
        public Func<bool> HiddenInEditor { get; set; }

        /// <summary>
        /// Gets or sets an optional method which changes the name of the node.
        /// </summary>
        public Action<PropertyNode, string>? ChangeName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Name of this element creates a variable.
        /// </summary>
        public bool NameCreatesVariableName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Contents of this element represents an existing variable.
        /// </summary>
        public bool ContentsConsumeVariableName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a change to the contents of this property causes the definition name to change.
        /// </summary>
        public bool ControlsDefinitionName { get; set; }

        /// <summary>
        /// Gets a method delegate for the specified condition.
        /// </summary>
        /// <param name="type">The type of the input.</param>
        /// <param name="condition">The condition to eval.</param>
        /// <returns>The method delegate.</returns>
        private static MethodDelegate<bool> GenerateMethodDelegate(Type type, string condition)
        {
            StringBuilder bldr = new StringBuilder();
            bldr.AppendLine(GenerateMethodSignature<bool>(new Dictionary<string, Type>() { { "value", type }, }));
            bldr.AppendLine($"return {condition};");
            bldr.AppendLine("}");

            return CSScript.Evaluator.CreateDelegate<bool>(bldr.ToString());
        }

        /// <summary>
        /// Generates the method signature with the specified variables.
        /// </summary>
        /// <typeparam name="T">The return type.</typeparam>
        /// <param name="variables">The variable names.</param>
        /// <returns>The generated method signature.</returns>
        private static string GenerateMethodSignature<T>(IDictionary<string, Type> variables)
        {
            StringBuilder bldr = new StringBuilder();
            bldr.AppendLine("using LegendsGenerator.Contracts;");
            bldr.AppendLine("using LegendsGenerator.Contracts.Definitions;");
            bldr.AppendLine("using LegendsGenerator.Contracts.Definitions.Events;");
            bldr.AppendLine("using LegendsGenerator.Contracts.Things;");
            bldr.Append($"{typeof(T).Name} EvaluateCondition(");
            bldr.AppendJoin(", ", variables.Select(v => $"{v.Value.FullName} {v.Key}"));
            bldr.Append(") {");

            return bldr.ToString();
        }
    }
}
