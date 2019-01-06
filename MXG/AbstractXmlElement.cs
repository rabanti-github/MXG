/*
 * MXG - Minimalist XML Generator
 * Copyright Raphael Stoeckli © 2019
 * This library is licensed under the MIT License.
 * You find a copy of the license in project folder or on: http://opensource.org/licenses/MIT
 */

using System.Text;

namespace MXG
{
    public abstract class AbstractXmlElement
    {
        private string value = null;

        /// <summary>
        /// Gets the name of the element
        /// </summary>
        /// <value>
        /// Element name
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets the value of the element
        /// </summary>
        /// <value>
        /// Element value (string content)
        /// </value>
        public string Value
        {
            get { return this.value; }
            set
            {
                this.value = value;
            }
        }

        /// <summary>
        /// Gets whether the element is empty
        /// </summary>
        /// <value>
        ///   True, if the element has no inner string content (empty), otherwise false
        /// </value>
        public bool IsEmpty { get { return this.value == null ? true : false; } }

        /// <summary>
        /// Base constructor with arguments
        /// </summary>
        /// <param name="name">Element name</param>
        /// <param name="value">Element value. The value can be null (default)</param>
        /// <exception cref="XmlException">An XML element must contain a valid, non-empty name</exception>
        public AbstractXmlElement(string name, string value = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new XmlException("An XML element must contain a valid, non-empty name");
            }
            this.Name = name;
            this.Value = value;
        }

        /// <summary>
        /// Abstract method to append the current element with all sub-elements to the passed string builder
        /// </summary>
        /// <param name="builder">Document string builder</param>
        public abstract void AppendXmlString(StringBuilder builder);

        /// <summary>
        /// Abstract method to set the content of the element
        /// </summary>
        /// <param name="content">Content as string</param>
        /// <param name="escapeContent">If true, the element will be checked and escaped according to the XML specifications</param>
        public abstract void SetContent(string content, bool escapeContent = true);
    }
}
