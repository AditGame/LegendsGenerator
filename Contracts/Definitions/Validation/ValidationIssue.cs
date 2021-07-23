// -------------------------------------------------------------------------------------------------
// <copyright file="ValidationIssue.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Contracts.Definitions.Validation
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents an issue with validation.
    /// </summary>
    public class ValidationIssue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationIssue"/> class.
        /// </summary>
        /// <param name="level">The level of issue (error, warning, etc).</param>
        /// <param name="description">A description of the issue.</param>
        /// <param name="stack">The stack of definitions to the issue.</param>
        public ValidationIssue(
            ValidationLevel level,
            string description,
            params string[] stack)
        {
            this.Level = level;
            this.Description = description;
            this.Stack = stack;
        }

        /// <summary>
        /// Gets the level of the issue.
        /// </summary>
        public ValidationLevel Level { get; }

        /// <summary>
        /// Gets the description of the level.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets the stacktrace in Most-General to Most-Specific order.
        /// </summary>
        public IReadOnlyList<string> Stack { get; }

        /// <summary>
        /// Gets a nice display string of the validaiton failure.
        /// </summary>
        public string DisplayString => $"{this.Level} {string.Join(".", this.Stack)}: {this.Description}";

        /// <summary>
        /// Clones this validation issue with additional stack.
        /// </summary>
        /// <param name="additionalStack">The additional, more general stack members.</param>
        /// <returns>Validation issues.</returns>
        public ValidationIssue Clone(params string[] additionalStack)
        {
            List<string> newStack = new List<string>();
            newStack.AddRange(additionalStack);
            newStack.AddRange(this.Stack);

            return new ValidationIssue(
                this.Level,
                this.Description,
                newStack.ToArray());
        }
    }
}
