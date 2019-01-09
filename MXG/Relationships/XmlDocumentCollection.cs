using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXG.Relationships
{
    public class XmlDocumentCollection
    {
        public XmlContenTypes ContenTypes { get; private set; }
        public List<XmlRelationships> Relationships { get; set; }

        public XmlDocumentCollection(string contentTypeXmlns, int estimatedContentTypeCount = 10, int estimatedRelationshipCount = 10)
        {
            this.ContenTypes = new XmlContenTypes(contentTypeXmlns, estimatedContentTypeCount);
            this.Relationships = new List<XmlRelationships>(estimatedRelationshipCount);
        }


    }
}
