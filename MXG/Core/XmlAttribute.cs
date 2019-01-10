/*
 * MXG - Minimalist XML Generator
 * Copyright Raphael Stoeckli © 2019
 * This library is licensed under the MIT License.
 * You find a copy of the license in project folder or on: http://opensource.org/licenses/MIT
 */

using System.Text;

namespace MXG.Core
{
    public class XmlAttribute : AbstractXmlElement
    {


        /// <summary>
        /// Creates an XML attribute with a name and an optional value
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="value">Optional attribute value</param>
        /// <param name="skipNameCheck">If true, the validity check of the attribute name will be skipped</param>
        /// <param name="escapeValue">If true, the attribute value will be escaped</param>
        /// <returns>XML Attribute Object</returns>
        /// <exception cref="XmlException">The XML attribute '" + name + "' contains invalid characters or is empty</exception>
        public static XmlAttribute CreateXmlAttribute(string name, string value = null, bool skipNameCheck = true, bool escapeValue = true)
        {
            if (!skipNameCheck)
            {
                if (!Validator.ValidateAttributeName(name))
                {
                    throw new XmlException("The XML attribute '" + name + "' contains invalid characters or is empty");
                }
            }
            XmlAttribute attribute;
            if (escapeValue)
            {
                attribute = new XmlAttribute(name);
                attribute.SetContent(value);
            }
            else
            {
                attribute = new XmlAttribute(name, value);
                //attribute.UpdateCapacity();
            }
            return attribute;
        }

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="value">Attribute value (can be null)</param>
        /// <exception cref="XmlException">An XML attribute must contain a valid name</exception>
        public XmlAttribute(string name, string value = null) : base(name, value)
        { }

        /// <summary>
        /// Method to append the current attribute to the passed string builder
        /// </summary>
        /// <param name="builder">Document string builder</param>
        public override void AppendXmlString(StringBuilder builder)
        {
            if (this.IsEmpty)
            {
                builder.Append(' ');
            }
            else
            {
                builder.Append(this.Name).Append(Constants.EQUAL_TOKEN).Append(this.Value).Append(Constants.DELIMITER_TOKEN);
            }
        }

        /// <summary>
        /// Method to set the content of the attribute
        /// </summary>
        /// <param name="content">Content as string</param>
        /// <param name="escapeContent">If true, the attribute will be checked and escaped according to the XML specifications</param>
        public override void SetContent(string content, bool escapeContent = true)
        {
            if (escapeContent)
            {
                this.Value = Validator.EscapeXmlAttributeChars(content);
            }
            else
            {
                this.Value = content;
            }
        }
    }
}
