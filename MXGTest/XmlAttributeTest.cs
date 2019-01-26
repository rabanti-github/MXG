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
    [TestFixture, Description("Test class for XmlAttribute class")]
    public class XmlAttributeTest
    {

        [Description("Test the static factory method CreateXmlAttribute. Name checks are omitted in this scenario")]
        [TestCase("att", "x1", false, "att", "x1", false)]
        [TestCase("att", "x1 ", false, "att", "x1 ", false)]
        [TestCase("att", "x\x4X", false, "att", "x\x4X", false)]
        [TestCase("att", "x\x4Y", true, "att", "x Y", false)]
        [TestCase("att", null, false, "att", null, true)]
        [TestCase("att", "", false, "att", "", false)]
        [TestCase("att", null, true, "att", "", false)]
        public void CreateXmlAttributeTest(string name, string value, bool escapeValue, string expectedName, string expectedValue, bool expectedEmpty)
        {
            XmlAttribute attribute = XmlAttribute.CreateXmlAttribute(name, value, true, escapeValue);
            Assert.Multiple(() =>
            {
                Assert.AreEqual(attribute.Name, expectedName);
                Assert.AreEqual(attribute.Value, expectedValue);
                Assert.AreEqual(attribute.IsEmpty, expectedEmpty);
            });
        }

        [Description("Test the function SetContent indirectly by its output property Value")]
        [TestCase("x1", false, "x1")]
        [TestCase("x1", true, "x1")]
        [TestCase("", false, "")]
        [TestCase("", true, "")]
        [TestCase(null, false, null)]
        [TestCase(null, true, "")]
        [TestCase("x\x3y", false, "x\x3y")]
        [TestCase("x\x3y", true, "x y")]
        [TestCase("<a>", true, "&lt;a&gt;")]
        public void SetContentTest(string value, bool escapeValue, string excpectedValue)
        {
            XmlAttribute attribute = new XmlAttribute("att");
            attribute.SetContent(value, escapeValue);
            Assert.That(attribute.Value, Is.EqualTo(excpectedValue));
        }

        [Description("Test the function GetXmlString")]
        [TestCase("att", "x1", false, " att=\"x1\"")]
        [TestCase("att", "", false, " att=\"\"")]
        [TestCase("att", "x\x4y", true, " att=\"x y\"")]
        [TestCase("att", null, false, " att")]
        [TestCase("att", null, true, " att=\"\"")]
        public void GetXmlStringTest(string name, string value, bool escapeValue, string excpectedString)
        {
            XmlAttribute attribute = new XmlAttribute(name);
            attribute.SetContent(value, escapeValue);
            StringBuilder sb = new StringBuilder();
            attribute.AppendXmlString(sb);
            Assert.That(sb.ToString(), Is.EqualTo(excpectedString));
        }

        [Description("Test the Exception Handling of the Factory method")]
        [TestCase("att", true, true)]       // valid
        [TestCase("att", false, true)]      // valid
        [TestCase(":att", true, true)]      // valid
        [TestCase("0att", true, true)]      // no check - valid
        [TestCase("0att", false, false)]      // check - invalid
        [TestCase("<x>", false, false)]       // invalid
        [TestCase("xml", false, true)]      // valid
        [TestCase("x y", false, false)]       // invalid
        [TestCase("x\x4y", false, false)]     // invalid
        [TestCase(";", false, false)]         // invalid
        [TestCase("-att", false, false)]      // invalid
        [TestCase("\xE4", false, true)]     // valid
        [TestCase("_att", false, true)]     // valid
        [TestCase("\xB7_att", false, false)]  // invalid
        [TestCase("x\xB7_att", false, true)]// valid
        [TestCase("", false, false)]          // invalid
        [TestCase(null, false, false)]        // invalid
        public void NameErrorTest(string name, bool skipNameCheck, bool expected)
        {
            try
            {
                XmlAttribute.CreateXmlAttribute(name, null, skipNameCheck, false);
                if (!expected)
                {
                    Assert.Fail();
                    return;
                }
            }
            catch(Exception e)
            {
                if (expected)
                {
                    Assert.Fail();
                }
            }
        }

    }
}
