/*
 * MXG - Minimalist XML Generator
 * Copyright Raphael Stoeckli © 2019
 * This library is licensed under the MIT License.
 * You find a copy of the license in project folder or on: http://opensource.org/licenses/MIT
 */

using MXG;
using NUnit.Framework;
using System.Text;

namespace MXGTest
{
    [TestFixture, Description("Test class for XmlElement class")]
    public class XmlElementTest
    {
        [Description("Test the static factory method CreateXmlElement (without names pace). Name checks are omitted in this scenario")]
        [TestCase ("tag", "t1", false, "<tag>t1</tag>", false, 13)]
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
    }
}
