/*
 * MXG - Minimalist XML Generator
 * Copyright Raphael Stoeckli © 2019
 * This library is licensed under the MIT License.
 * You find a copy of the license in project folder or on: http://opensource.org/licenses/MIT
 */
using MXG;
using NUnit.Framework;
using System;
using System.IO;

namespace MXGTest
{
    [TestFixture]
    public class PerformanceTest
    {
        [Test]
        public void TestMethod()
        {
            for (int n = 0; n < 1; n++)
            {
                DateTime now = DateTime.Now;
                XmlDocument doc = new XmlDocument(50);
                for (int i = 0; i < 50; i++)
                {
                    XmlElement element = new XmlElement("xyz", null, null, 0, 100);
                    for (int j = 0; j < 100; j++)
                    {
                        element.AddAttribute("asd" + j, "xxx",true, false);
                    }
                    doc.AddChild(element);
                }
                String output = doc.GetXmlString();
                TimeSpan t = DateTime.Now - now;
                Console.WriteLine(t.TotalMilliseconds + " ms");
                Console.WriteLine(System.Text.ASCIIEncoding.Unicode.GetByteCount(output) + " bytes");

                now = DateTime.Now;
                using (MemoryStream ms = new MemoryStream()) // Write workbook.xml
                {
                    if (ms.CanWrite == false) { return; }
                    using (System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(ms))
                    {
                        writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"");
                        writer.WriteRaw(output);
                        writer.Flush();
                    }

                    t = DateTime.Now - now;
                    Console.WriteLine(t.TotalMilliseconds + " ms for MSXML approach");
                }
            }
        }
    }
}
