/*
 * MXG - Minimalist XML Generator
 * Copyright Raphael Stoeckli © 2019
 * This library is licensed under the MIT License.
 * You find a copy of the license in project folder or on: http://opensource.org/licenses/MIT
 */
using MXG.Core;
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
            int elementCounter = 2500;
            int attributeCounter = 1;
            string elementNameTemplate = "xyz";
            string attributeNameTemplate = "asd";
            string attributeValueTemplate = "xxx";
            double tSum1 = 0;
            double tSum2 = 0;
            int iterations = 1;

            for (int n = 0; n < iterations; n++)
            {
                double t1 = MXGapproach(elementCounter, elementCounter, elementNameTemplate, attributeNameTemplate, attributeValueTemplate);
                //double t2 = 0;
                double t2 = MSXMLapproach(elementCounter, attributeCounter, elementNameTemplate, attributeNameTemplate, attributeValueTemplate);
                tSum1 += t1;
                tSum2 += t2;
                //Console.WriteLine(t1 + " ms for MXG approach");
                //Console.WriteLine(t2 + " ms for MSXML approach");
                
            }
            Console.WriteLine("Number of iterations: " + iterations);
            Console.WriteLine("System.Xml approach: Total: " + tSum2 + " ms (" + tSum2 / ((double)iterations) + " ms avg.)");
            Console.WriteLine("MXG approach:   Total: " + tSum1 + " ms (" + tSum1 / ((double)iterations) + " ms avg.)");
            Console.WriteLine("MXG performance factor: " + tSum2 / tSum1 + " (f>1 = faster than System.Xml)");
        }

        private double MXGapproach(int elementCounter, int attributeCounter, string elementNameTemplate, string attributeNameTemplate, string attributeValueTemplate)
        {
            DateTime now = DateTime.Now;
            XmlDocument doc = new XmlDocument(elementCounter);
            XmlElement root = doc.AddChild("root");
            for (int i = 0; i < elementCounter; i++)
            {
                XmlElement element = new XmlElement(elementNameTemplate, null, null, 0, attributeCounter);
                for (int j = 0; j < attributeCounter; j++)
                {
                    element.AddAttribute(attributeNameTemplate + j, attributeValueTemplate, true, false);
                }
                root.AddChild(element);
            }
            String output = doc.GetXmlString();
            TimeSpan t = DateTime.Now - now;
            return t.TotalMilliseconds;
        }

        private double MSXMLapproach(int elementCounter, int attributeCounter, string elementNameTemplate, string attributeNameTemplate, string attributeValueTemplate)
        {
            
            DateTime now = DateTime.Now;
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            System.Xml.XmlDeclaration header = doc.CreateXmlDeclaration("1.0", "UTF-8", "yes");
            System.Xml.XmlElement root = doc.DocumentElement;
            doc.InsertBefore(header, root);
            System.Xml.XmlElement rootElement = doc.CreateElement(string.Empty, "root", string.Empty);
            doc.AppendChild(rootElement);
            for (int i = 0; i < elementCounter; i++)
            {

                System.Xml.XmlElement element = doc.CreateElement(string.Empty, elementNameTemplate, string.Empty);
                for (int j = 0; j < attributeCounter; j++)
                {
                    element.SetAttribute(attributeNameTemplate + j, attributeValueTemplate);
                }
                rootElement.AppendChild(element);
            }
            string output = doc.OuterXml;
            TimeSpan t = DateTime.Now - now;
            return t.TotalMilliseconds;
        }
    }
}
