/*
 * MXG - Minimalist XML Generator
 * Copyright Raphael Stoeckli © 2019
 * This library is licensed under the MIT License.
 * You find a copy of the license in project folder or on: http://opensource.org/licenses/MIT
 */
using System;
using MXG.Core;
using NUnit.Framework;


namespace MXGTest
{
    [TestFixture, Description("Test class for Validator class")]
    public class ValidatorTest
    {
        [Description("Test the static function EscapeXmlAttributeChars")]
        [TestCase("test", "test")]
        [TestCase(" ", " ")]
        [TestCase("'x'", "'x'")]
        [TestCase("\"\"", "&quot;&quot;")]
        [TestCase("'\"'", "'&quot;'")]
        [TestCase("\"\x4\"", "&quot; &quot;")]
        [TestCase("<\"test\">", "&lt;&quot;test&quot;&gt;")]
        [TestCase(null, "")]
        public void EscapeXmlAttributeCharsTest(string input, string expected)
        {
            string result = Validator.EscapeXmlAttributeChars(input);
            Assert.That(expected, Is.EqualTo(result));
        }

        [Description("Test the static function EscapeXmlChars")]
        [TestCase("test", "test")]
        [TestCase(" ", " ")]
        [TestCase(" test ", " test ")]
        [TestCase("\"test\"", "\"test\"")]
        [TestCase("`test´", "`test´")]
        [TestCase("'test'", "'test'")]
        [TestCase("&amp;", "&amp;amp;")]
        [TestCase("<test>", "&lt;test&gt;")]
        [TestCase("x&y", "x&amp;y")]
        [TestCase("<&>", "&lt;&amp;&gt;")]
        [TestCase("-\x8-", "- -")]
        [TestCase("\x5", " ")]
        [TestCase("", "")]
        public void EscapeXmlCharsTest(string input, string expected)
        {
            string result = Validator.EscapeXmlChars(input);
            Assert.That(expected, Is.EqualTo(result));
        }

        [Description("Test the static function ValidateElementeName")]
        [TestCase("test", true)]
        [TestCase(":test", true)]
        [TestCase("Test0", true)]
        [TestCase("_test", true)]
        [TestCase("tèst", true)]
        [TestCase("xml", false)]
        [TestCase("XML", false)]
        [TestCase("Xml", false)]
        [TestCase("XMl", false)]
        [TestCase("XmL", false)]
        [TestCase("xmL", false)]
        [TestCase("xML", false)]
        [TestCase("test\x3test", false)]
        [TestCase("test test", false)]
        [TestCase("Test ", false)]
        [TestCase("0test", false)]
        [TestCase("", false)]
        [TestCase(null, false)]
        public void ValidateElementeNameTest(string input, bool expected)
        {
            bool result = Validator.ValidateElementName(input);
            Assert.That(expected, Is.EqualTo(result));
        }

        [Description("Test the static function ValidateAttributeName")]
        [TestCase("test",true)]
        [TestCase(":test", true)]
        [TestCase("Test0", true)]
        [TestCase("_test", true)]
        [TestCase("tèst", true)]
        [TestCase("test\x3test", false)]
        [TestCase(" test", false)]
        [TestCase("Test ", false)]
        [TestCase("0test", false)]
        [TestCase("", false)]
        [TestCase(null, false)]
        public void ValidateAttributeNameTest(string input, bool expected)
        {
            bool result = Validator.ValidateAttributeName(input);
            Assert.That(expected, Is.EqualTo(result));
        }


    }
}
