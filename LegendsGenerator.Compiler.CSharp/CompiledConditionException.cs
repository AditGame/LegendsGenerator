// -------------------------------------------------------------------------------------------------
// <copyright file="CompiledConditionException.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Compiler.CSharp
{
    using System;
    using System.Runtime.Serialization;

    using LegendsGenerator.Contracts.Compiler;

    /// <summary>
    /// Exception from the compilation process.
    /// </summary>
    [Serializable]
    public class CompiledConditionException : Exception, ICompilerErrorException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompiledConditionException"/> class.
        /// </summary>
        public CompiledConditionException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompiledConditionException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public CompiledConditionException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompiledConditionException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public CompiledConditionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompiledConditionException"/> class.
        /// </summary>
        /// <param name="condition">The condition which failed.</param>
        /// <param name="error">The error text.</param>
        /// <param name="inner">The inner exception.</param>
        public CompiledConditionException(
            string condition,
            string error,
            Exception inner)
            : base($"Condition [{condition}] failed with [{error}]", inner)
        {
            this.Condition = condition;
            this.Error = error;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompiledConditionException"/> class.
        /// </summary>
        /// <param name="serializationInfo">The serialization info.</param>
        /// <param name="streamingContext">The streaming context.</param>
        protected CompiledConditionException(
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
