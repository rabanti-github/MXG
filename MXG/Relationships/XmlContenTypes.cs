/*
 * MXG - Minimalist XML Generator
 * Copyright Raphael Stoeckli © 2019
 * This library is licensed under the MIT License.
 * You find a copy of the license in project folder or on: http://opensource.org/licenses/MIT
 */

using MXG.Core;

namespace MXG.Relationships
{
    /// <summary>
    /// Class represents an MSXML content type document ([Content_Types].xls)
    /// </summary>
    public class XmlContenTypes : AbstractXmlDocument
    {
        private const string XMLNS_NAME = "xmlns";
        private const string TYPES_NAME = "Types";
        private const string DEFAULT_NAME = "Default";
        private const string OVERRIDE_NAME = "Override";
        private const string CONTENT_TYPE_NAME = "ContentType";
        private const string EXTENSION_NAME = "Extension";
        private XmlElement contentTypes;

        /// <summary>
        /// Gets the content type elements
        /// </summary>
        /// <value>
        /// Content type document which contains the element tuples (ContentType and Extension) or (Override ContentType and PartName)
        /// </value>
        public XmlDocument ContenTypeDocument { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlContenTypes"/> class
        /// </summary>
        /// <param name="xmlns">XML name space declaration (scheme) of the content type document</param>
        /// <param name="estimatedElementCount">The estimated element count of the relationship document</param>
        public XmlContenTypes(string xmlns, int estimatedElementCount = 10)
        {
            this.ContenTypeDocument = new XmlDocument(estimatedElementCount);
            this.contentTypes = XmlElement.CreateXmlElement(TYPES_NAME, true, null, false, estimatedElementCount, 1);
            this.contentTypes.AddAttribute(XMLNS_NAME, xmlns, true, false);
            this.ContenTypeDocument.AddChild(this.contentTypes);
        }

        /// <summary>
        /// Adds a new default content type element
        /// </summary>
        /// <param name="contentType">Content type</param>
        /// <param name="extension">Extension of the content type</param>
        public void AddDefaultContentType(string contentType, string extension)
        {
            XmlElement element = new XmlElement(DEFAULT_NAME, null, null, 0, 2);
            element.AddAttribute(CONTENT_TYPE_NAME, contentType, true, false);
            element.AddAttribute(EXTENSION_NAME, extension, true, false);
            this.contentTypes.AddChild(element);
        }

        /// <summary>
        /// Adds a new override content type element
        /// </summary>
        /// <param name="contentType">Content type</param>
        /// <param name="extension">Extension of the content type</param>
        public void AddOverrideContentType(string contentType, string partName)
        {
            XmlElement element = new XmlElement(OVERRIDE_NAME, null, null, 0, 2);
            element.AddAttribute(CONTENT_TYPE_NAME, contentType, true, false);
            element.AddAttribute(EXTENSION_NAME, partName, true, false);
            this.contentTypes.AddChild(element);
        }

        /// <summary>
        /// Gets the string of the entire XML document
        /// </summary>
        /// <returns>Non-beautified XML string</returns>
        public override string GetXmlString()
        {
            return this.ContenTypeDocument.GetXmlString();
        }
    }
}
