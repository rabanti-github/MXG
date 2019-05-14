/*
 * MXG - Minimalist XML Generator
 * Copyright Raphael Stoeckli © 2019
 * This library is licensed under the MIT License.
 * You find a copy of the license in project folder or on: http://opensource.org/licenses/MIT
 */
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using MXG.Core;
using NUnit.Framework;

namespace MXGTest.Core
{
    [TestFixture, Description("Test class to cover shared save functions of AbstractXmlDocument")]
    class AbstractDocumentSaveTest
    {
        [Description("Test the SaveAsStream method with terminating stream (save to stream synchronous)")]
        [Test]
        public void SaveAsStreamTest()
        {
            string file = GetTempFileName();
            string expectedString, result;
            bool status;
            XmlDocument doc = GetSampleDocument(out expectedString);
            using(FileStream fs = new FileStream(file, FileMode.Create))
            {
                doc.SaveAsStream(fs);
            }
            result = ReadAndDeleteTestFile(file, 1000, out status);
            Assert.Multiple(() =>
            {
                Assert.IsTrue(status);
                Assert.AreEqual(expectedString, result);
            });
        }

        [Description("Test the SaveAsStream method with open stream (save to stream synchronous)")]
        [Test]
        public void SaveAsStreamTest2()
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
        public void SaveAsStreamTest3()
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

        [Description("Test the SaveAsStreamAsync method with terminating stream (save to stream asynchronous)")]
        [Test]
        public async Task SaveAsStreamAsyncTest()
        {
            string file = GetTempFileName();
            string expectedString, result;
            bool status;
            XmlDocument doc = GetSampleDocument(out expectedString);
            await Task.Run(async () =>
            {
                using (FileStream fs = new FileStream(file, FileMode.Create))
                {
                    await doc.SaveAsStreamAsync(fs);
                }
            });
            result = ReadAndDeleteTestFile(file, 1000, out status);
            Assert.Multiple(() =>
            {
                Assert.IsTrue(status);
                Assert.AreEqual(expectedString, result);
            });
        }

        [Description("Test the SaveAsStreamAsync method with open stream (save to stream asynchronous)")]
        [Test]
        public async Task SaveAsStreamAsyncTest2()
        {
            string expectedString;
            XmlDocument doc = GetSampleDocument(out expectedString);
            MemoryStream ms = new MemoryStream();
            await Task.Run(async () =>
            {
                await doc.SaveAsStreamAsync(ms, true);
            });
            ms.Position = 0;
            StreamReader sr = new StreamReader(ms);
            string result = sr.ReadToEnd();
            Assert.AreEqual(result, expectedString);
        }

        [Description("Test the SaveAsStreamAsync method with all parameters (save to stream asynchronous)")]
        [Test]
        public async Task SaveAsStreamAsyncTest3()
        {
            string expectedString;
            XmlDocument doc = GetSampleDocument(out expectedString);
            MemoryStream ms = new MemoryStream();
            await Task.Run(async () =>
            {
                await doc.SaveAsStreamAsync(ms, Encoding.ASCII, 1024, true);
            });
            ms.Position = 0;
            StreamReader sr = new StreamReader(ms);
            string result = sr.ReadToEnd();
            Assert.AreEqual(result, expectedString);
        }

        [Description("Test the Save method with filename (save to file synchronous)")]
        [Test]
        public void SaveTest()
        {
            string expectedString, result;
            bool status;
            string file = GetTempFileName();
            XmlDocument doc = GetSampleDocument(out expectedString);
            doc.Save(file);
            result = ReadAndDeleteTestFile(file, 1000, out status);
            Assert.Multiple(() =>
            {
                Assert.IsTrue(status);
                Assert.AreEqual(expectedString, result);
            });       
        }

        [Description("Test the Save method with all parameters (save to file synchronous)")]
        [Test]
        public void SaveTest2()
        {
            string expectedString, result;
            bool status;
            string file = GetTempFileName();
            XmlDocument doc = GetSampleDocument(out expectedString);
            doc.Save(file, Encoding.UTF8, 512);
            result = ReadAndDeleteTestFile(file, 1000, out status);
            Assert.Multiple(() =>
            {
                Assert.IsTrue(status);
                Assert.AreEqual(expectedString, result);
            });
        }

        [Description("Test the SaveAsync method with filename (save to file asynchronous)")]
        [Test]
        public async Task SaveAsyncTestAsync()
        {
            string expectedString, result;
            bool status;
            string file = GetTempFileName();
            XmlDocument doc = GetSampleDocument(out expectedString);
            await Task.Run(async () =>
            {
                await doc.SaveAsync(file);
            });
            result = ReadAndDeleteTestFile(file, 1000, out status);
            Assert.Multiple(() =>
            {
                Assert.IsTrue(status);
                Assert.AreEqual(expectedString, result);
            });
        }


        [Description("Test the SaveAsync method with all parameters (save to file asynchronous)")]
        [Test]
        public async Task SaveAsynxTest2Async()
        {
            string expectedString, result;
            bool status;
            string file = GetTempFileName();
            XmlDocument doc = GetSampleDocument(out expectedString);
            await Task.Run(async () =>
            {
                await doc.SaveAsync(file, Encoding.UTF8, 512);
            });
            result = ReadAndDeleteTestFile(file, 1000, out status);
            Assert.Multiple(() =>
            {
                Assert.IsTrue(status);
                Assert.AreEqual(expectedString, result);
            });
        }


        private string ReadAndDeleteTestFile(string fileName, int delay, out bool success)
        {
            System.Threading.Thread.Sleep(delay); // Not good, but easy (wait for IO)
            string result = null;
            try
            {
            using (StreamReader sr = new StreamReader(fileName))
            {
                result = sr.ReadToEnd();
            }
                File.Delete(fileName);
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                success = false;
            }
            return result;
        }

        private string GetTempFileName()
        {
            return Path.GetTempPath() + "saveTest_" + DateTime.Now.ToString("YYYYMMddhhmmssffff") + ".xml";
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
