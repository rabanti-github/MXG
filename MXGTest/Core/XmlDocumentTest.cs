using MXG.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXGTest.Core
{
    [TestFixture, Description("Test class for XmlDocument class")]
    public class XmlDocumentTest
    {

        [Description("Test the constructor of the XmlDocument class with the available properties")]
        [TestCase(10, "UTF-8", true, "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>")]
        [TestCase(0, "ASCII", false, "<?xml version=\"1.0\" encoding=\"ASCII\" standalone=\"no\"?>")]
        [TestCase(-1, null, null, "<?xml version=\"1.0\"?>")]
        [TestCase(5, "", null, "<?xml version=\"1.0\"?>")]
        public void ConstructorTest(int estimatedContentTypeCount, string encoding, bool? standalone, string expectedString)
        {
            XmlDocument document = new XmlDocument(estimatedContentTypeCount, encoding, standalone);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(document);
                Assert.AreEqual(document.GetXmlString(), expectedString);
            });
        }

        [Description("Test the AddChild method")]
        [TestCase("el¦!!¦!!", "ASCII", null, "<?xml version=\"1.0\" encoding=\"ASCII\"?><el/>")]
        [TestCase("el¦!!¦!!", "UTF-8", false, "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?><el/>")]
        [TestCase("el¦ns¦!!", null, true, "<?xml version=\"1.0\" standalone=\"yes\"?><ns:el/>")]
        [TestCase("el¦ns¦content", "UTF-8", true, "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><ns:el>content</ns:el>")]
        [TestCase("el¦ns¦c1|el2¦!!¦c2", "UTF-8", true, "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><ns:el>c1</ns:el><el2>c2</el2>")]
        [TestCase("el¦ns¦c1|el2¦!!¦c2|el3¦!!¦!!", "", null, "<?xml version=\"1.0\"?><ns:el>c1</ns:el><el2>c2</el2><el3/>")]
        public void AddChildTest(string elementString, string encoding, bool? standAlone, string expected)
        {
            string[][] tokens = TestUtils.SplitValues(elementString, '|', '¦', "!!");
            XmlDocument doc = new XmlDocument(10,encoding, standAlone);
            foreach(string[] token in tokens)
            {
                XmlElement element = TestUtils.GetElement(token);
                doc.AddChild(element);
            }
            Assert.That(doc.GetXmlString(), Is.EqualTo(expected));
        }

        [Description("Test the AddChild method (1st overloading)")]
        [TestCase("el¦!!¦!!", "ASCII", null, "<?xml version=\"1.0\" encoding=\"ASCII\"?><el/>")]
        [TestCase("el¦!!¦!!", "UTF-8", false, "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?><el/>")]
        [TestCase("el¦!!¦!!", null, true, "<?xml version=\"1.0\" standalone=\"yes\"?><el/>")]
        [TestCase("el¦!!¦content", "UTF-8", true, "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><el>content</el>")]
        [TestCase("el¦!!¦c1|el2¦!!¦c2", "UTF-8", true, "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><el>c1</el><el2>c2</el2>")]
        [TestCase("el¦!!¦c1|el2¦!!¦c2|el3¦!!¦!!", "", null, "<?xml version=\"1.0\"?><el>c1</el><el2>c2</el2><el3/>")]
        public void AddChildTest2(string elementString, string encoding, bool? standAlone, string expected)
        {
            string[][] tokens = TestUtils.SplitValues(elementString, '|', '¦', "!!");
            XmlDocument doc = new XmlDocument(10, encoding, standAlone);
            foreach (string[] token in tokens)
            {
                doc.AddChild(token[0], true, token[2], true);
            }
            Assert.That(doc.GetXmlString(), Is.EqualTo(expected));
        }

        [Description("Test the AddChild method (2nd overloading)")]
        [TestCase("el¦!!¦!!", "ASCII", null, "<?xml version=\"1.0\" encoding=\"ASCII\"?><el/>")]
        [TestCase("el¦!!¦!!", "UTF-8", false, "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?><el/>")]
        [TestCase("el¦ns¦!!", null, true, "<?xml version=\"1.0\" standalone=\"yes\"?><ns:el/>")]
        [TestCase("el¦ns¦content", "UTF-8", true, "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><ns:el>content</ns:el>")]
        [TestCase("el¦ns¦c1|el2¦!!¦c2", "UTF-8", true, "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><ns:el>c1</ns:el><el2>c2</el2>")]
        [TestCase("el¦ns¦c1|el2¦!!¦c2|el3¦!!¦!!", "", null, "<?xml version=\"1.0\"?><ns:el>c1</ns:el><el2>c2</el2><el3/>")]
        public void AddChildTest3(string elementString, string encoding, bool? standAlone, string expected)
        {
            string[][] tokens = TestUtils.SplitValues(elementString, '|', '¦', "!!");
            XmlDocument doc = new XmlDocument(10, encoding, standAlone);
            foreach (string[] token in tokens)
            {
                doc.AddChild(token[0], token[1], true, token[2], true);
            }
            Assert.That(doc.GetXmlString(), Is.EqualTo(expected));
        }


    }
}
