/*
 * MXG - Minimalist XML Generator
 * Copyright Raphael Stoeckli © 2019
 * This library is licensed under the MIT License.
 * You find a copy of the license in project folder or on: http://opensource.org/licenses/MIT
 */

using System.Collections.Generic;
using System.Text;

namespace MXG.Core
{
    public class XmlDocument : AbstractXmlDocument
    {

        private const string DEFAULT_XML_DECLARATION = "<?xml version=\"1.0\"";
        private const int DEFAULT_STRING_BUILDER_SIZE = 512;

        private List<XmlElement> children;
        private StringBuilder documentBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlDocument"/> class.
        /// </summary>
        /// <param name="estimatedElementCount">Estimated element count (for list initialization; default: 10)</param>
        /// <param name="encoding">Encoding of the document (default: UTF-8)</param>
        /// <param name="standalone">Standalone flag of the document (default: true)</param>
        public XmlDocument(int estimatedElementCount = 10, string encoding = "UTF-8", bool? standalone = true)
        {
            this.documentBuilder = new StringBuilder(DEFAULT_STRING_BUILDER_SIZE);
            this.children = new List<XmlElement>(estimatedElementCount);
            this.documentBuilder.Append(DEFAULT_XML_DECLARATION);
            if (!string.IsNullOrEmpty(encoding))
            {
                this.documentBuilder.Append(" encoding=\"").Append(encoding).Append("\"");
            }
            if (standalone.HasValue)
            {
                if (standalone.Value == true)
                {
                    this.documentBuilder.Append(" standalone=\"yes\"");
                }
                else
                {
                    this.documentBuilder.Append(" standalone =\"no\"");
                }
            }
            this.documentBuilder.Append("?>");
        }

        /// <summary>
        /// Method to add a child element to the document
        /// </summary>
        /// <param name="name">Name of the element</param>
        /// <param name="nameSpace">Name space of the element (optional; default: null)</param>
        /// <param name="skipNameCheck">If true, the name will not be validated (optional, default: true)</param>
        /// <param name="content">Inner string content of the element (optional, default: null)</param>
        /// <param name="escapeContent">If true, the content will be escaped (optional, default: false)</param>
        /// <returns>Added XML element</returns>
        public XmlElement AddChild(string name, string nameSpace, bool skipNameCheck = true, string content = null, bool escapeContent = false)
        {
            XmlElement element = XmlElement.CreateXmlElement(name, nameSpace, skipNameCheck, content, escapeContent);
            this.children.Add(element);
            return element;
        }

        /// <summary>
        /// Method to add a child element to the document
        /// </summary>
        /// <param name="name">Name of the element</param>
        /// <param name="nameSpace">Name space of the element (optional; default: null)</param>
        /// <param name="content">Inner string content of the element (optional, default: null)</param>
        /// <param name="escapeContent">If true, the content will be escaped (optional, default: false)</param>
        /// <returns>Added XML element</returns>
        public XmlElement AddChild(string name, bool skipNameCheck = true, string content = null, bool escapeContent = false)
        {
            XmlElement element = XmlElement.CreateXmlElement(name, null, skipNameCheck, content, escapeContent);
            this.children.Add(element);
            return element;
        }

        /// <summary>
        /// Method to add a child element to the document
        /// </summary>
        /// <param name="element">Prepared XmlElement object</param>
        public void AddChild(XmlElement element)
        {
            this.children.Add(element);
        }

        /// <summary>
        /// Gets the string of the entire XML document
        /// </summary>
        /// <returns>Non-beautified XML string</returns>
        public override string GetXmlString()
        {
            int len = this.children.Count;
            for(int i = 0; i < len; i++)
            {
                this.children[i].AppendXmlString(this.documentBuilder);
            }
            return this.documentBuilder.ToString();
        }

    }
}
