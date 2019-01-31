/*
 * MXG - Minimalist XML Generator
 * Copyright Raphael Stoeckli © 2019
 * This library is licensed under the MIT License.
 * You find a copy of the license in project folder or on: http://opensource.org/licenses/MIT
 */

using MXG.Core;
using NUnit.Framework;
using System;
using System.Text;

namespace MXGTest.Core
{
    [TestFixture, Description("Test class for XmlElement class")]
    public class XmlElementTest
    {
        [Description("Test the static factory method CreateXmlElement (without namespace). Name checks are omitted in this scenario")]
        [TestCase ("tag", "t1", false, "<tag>t1</tag>", false)]
        [TestCase("tag", null, false, "<tag/>", true)]
        [TestCase("xyz", null, true, "<xyz/>", true)]
        [TestCase("xyz", "t2", true, "<xyz>t2</xyz>", false)]
        [TestCase("xyz", "", false, "<xyz></xyz>", false)]
        [TestCase("a", "", true, "<a></a>", false)]
        public void CreateXmlElementTest(string name, string content, bool escapeContent, string expectedString, bool expectedEmpty)
        {
            XmlElement element = XmlElement.CreateXmlElement(name, true, content, escapeContent);
            StringBuilder sb = new StringBuilder();
            element.AppendXmlString(sb);
            Assert.Multiple(() =>
            {
                Assert.AreEqual(sb.ToString(), expectedString);
                Assert.AreEqual(element.HasNameSpace, false);
                Assert.AreEqual(element.IsEmpty, expectedEmpty);
            });
        }

        [Description("Test the static factory method CreateXmlElement (with name space). Name checks are omitted in this scenario")]
        [TestCase("tag", null, "t1", false, "<tag>t1</tag>", false)]
        [TestCase("tag", null, null, false, "<tag/>", false)]
        [TestCase("tag", "", "t1", false, "<:tag>t1</:tag>", true)] // However, this is not valid but accepted (assert pass) since no name check is executed though
        [TestCase("tag", "", null, false, "<:tag/>", true)] // However, this is not valid but accepted (assert pass) since no name check is executed though
        [TestCase("tag", "ns", "t1", false, "<ns:tag>t1</ns:tag>", true)]
        [TestCase("tag", "ns", null, false, "<ns:tag/>", true)]
        [TestCase("tag", "ns", "t1", false, "<ns:tag>t1</ns:tag>", true)]
        [TestCase("tag", "ns", null, false, "<ns:tag/>", true)]
        [TestCase("tag", "ns", "\x7", true, "<ns:tag> </ns:tag>", true)]
        [TestCase("tag", null, "\x7\t\xFFFE", true, "<tag> \t </tag>", false)]
        public void CreateXmlElementTest2(string name, string nameSpace, string content, bool escapeContent, string expectedString, bool expectedNameSpace)
        {
            XmlElement element = XmlElement.CreateXmlElement(name, nameSpace, true, content, escapeContent);
            StringBuilder sb = new StringBuilder();
            element.AppendXmlString(sb);
            Assert.Multiple(() =>
            {
                Assert.AreEqual(sb.ToString(), expectedString);
                Assert.AreEqual(element.HasNameSpace, expectedNameSpace);
            });
        }

        [Description("Test the static factory method CreateXmlElement for name validation (name and name space)")]
        [TestCase("tag", null, true)]      // valid
        [TestCase("X", null, true)]        // valid
        [TestCase("_", null, true)]        // valid
        [TestCase("", null, false)]          // invalid
        [TestCase(null, null, false)]        // invalid
        [TestCase("\t", null, false)]        // invalid
        [TestCase("\xB7_element", null, false)] // invalid
        [TestCase("tag", "", false)]         // invalid
        [TestCase("X", null, true)]        // valid
        [TestCase("_", "_", true)]         // valid
        [TestCase("", "x", false)]           // invalid
        [TestCase(null, "ns", false)]        // invalid
        [TestCase("\t", "ns", false)]        // invalid
        [TestCase("\xB7_element", "x", false)] // invalid
        [TestCase("tag", "\xB7", false)]     // invalid
        [TestCase("tag", "ns", true)]      // valid
        public void CreateXmlElementTest3(string name, string nameSpace, bool expected)
        {
            try
            {
                XmlElement element = XmlElement.CreateXmlElement(name, nameSpace, false);
                if (!expected)
                {
                    Assert.Fail();
                    return;
                }
            }
            catch (Exception e)
            {
                if (expected)
                {
                    Assert.Fail();
                }
            }
        }

        [Description("Test the method AddChild")]
        [TestCase("tag", "subtag", "<subtag/>")]
        [TestCase("tag", "subtag¦ns", "<ns:subtag/>")]
        [TestCase("tag", "subtag¦ns¦content1", "<ns:subtag>content1</ns:subtag>")]
        [TestCase("tag", "subtag|subtag2", "<subtag/>|<subtag2/>")]
        [TestCase("tag", "subtag¦ns|subtag2¦ns2", "<ns:subtag/>|<ns2:subtag2/>")]
        [TestCase("tag", "subtag¦ns¦content1|subtag2¦ns2¦content2", "<ns:subtag>content1</ns:subtag>|<ns2:subtag2>content2</ns2:subtag2>")]
        public void AddChildTest(string name, string childString, string expectedChildrenString)
        {
            XmlElement element = new XmlElement(name);
            string[][] children = TestUtils.SplitValues(childString, '|', '¦');
            foreach (string[] child in children)
            {
                XmlElement childElement;
                if (child.Length == 1)
                {
                    childElement = new XmlElement(child[0]);
                    
                }
                else if (child.Length == 2)
                {
                    childElement = new XmlElement(child[0],child[1]);
                }
                else if (child.Length == 3)
                {
                    childElement = new XmlElement(child[0], child[1], child[2]);
                }
                else // Not applicable
                {
                    Assert.Fail();
                    return;
                }
                element.AddChild(childElement);
            }
            string[] expectedChildren = TestUtils.SplitValues(expectedChildrenString, '|');
            for (int i = 0; i < element.Children.Count; i++)
            {
                Assert.That(expectedChildren[i], Is.EqualTo(element.Children[i].ToString()));
            }
        }

        [Description("Test the method AddChild with the entire XML element as output")]
        [TestCase("tag", "subtag", "<tag><subtag/></tag>")]
        [TestCase("tag2", "subtag¦ns", "<tag2><ns:subtag/></tag2>")]
        [TestCase("tag|ns1", "subtag¦ns¦content1", "<ns1:tag><ns:subtag>content1</ns:subtag></ns1:tag>")]
        [TestCase("tag|ns2|rootContent", "subtag|subtag2", "<ns2:tag>rootContent<subtag/><subtag2/></ns2:tag>")]
        [TestCase("tag", "subtag¦ns|subtag2¦ns2", "<tag><ns:subtag/><ns2:subtag2/></tag>")]
        [TestCase("tag|ns3", "subtag¦ns¦content1|subtag2¦ns2¦content2", "<ns3:tag><ns:subtag>content1</ns:subtag><ns2:subtag2>content2</ns2:subtag2></ns3:tag>")]
        public void AddChildTest2(string name, string childString, string expectedString)
        {
            string[] root = TestUtils.SplitValues(name, '|');
            XmlElement element = TestUtils.GetElement(root);
            if (element == null)
            {
                Assert.Fail();
                return;
            }
            string[][] children = TestUtils.SplitValues(childString, '|', '¦');
            foreach (string[] child in children)
            {
                XmlElement childElement = TestUtils.GetElement(child);
                if (childElement == null)
                {
                    Assert.Fail();
                    return;
                }
                element.AddChild(childElement);

            }
            Assert.That(expectedString, Is.EqualTo(element.ToString()));
        }

        [Description("Test the method AddChild and AddAttribute with the entire XML element as output. !! is a placeholder for null")]
        [TestCase("tag#att¦v1", "subtag#att2¦v2", "<tag att=\"v1\"><subtag att2=\"v2\"/></tag>")]
        [TestCase("tag#att¦v1|att2¦v2", "subtag#att3¦v3|att4¦!!", "<tag att=\"v1\" att2=\"v2\"><subtag att3=\"v3\" att4/></tag>")]
        /*
        [TestCase("tag2", "subtag¦ns", "<tag2><ns:subtag/></tag2>")]
        [TestCase("tag|ns1", "subtag¦ns¦content1", "<ns1:tag><ns:subtag>content1</ns:subtag></ns1:tag>")]
        [TestCase("tag|ns2|rootContent", "subtag|subtag2", "<ns2:tag>rootContent<subtag/><subtag2/></ns2:tag>")]
        [TestCase("tag", "subtag¦ns|subtag2¦ns2", "<tag><ns:subtag/><ns2:subtag2/></tag>")]
        [TestCase("tag|ns3", "subtag¦ns¦content1|subtag2¦ns2¦content2", "<ns3:tag><ns:subtag>content1</ns:subtag><ns2:subtag2>content2</ns2:subtag2></ns3:tag>")]
    */
        public void AddChildTest3(string parentString, string childString, string expectedString)
        {
            string[] tokens = TestUtils.SplitValues(parentString, '#');
            string[] root = TestUtils.SplitValues(tokens[0], '|');
            XmlElement element = TestUtils.GetElement(root);
            if (element == null)
            {
                Assert.Fail();
                return;
            }
            if (tokens.Length == 2)
            {
                TestUtils.AppendXmlAttributes(ref element, tokens[1], '|', '¦', "!!");
            }
            tokens = TestUtils.SplitValues(childString, '#');
            string[][] children = TestUtils.SplitValues(tokens[0], '|', '¦');
            foreach (string[] child in children)
            {
                XmlElement childElement = TestUtils.GetElement(child);
                if (childElement == null)
                {
                    Assert.Fail();
                    return;
                }
                if (tokens.Length == 2)
                {
                    TestUtils.AppendXmlAttributes(ref childElement, tokens[1], '|', '¦', "!!");
                }
                element.AddChild(childElement);
            }
            Assert.That(expectedString, Is.EqualTo(element.ToString()));
        }

        [Description("Test the Exception Handling of the XmlElement class")]
        [TestCase("n",null, null, null, true)]       // valid
        [TestCase("\t", null, null, null, false)]       // invalid
        [TestCase("\n", null, null, null, false)]       // invalid
        [TestCase("n", "n", null, null, true)]       // valid
        [TestCase("n", "", null, null, false)]       // invalid
        [TestCase("n", null, "x", null, true)]       // valid
        [TestCase("n", null, "x", "y", true)]       // valid
        [TestCase("n", null, "x", "", true)]       // valid
        [TestCase("n", null, "", null, false)]       // invalid
        [TestCase("n", null, "\t", null, false)]       // invalid
        [TestCase("x", "x", "x", "\t", true)]       // valid
        public void XmleElementErrorTest(string name, string nameSpace, string attributeName, string attributeValue, bool expected)
        {
            try
            {
                XmlElement element = new XmlElement(name, nameSpace);
                if (attributeName != null && attributeValue != null)
                {
                    element.AddAttribute(attributeName, attributeValue, false, false);
                }
                else if (attributeName != null && attributeValue == null)
                {
                    element.AddAttribute(attributeName, false);
                }
                if (!expected)
                {
                    Assert.Fail();
                    return;
                }
            }
            catch (Exception e)
            {
                if (expected)
                {
                    Assert.Fail();
                }
            }
        }

    }
}
