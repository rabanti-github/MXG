using MXG.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXG.Relationships
{
    public class XmlDocumentCollection
    {
        public XmlContentTypes ContentTypes { get; private set; }
        public List<RelationshipSection> RelationshipSections { get; private set; }

        public string RelationshipXmlns { get; private set; }

        public XmlDocumentCollection(string contentTypeXmlns, string  relationshipXmlns, int estimatedContentTypeCount = 10, int estimatedSectionCount = 10)
        {
            this.RelationshipXmlns = relationshipXmlns;
            this.ContentTypes = new XmlContentTypes(contentTypeXmlns, estimatedContentTypeCount);
            this.RelationshipSections = new List<RelationshipSection>(estimatedSectionCount);
        }

        public RelationshipSection AddSection(string path, int estimatedDocumentCount = 10)
        {
            RelationshipSection section = new RelationshipSection(path, this.RelationshipXmlns, this.ContentTypes, estimatedDocumentCount);
            this.RelationshipSections.Add(section);
            return section;
        }


        public class RelationshipSection
        {
            private XmlContentTypes contentTypesReference;

            public XmlRelationships Relationships { get; private set; }
            public string Path { get; private set; }
            public List<XmlDocument> Documents { get; private set; }

            public RelationshipSection(string path, string xmlns, XmlContentTypes contentTypesReference, int estimatedDocumentCount = 10)
            {
                this.Relationships = new XmlRelationships(xmlns, estimatedDocumentCount);
                this.Documents = new List<XmlDocument>(estimatedDocumentCount);
                this.Path = path;
                this.contentTypesReference = contentTypesReference;
            }

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
