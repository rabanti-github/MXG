/*
 * MXG - Minimalist XML Generator
 * Copyright Raphael Stoeckli © 2019
 * This library is licensed under the MIT License.
 * You find a copy of the license in project folder or on: http://opensource.org/licenses/MIT
 */

namespace MXG.Core
{
    /// <summary>
    /// /Abstract class representing an XML document of any kind
    /// </summary>
    public abstract class AbstractXmlDocument
    {
        /// <summary>
        /// Abstract method to get the string of the entire XML document
        /// </summary>
        /// <returns>Non-beautified XML string</returns>
        public abstract string GetXmlString();

    }
}