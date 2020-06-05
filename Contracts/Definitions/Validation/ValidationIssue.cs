using System.Collections.Generic;

namespace LegendsGenerator.Contracts.Definitions.Validation
{
    /// <summary>
    /// Represents an issue with validation.
    /// </summary>
    public class ValidationIssue
    {
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
        public string DisplayString => $"{this.Level} {string.Join(".", this.Stack)}: {Description}";

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
