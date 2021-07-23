// -------------------------------------------------------------------------------------------------
// <copyright file="ConditionException.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Contracts.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Exception thrown by a condition.
    /// </summary>
    [Serializable]
    public class ConditionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionException"/> class.
        /// </summary>
        public ConditionException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ConditionException(string? message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public ConditionException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionException"/> class.
        /// </summary>
        /// <param name="defintion">The definition.</param>
        /// <param name="conditionName">The condition name.</param>
        public ConditionException(string defintion, string conditionName)
            : base($"Condition {defintion}::{conditionName} failed.")
        {
            this.DefinitionName = defintion;
            this.ConditionName = conditionName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionException"/> class.
        /// </summary>
        /// <param name="defintion">The definition name.</param>
        /// <param name="conditionName">The condition name.</param>
        /// <param name="inner">The inner exception.</param>
        public ConditionException(string defintion, string conditionName, Exception inner)
            : base($"Condition {defintion}::{conditionName} failed.", inner)
        {
            this.DefinitionName = defintion;
            this.ConditionName = conditionName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionException"/> class.
        /// </summary>
        /// <param name="defintion">The definition name.</param>
        /// <param name="conditionName">The condition name.</param>
        /// <param name="error">The error.</param>
        public ConditionException(string defintion, string conditionName, string error)
            : base($"Condition {defintion}::{conditionName} failed with {error}.")
        {
            this.DefinitionName = defintion;
            this.ConditionName = conditionName;
            this.Error = error;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionException"/> class.
        /// </summary>
        /// <param name="defintion">The definition name.</param>
        /// <param name="conditionName">The condition name.</param>
        /// <param name="error">The error.</param>
        /// <param name="inner">The inner exception.</param>
        public ConditionException(string defintion, string conditionName, string error, Exception inner)
            : base($"Condition {defintion}::{conditionName} failed with {error}.", inner)
        {
            this.DefinitionName = defintion;
            this.ConditionName = conditionName;
            this.Error = error;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionException"/> class.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaming context.</param>
        protected ConditionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.DefinitionName = info.GetString(nameof(this.DefinitionName));
            this.ConditionName = info.GetString(nameof(this.ConditionName));
            this.Error = info.GetString(nameof(this.Error));
        }

        /// <summary>
        /// Gets the definition which contains the condition which threw.
        /// </summary>
        public string? DefinitionName { get; }

        /// <summary>
        /// Gets the name of the condition which threw.
        /// </summary>
        public string? ConditionName { get; }

        /// <summary>
        /// Gets the error.
        /// </summary>
        public string? Error { get; }

        /// <inheritdoc/>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(this.DefinitionName), this.DefinitionName);
            info.AddValue(nameof(this.ConditionName), this.ConditionName);
            info.AddValue(nameof(this.Error), this.Error);
        }
    }
}
