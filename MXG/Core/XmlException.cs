/*
 * MXG - Minimalist XML Generator
 * Copyright Raphael Stoeckli © 2019
 * This library is licensed under the MIT License.
 * You find a copy of the license in project folder or on: http://opensource.org/licenses/MIT
 */

using System;

namespace MXG.Core
{
    /// <summary>
    /// General MXG exception
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class XmlException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlException"/> class.
        /// </summary>
        public XmlException()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public XmlException(string message) : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public XmlException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}
