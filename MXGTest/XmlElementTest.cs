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

namespace MXGTest
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
        [TestCase("tag", "", "t1", false, "<:tag>t1</tag>", true)] // However, this is not valid but accepted (assert pass) since no name check is executed though
        [TestCase("tag", "", null, false, "<:tag/>", true)] // However, this is not valid but accepted (assert pass) since no name check is executed though
        [TestCase("tag", "ns", "t1", false, "<ns:tag>t1</tag>", true)]
        [TestCase("tag", "ns", null, false, "<ns:tag/>", true)]
        [TestCase("tag", "ns", "t1", false, "<ns:tag>t1</tag>", true)]
        [TestCase("tag", "ns", null, false, "<ns:tag/>", true)]
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
        [TestCase("tag", null, false)]      // valid
        [TestCase("X", null, false)]        // valid
        [TestCase("_", null, false)]        // valid
        [TestCase("", null, true)]          // invalid
        [TestCase(null, null, true)]        // invalid
        [TestCase("\t", null, true)]        // invalid
        [TestCase("\xB7_element", null, true)] // invalid
        [TestCase("tag", "", true)]         // invalid
        [TestCase("X", null, false)]        // valid
        [TestCase("_", "_", false)]         // valid
        [TestCase("", "x", true)]           // invalid
        [TestCase(null, "ns", true)]        // invalid
        [TestCase("\t", "ns", true)]        // invalid
        [TestCase("\xB7_element", "x", true)] // invalid
        [TestCase("tag", "\xB7", true)]     // invalid
        [TestCase("tag", "ns", false)]      // valid
        public void CreateXmlElementTest3(string name, string nameSpace, bool exceptionExpected)
        {
            try
            {
                XmlElement element = XmlElement.CreateXmlElement(name, nameSpace, false);
                if (exceptionExpected)
                {
                    Assert.Fail();
                    return;
                }
            }
            catch (Exception e)
            {
                if (!exceptionExpected)
                {
                    Assert.Fail();
                }
            }
        }


    }
}
