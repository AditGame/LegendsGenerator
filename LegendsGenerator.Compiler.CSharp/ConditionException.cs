// -------------------------------------------------------------------------------------------------
// <copyright file="CompilerException.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Compiler.CSharp
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Exception from the compilation process.
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
        /// <param name="message">The exception message.</param>
        public ConditionException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public ConditionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionException"/> class.
        /// </summary>
        /// <param name="condition">The condition which failed.</param>
        /// <param name="error">The error text.</param>
        /// <param name="inner">The inner exception.</param>
        public ConditionException(
            string condition,
            string error,
            Exception inner)
            : base($"Condition [{condition}] failed with [{error}]", inner)
        {
            this.Condition = condition;
            this.Error = error;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionException"/> class.
        /// </summary>
        /// <param name="serializationInfo">The serialization info.</param>
        /// <param name="streamingContext">The streaming context.</param>
        protected ConditionException(
            SerializationInfo serializationInfo,
            StreamingContext streamingContext)
        {
            this.Condition = serializationInfo.GetString(nameof(this.Condition));
            this.Error = serializationInfo.GetString(nameof(this.Error));
        }

        /// <summary>
        /// Gets the condition text which caused the failure.
        /// </summary>
        public string? Condition { get; }

        /// <summary>
        /// Gets the text of the error.
        /// </summary>
        public string? Error { get; }

        /// <inheritdoc/>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(this.Condition), this.Condition);
            info.AddValue(nameof(this.Error), this.Error);
        }
    }
}
