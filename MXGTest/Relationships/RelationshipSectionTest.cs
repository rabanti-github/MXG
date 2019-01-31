using MXG.Relationships;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MXG.Relationships.XmlDocumentCollection;

namespace MXGTest.Relationships
{
    [TestFixture, Description("Test class for RelationshipSection class")]
    public class RelationshipSectionTest
    {
        [Description("Test the constructor of the RelationshipSection class with the available properties")]
        [TestCase("/path", "ns1", "/path", "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><Relationships xmlns=\"ns1\"/>")]
        [TestCase("/path/subPath/", "ns2", "/path/subPath/", "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><Relationships xmlns=\"ns2\"/>")]
        [TestCase("", "ns3", "", "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><Relationships xmlns=\"ns3\"/>")]
        public void ConstructorTest(string path, string xmlns, string expectedPath, string expectedRelationship)
        {
            XmlContentTypes reference = new XmlContentTypes("contentTypeNS");
            RelationshipSection section = new RelationshipSection(path, xmlns, reference);
            Assert.Multiple(() => { 
                Assert.IsNotNull(section.Documents);
                Assert.AreEqual(section.Documents.Count, 0);
                Assert.AreEqual(section.Relationships.GetXmlString(), expectedRelationship);
                Assert.AreEqual(section.Path, expectedPath);
            });
        }


        
    }
}
