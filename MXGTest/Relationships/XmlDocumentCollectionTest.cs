using MXG.Relationships;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXGTest.Relationships
{
    [TestFixture, Description("Test class for XmlDocumentCollection class")]
    public class XmlDocumentCollectionTest
    {
        [Description("Test the constructor of the XmlDocumentCollection class with the available properties")]
        [TestCase("XNS1", "RNS1", 10, 10, false, "RNS1", "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><Types xmlns=\"XNS1\"/>")]
        [TestCase("", "", 0, 0, false, "", "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><Types xmlns=\"\"/>")]
        [TestCase(null, null, -10, -10, true, "", "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><Types xmlns/>")]
        [TestCase(null, "RNS2", 5, 5, false, "RNS2", "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><Types xmlns/>")]
        public void ConstructorTest(string contentTypeXmlns, string relationshipXmlns, int estimatedContentTypeCount, int estimatedSectionCount, bool expectedRelationshipNsNull, string expectesRelationShipNs, string expectedContetType)
        {
            XmlDocumentCollection collection = new XmlDocumentCollection(contentTypeXmlns, relationshipXmlns, estimatedContentTypeCount, estimatedSectionCount);
            Assert.Multiple(() =>
            {
                Assert.IsNotNull(collection);
                Assert.IsNotNull(collection.ContentTypes);
                Assert.AreEqual(collection.ContentTypes.GetXmlString(), expectedContetType);
                if (expectedRelationshipNsNull)
                {
                    Assert.IsNull(collection.RelationshipXmlns);
                }
                else
                {
                    Assert.IsNotNull(collection.RelationshipXmlns);
                    Assert.AreEqual(collection.RelationshipXmlns, expectesRelationShipNs);
                }
            });
        }

        [Description("Test of the AddSection method")]
        [TestCase("/path1", 10, "/path1")]
        [TestCase(null, 0, null)]
        [TestCase("", -10, "")]
        public void AddSectionTest(string path, int count, string expectedPath)
        {
            XmlDocumentCollection collection = new XmlDocumentCollection("CNS1", "RNS1");
            collection.AddSection(path, count);
            Assert.Multiple(() =>
            {
                Assert.IsNotNull(collection.RelationshipSections);
                Assert.AreEqual(collection.RelationshipSections.Count, 1);
                Assert.AreEqual(collection.RelationshipSections[0].Path, expectedPath);
            });
        }

        [Description("Test of the AddSection method with multiple entries")]
        [TestCase("/path1|/path2", 10, 2, "/path1|/path2")]
        [TestCase("P1|P2|P3", 0, 3, "P1|P2|P3")]
        [TestCase("|||", -10, 4, "|||")]
        public void AddSectionTest2(string path, int count, int expectedCount, string expectedPath)
        {
            XmlDocumentCollection collection = new XmlDocumentCollection("CNS1", "RNS1");
            string[] paths = TestUtils.SplitValues(path, '|');
            string[] expectedPaths = TestUtils.SplitValues(expectedPath, '|');
            for(int i = 0; i < paths.Length; i++)
            {
                collection.AddSection(paths[i], count);
            }
            Assert.Multiple(() =>
            {
                Assert.AreEqual(collection.RelationshipSections.Count, expectedCount);
                Assert.IsNotNull(collection.RelationshipSections);
                for (int i = 0; i < paths.Length; i++)
                {
                    Assert.AreEqual(collection.RelationshipSections[i].Path, expectedPaths[i]);
                } 
            });
        }
    }
}
