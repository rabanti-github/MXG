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

        /*
        [Description("Test the function CalculateCapacity indirectly by its output property Capacity")]
        [TestCase("att", "x1", 9)]
        [TestCase("att", "", 7)]
        [TestCase("att", null, 4)]
        [TestCase("att", "   ", 10)]
        [TestCase("att", "&amp;")]
        public void CalculateCapacityTest(string name, string value)
        {
            XmlAttribute attribute = new XmlAttribute(name, value);
          //  Assert.That(expectedSize, Is.EqualTo(attribute.Capacity));
        }
        */

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
            Assert.That(excpectedValue, Is.EqualTo(attribute.Value));
        }

        [Description("Test the function GetXmlString")]
        [TestCase("att", "x1", false, "att=\"x1\" ")]
        [TestCase("att", "", false, "att=\"\" ")]
        [TestCase("att", "x\x4y", true, "att=\"x y\" ")]
        [TestCase("att", null, false, "att ")]
        [TestCase("att", null, true, "att=\"\" ")]
        public void GetXmlStringTest(string name, string value, bool escapeValue, string excpectedString)
        {
            XmlAttribute attribute = new XmlAttribute(name);
            attribute.SetContent(value, escapeValue);
            StringBuilder sb = new StringBuilder();
            attribute.AppendXmlString(sb);
            Assert.That(excpectedString, Is.EqualTo(sb.ToString()));
        }

        [Description("Test the Exception Handling of the Factory method")]
        [TestCase("att", true, false)]       // valid
        [TestCase("att", false, false)]      // valid
        [TestCase(":att", true, false)]      // valid
        [TestCase("0att", true, false)]      // no check - valid
        [TestCase("0att", false, true)]      // check - invalid
        [TestCase("<x>", false, true)]       // invalid
        [TestCase("xml", false, false)]      // valid
        [TestCase("x y", false, true)]       // invalid
        [TestCase("x\x4y", false, true)]     // invalid
        [TestCase(";", false, true)]         // invalid
        [TestCase("-att", false, true)]      // invalid
        [TestCase("\xE4", false, false)]     // valid
        [TestCase("_att", false, false)]     // valid
        [TestCase("\xB7_att", false, true)]  // invalid
        [TestCase("x\xB7_att", false, false)]// valid
        [TestCase("", false, true)]          // invalid
        [TestCase(null, false, true)]        // invalid
        public void NameErrorTest(string name, bool skipNameCheck, bool exceptionExcepted)
        {
            try
            {
                XmlAttribute.CreateXmlAttribute(name, null, skipNameCheck, false);
                if (exceptionExcepted)
                {
                    Assert.Fail();
                    return;
                }
            }
            catch(Exception e)
            {
                if (!exceptionExcepted)
                {
                    Assert.Fail();
                }
            }
        }

    }
}
