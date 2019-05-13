/*
 * MXG - Minimalist XML Generator
 * Copyright Raphael Stoeckli © 2019
 * This library is licensed under the MIT License.
 * You find a copy of the license in project folder or on: http://opensource.org/licenses/MIT
 */
using System;
using System.IO;
using System.Text;
using MXG.Core;
using NUnit.Framework;

namespace MXGTest.Core
{
    [TestFixture, Description("Test class to cover shared save functions of AbstractXmlDocument")]
    class AbstractDocumentSaveTest
    {
        [Description("Test the SaveAsStream method (save to stream synchronous)")]
        [Test]
        public void SaveAsStreamTest()
        {
            string expectedString;
            XmlDocument doc = GetSampleDocument(out expectedString);
            MemoryStream ms = new MemoryStream();
            doc.SaveAsStream(ms, true);
            ms.Position = 0;
            StreamReader sr = new StreamReader(ms);
            string result = sr.ReadToEnd();
            Assert.AreEqual(result, expectedString);
        }

        [Description("Test the SaveAsStream method with all parameters (save to stream synchronous)")]
        [Test]
        public void SaveAsStreamTest2()
        {
            string expectedString;
            XmlDocument doc = GetSampleDocument(out expectedString);
            MemoryStream ms = new MemoryStream();
            doc.SaveAsStream(ms, Encoding.ASCII, 1024, true);
            ms.Position = 0;
            StreamReader sr = new StreamReader(ms);
            string result = sr.ReadToEnd();
            Assert.AreEqual(result, expectedString);
        }

        [Description("Test the Save method (save to file synchronous)")]
        [Test]
        public void SaveTest()
        {
            string expectedString;
            string file = Path.GetTempPath() + "saveTest_" + DateTime.Now.ToString("YYYYMMddhhmmssffff") + ".xml";
            XmlDocument doc = GetSampleDocument(out expectedString);
            doc.Save(file);
            System.Threading.Thread.Sleep(1000); // Not good, but easy (wait for IO)
            using(StreamReader sr = new StreamReader(file))
            {
                string result = sr.ReadToEnd();
                Assert.AreEqual(result, expectedString);
            }
            try
            {
                File.Delete(file);
            }
            catch (Exception ex)
            {
                Assert.Fail("Test file " + file + " could not be deleted: " + ex.Message);
            }            
        }

        [Description("Test the Save method with all parameters (save to file synchronous)")]
        [Test]
        public void SaveTest2()
        {
            string expectedString;
            string file = Path.GetTempPath() + "saveTest_" + DateTime.Now.ToString("YYYYMMddhhmmssffff") + ".xml";
            XmlDocument doc = GetSampleDocument(out expectedString);
            doc.Save(file, Encoding.UTF8, 512);
            System.Threading.Thread.Sleep(1000); // Not good, but easy (wait for IO)
            using (StreamReader sr = new StreamReader(file))
            {
                string result = sr.ReadToEnd();
                Assert.AreEqual(result, expectedString);
            }
            try
            {
                File.Delete(file);
            }
            catch (Exception ex)
            {
                Assert.Fail("Test file " + file + " could not be deleted: " + ex.Message);
            }
        }


        private XmlDocument GetSampleDocument(out string stringResult)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement element = new XmlElement("test");
            element.AddAttribute("testAttribute", "attributeValue");
            element.SetContent("testContent");
            doc.AddChild(element);
            stringResult = doc.GetXmlString();
            return doc;
        }

    }
}
