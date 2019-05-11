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
    /// Class represents an MSXML relationship document (.rels)
    /// </summary>
    public class XmlRelationships: AbstractXmlDocument
    {
        private XmlElement relationships;

        /// <summary>
        /// Gets the relationship elements
        /// </summary>
        /// <value>
        /// Relationship document which contains the element triples (id, type and target)
        /// </value>
        public XmlDocument RelationshipDocument { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlRelationships"/> class
        /// </summary>
        /// <param name="xmlns">XML name space declaration (scheme) of the relationship document. Since mandatory, null will be transformed into an empty string</param>
        /// <param name="estimatedElementCount">The estimated element count of the relationship document</param>
        public XmlRelationships(string xmlns, int estimatedElementCount = 10)
        {
            this.RelationshipDocument = new XmlDocument(estimatedElementCount);
            this.relationships = XmlElement.CreateXmlElement(Constants.RELATIONSHIPS_NAME, true, null, false, estimatedElementCount, 1);
            this.relationships.AddAttribute(Constants.XMLNS_NAME, xmlns, true, false);
            this.RelationshipDocument.AddChild(this.relationships);
        }

        /// <summary>
        /// Adds a new relationship element
        /// </summary>
        /// <param name="id">Unique ID</param>
        /// <param name="type">Type, usually an URL to an XML scheme</param>
        /// <param name="target">Target path to the document which is identified by id and described by type</param>
        public void AddRelationship(string id, string type, string target)
        {
            XmlElement element = new XmlElement(Constants.RELATIONSHIP_NAME, null, null, 0, 3);
            element.AddAttribute(Constants.ID_NAME, id, true, false);
            element.AddAttribute(Constants.TYPE_NAME, type, true, false);
            element.AddAttribute(Constants.TARGET_NAME, target, true, false);
            this.relationships.AddChild(element);
        }

        /// <summary>
        /// Gets the string of the entire XML document
        /// </summary>
        /// <returns>Non-beautified XML string</returns>
        public override string GetXmlString()
        {
            return this.RelationshipDocument.GetXmlString();
        }
    }
}
