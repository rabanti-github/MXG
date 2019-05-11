using MXG.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXG.Relationships
{
    /// <summary>
    /// Class represents an MSXML document collection
    /// </summary>
    public class XmlDocumentCollection
    {
        /// <summary>
        /// Content types of the document collection
        /// </summary>
        public XmlContentTypes ContentTypes { get; private set; }
        /// <summary>
        /// Relationship sections of the document collections
        /// </summary>
        public List<RelationshipSection> RelationshipSections { get; private set; }

        /// <summary>
        /// XML name space of the relationships in the document collection 
        /// </summary>
        public string RelationshipXmlns { get; private set; }

        /// <summary>
        /// Constructor with arguments
        /// </summary>
        /// <param name="contentTypeXmlns">XML name space of the content types</param>
        /// <param name="relationshipXmlns">XML name space of the relationships</param>
        /// <param name="estimatedContentTypeCount">Estimated number of content type entries. Default is 10. This number is a performance measure</param>
        /// <param name="estimatedSectionCount">Estimated number of section entries. Default is 10. This number is a performance measure</param>
        public XmlDocumentCollection(string contentTypeXmlns, string  relationshipXmlns, int estimatedContentTypeCount = 10, int estimatedSectionCount = 10)
        {
            this.RelationshipXmlns = relationshipXmlns;
            this.ContentTypes = new XmlContentTypes(contentTypeXmlns, estimatedContentTypeCount);
            if (estimatedSectionCount < 0)
            {
                estimatedSectionCount = 0;
            }
            this.RelationshipSections = new List<RelationshipSection>(estimatedSectionCount);
        }

        /// <summary>
        /// Adds a new section to the document collection
        /// </summary>
        /// <param name="path">Path of the corresponding XML document described by the section</param>
        /// <param name="estimatedDocumentCount">Estimated number of content type entries. Default is 10. This number is a performance measure</param>
        /// <returns>RelationshipSection object</returns>
        public RelationshipSection AddSection(string path, int estimatedDocumentCount = 10)
        {
            RelationshipSection section = new RelationshipSection(path, this.RelationshipXmlns, this.ContentTypes, estimatedDocumentCount);
            this.RelationshipSections.Add(section);
            return section;
        }

        /// <summary>
        /// Sub-class represents a section of an MSXML relationship (internal use of MSXML)
        /// </summary>
        public class RelationshipSection
        {
            private XmlContentTypes contentTypesReference;

            /// <summary>
            /// Relationships object of the section
            /// </summary>
            public XmlRelationships Relationships { get; private set; }
            /// <summary>
            /// Path of the corresponding XML document described by the section
            /// </summary>
            public string Path { get; private set; }
            /// <summary>
            /// Attached XML documents of the section
            /// </summary>
            public List<XmlDocument> Documents { get; private set; }

            /// <summary>
            /// Constructor with parameters
            /// </summary>
            /// <param name="path">Path of the corresponding XML document described by the section</param>
            /// <param name="xmlns">XML name space of the relationship</param>
            /// <param name="contentTypesReference">Content type reference of the section</param>
            /// <param name="estimatedDocumentCount">Estimated number of relationship entries. Default is 10. This number is a performance measure</param>
            public RelationshipSection(string path, string xmlns, XmlContentTypes contentTypesReference, int estimatedDocumentCount = 10)
            {
                if (estimatedDocumentCount < 0)
                {
                    estimatedDocumentCount = 0;
                }
                this.Relationships = new XmlRelationships(xmlns, estimatedDocumentCount);
                this.Documents = new List<XmlDocument>(estimatedDocumentCount);
                this.Path = path;
                this.contentTypesReference = contentTypesReference;
            }

            /// <summary>
            /// Creates an XML document, adds it to the section and returns the document
            /// </summary>
            /// <param name="id">Internal ID of the document</param>
            /// <param name="type">Document type (usually described by an URL)</param>
            /// <param name="targetPath">Relative path to the XML document</param>
            /// <param name="contentType">Content type of the XML document (usually described by an URL)</param>
            /// <param name="basePath">Relative base path of all XML documents</param>
            /// <param name="estimatedElementCount">Estimated number of XML elements within the document. Default is 10. This number is a performance measure</param>
            /// <param name="encoding">Encoding as String. Default is UTF-8</param>
            /// <param name="standalone">If true, the XML document is declared as standalone</param>
            /// <returns>XML document based on the parameters (empty object to be processed)</returns>
            public XmlDocument AddDocument(string id, string type, string targetPath, string contentType, string basePath, int estimatedElementCount = 10, string encoding = Constants.UTF8_NAME, bool? standalone = true)
            {
                this.contentTypesReference.AddOverrideContentType(contentType, basePath + targetPath);
                XmlDocument document = new XmlDocument(estimatedElementCount, encoding, standalone);
                this.Relationships.AddRelationship(id, type, targetPath);
                this.Documents.Add(document);
                return document;
            }
        }
    }
}
