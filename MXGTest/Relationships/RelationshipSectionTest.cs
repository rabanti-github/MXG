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

        [Description("Test the AddDocument method")]
        [TestCase("ns1", "id1", "http://type1", "/targetPath", "content1", "UTF-8", true, "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>", "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><Relationships xmlns=\"ns1\"><Relationship Id=\"id1\" Type=\"http://type1\" Target=\"/targetPath\"/></Relationships>")]
        [TestCase("ns2", "id2", "http://type2", "/", "content2", "UTF-16", null, "<?xml version=\"1.0\" encoding=\"UTF-16\"?>", "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><Relationships xmlns=\"ns2\"><Relationship Id=\"id2\" Type=\"http://type2\" Target=\"/\"/></Relationships>")]
        [TestCase("ns3", "id3", "http://type3", "t2", "content3",  null, null, "<?xml version=\"1.0\"?>", "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><Relationships xmlns=\"ns3\"><Relationship Id=\"id3\" Type=\"http://type3\" Target=\"t2\"/></Relationships>")]
        public void AddDocumentTest(string xmlns, string id, string type, string targetPath, string contentType, string encoding, bool? standalone, string expectedDocumentString, string expectedRelationShipString)
        {
            XmlContentTypes reference = new XmlContentTypes("contentTypeNS");
            RelationshipSection section = new RelationshipSection("/path", xmlns, reference);
            section.AddDocument(id, type, targetPath, contentType, "/basePath", 10, encoding, standalone);
            Assert.Multiple(() =>
            {
                Assert.AreEqual(section.Documents.Count, 1);
                Assert.AreEqual(section.Documents[0].GetXmlString(), expectedDocumentString);
                Assert.AreEqual(section.Relationships.GetXmlString(), expectedRelationShipString);
            });
        }


        [Description("Test the AddDocument method with more than one element")]
        [TestCase("id1|id2", "t1|t2", "tp1|tp2", 2, "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><Relationships xmlns=\"ns1\"><Relationship Id=\"id1\" Type=\"t1\" Target=\"tp1\"/><Relationship Id=\"id2\" Type=\"t2\" Target=\"tp2\"/></Relationships>")]
        [TestCase("id1|id2|id3", "t1|t2|", "tp1|tp2|", 3, "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><Relationships xmlns=\"ns1\"><Relationship Id=\"id1\" Type=\"t1\" Target=\"tp1\"/><Relationship Id=\"id2\" Type=\"t2\" Target=\"tp2\"/><Relationship Id=\"id3\" Type=\"\" Target=\"\"/></Relationships>")]
        public void AddDocumentTest2(string idString, string typeString, string targetPathString, int expectedCount, string expectedRelationShipString)
        {
            XmlContentTypes reference = new XmlContentTypes("contentTypeNS");
            RelationshipSection section = new RelationshipSection("/path", "ns1", reference);
            string[] ids = TestUtils.SplitValues(idString, '|');
            string[] types = TestUtils.SplitValues(typeString, '|');
            string[] targetPaths = TestUtils.SplitValues(targetPathString, '|');
            for(int i = 0; i < expectedCount; i++)
            {
                section.AddDocument(ids[i], types[i], targetPaths[i], "contentType", "/basePath", 10, "UTF-8", true);
            }
            Assert.Multiple(() =>
            {
                Assert.AreEqual(section.Documents.Count, expectedCount);
                Assert.AreEqual(section.Relationships.GetXmlString(), expectedRelationShipString);
            });
        }


    }
}
