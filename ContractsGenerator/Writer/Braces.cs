// <copyright file="Braces.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.ContractsGenerator.Writer
{
    using System;

    /// <summary>
    /// Manages the braces/indentation of the class via a disposible class.
    /// </summary>
    public sealed class Braces : IDisposable
    {
        /// <summary>
        /// If disposed already, true.
        /// </summary>
        private bool disposedValue;

        /// <summary>
        /// The class writer.
        /// </summary>
        private ClassWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="Braces"/> class.
        /// </summary>
        /// <param name="writer">The class writer to manage.</param>
        public Braces(ClassWriter writer)
        {
            this.writer = writer;
            this.writer.StartBrace();
        }

        /// <summary>
        /// Disposes this class.
        /// </summary>
        /// <param name="disposing">True if disposing.</param>
        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            { 
                if (disposing)
                {
                    this.writer.EndBrace();
                }

                disposedValue = true;
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
