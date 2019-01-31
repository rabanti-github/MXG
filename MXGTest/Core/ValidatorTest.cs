/*
 * MXG - Minimalist XML Generator
 * Copyright Raphael Stoeckli © 2019
 * This library is licensed under the MIT License.
 * You find a copy of the license in project folder or on: http://opensource.org/licenses/MIT
 */
using System;
using MXG.Core;
using NUnit.Framework;


namespace MXGTest.Core
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

        [Description("Test the static function ValidateAttributeName for specific illegal start characters")]
        [TestCase("\x3Axxx", true)] // :
        [TestCase("\x5Fyyy", true)] // _
        [TestCase("\x39---", false)] 
        [TestCase("\x45---", true)] // a-Z
        [TestCase("\x5Cmmm", false)]
        [TestCase("\x68vvv", true)] // A-Z
        [TestCase("\x7Brrr", false)]
        [TestCase("\xC1sss", true)]
        [TestCase("\xD7nnn", false)]
        [TestCase("\xDAlll", true)]
        [TestCase("\xF7ttt", false)]
        [TestCase("\xF9hhh", true)]
        [TestCase("\x300kkk", false)]
        [TestCase("\x371xxx", true)]
        [TestCase("\x37Eyyy", false)]
        [TestCase("\x380sss", true)]
        [TestCase("\x2000ppp", false)]
        [TestCase("\x200Dlll", true)]
        [TestCase("\x2069ttt", false)]
        [TestCase("\x2072sss", true)]
        [TestCase("\x2BFFnnn", false)]
        [TestCase("\x2C10hhh", true)]
        [TestCase("\x3000kkk", false)]
        [TestCase("\x3005---", true)]
        [TestCase("\xF8FF___", false)]
        [TestCase("\xF9005jjj", true)]
        [TestCase("\xFDD0iii", false)]
    
        [TestCase("\xFDF5zzz", true)]
        [TestCase("\xFFFFppp", false)]
        [TestCase("\x10002sss", true)]
       // [TestCase("\uEFFFF", true)] // Invalid range in C#
       // [TestCase("\uF0005ppp", false)] // "
        public void ValidateAttributeNameTest2(string input, bool expected)
        {
            bool result = Validator.ValidateAttributeName(input);
            Assert.That(expected, Is.EqualTo(result));
        }

        [Description("Test the static function ValidateAttributeName for specific illegal characters after the start character")]
        [TestCase("a\x3Axxx", true)] // :
        [TestCase("a\x5Fyyy", true)] // _
        [TestCase("a\x39---", true)]
        [TestCase("a\x45---", true)] // a-Z
        [TestCase("a\x5Cmmm", false)]
        [TestCase("a\x68vvv", true)] // A-Z
        [TestCase("a\x7Brrr", false)]
        [TestCase("a\xC1sss", true)]
        [TestCase("a\xD7nnn", false)]
        [TestCase("a\xDAlll", true)]
        [TestCase("a\xF7ttt", false)]
        [TestCase("a\xF9hhh", true)]
        [TestCase("a\x300kkk", true)]
        [TestCase("a\x371xxx", true)]
        [TestCase("a\x37Eyyy", false)]
        [TestCase("a\x380sss", true)]
        [TestCase("a\x2000ppp", false)]
        [TestCase("a\x200Dlll", true)]
        [TestCase("a\x2069ttt", false)]
        [TestCase("a\x2072sss", true)]
        [TestCase("a\x2BFFnnn", false)]
        [TestCase("a\x2C10hhh", true)]
        [TestCase("a\x3000kkk", false)]
        [TestCase("a\x3005---", true)]
        [TestCase("a\xF8FF___", false)]
        [TestCase("a\xF9005jjj", true)]
        [TestCase("a\xFDD0iii", false)]
        [TestCase("a\xFDF5zzz", true)]
        [TestCase("a\xFFFFppp", false)]
        [TestCase("a\x10002sss", true)]
        [TestCase("a\x2Dsss", true)]        // Additional allowed characters
        [TestCase("a\x2Exxx", true)]
        [TestCase("a\xB7zzz", true)]
        [TestCase("a\x31ttt", true)]
        [TestCase("a\x305sss", true)]
        [TestCase("a\x2040vvv", true)]
        public void ValidateAttributeNameTest3(string input, bool expected)
        {
            bool result = Validator.ValidateAttributeName(input);
            Assert.That(expected, Is.EqualTo(result));
        }

    }
}
