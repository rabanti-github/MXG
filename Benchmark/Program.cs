/*
 * MXG - Minimalist XML Generator
 * Copyright Raphael Stoeckli © 2019
 * This library is licensed under the MIT License.
 * You find a copy of the license in project folder or on: http://opensource.org/licenses/MIT
 */

using MXG.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dummy
{
    /// <summary>
    /// This class is only intended for performance profiling and other test-like checks
    /// </summary>
    class Program
    {

        private const string AZ_CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private const string ASCII_CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789<>.,;:\\/()[]{}!?'@\"+3#%&|=´~`^^$£§°";
        private static int AZ_CHARS_LENGTH = AZ_CHARS.Length - 1;
        private static int ASCII_CHARS_LENGTH = ASCII_CHARS.Length - 1;

        static void Main(string[] args)
        {
            TestMethod();
        }

        public static void TestMethod()
        {
            Console.WriteLine("MXG Benchmark\n-------------");
            bool cancel;
            int numberOfIterations = GetConsoleValue("Number of benchmark iterations?", out cancel);
            if (cancel == true)
            {
                Console.WriteLine("Benchmark canceled");
                return;
            }
            int numberOfElements = GetConsoleValue("Number of XML elements?", out cancel);
            if (cancel == true)
            {
                Console.WriteLine("Benchmark canceled");
                return;
            }
            int sizeOfElement = GetConsoleValue("Length of elements in characters?", out cancel);
            if (cancel == true)
            {
                Console.WriteLine("Benchmark canceled");
                return;
            }
            int numberOfAttributes = GetConsoleValue("Number of attributes per element?", out cancel);
            if (cancel == true)
            {
                Console.WriteLine("Benchmark canceled");
                return;
            };
            int sizeOfAttribute = GetConsoleValue("Length of attributes in characters?", out cancel);
            if (cancel == true)
            {
                Console.WriteLine("Benchmark canceled");
                return;
            }
            int sizeOfAttributeContent = GetConsoleValue("Length of attribute content in characters?", out cancel);
            if (cancel == true)
            {
                Console.WriteLine("Benchmark canceled");
                return;
            }

            double tSum1 = 0;
            double tSum2 = 0;
            int numberOfActualIterations = 0;
            List<string> elements = new List<string>(numberOfElements);
            List<string> attributeNames = new List<string>(numberOfAttributes);
            List<string> attributeValues = new List<string>(numberOfAttributes);
            Random rnd = new Random(DateTime.Now.Millisecond);

            for (int n = 0; n < numberOfIterations; n++)
            {
                Console.WriteLine("Iteration " + n + 1);
                elements.Clear();
                attributeNames.Clear();
                attributeValues.Clear();
                for(int i = 0; i < numberOfElements; i++)
                {
                    elements.Add(GetRandomString(sizeOfElement, false, rnd));
                }
                for (int i = 0; i < numberOfAttributes; i++)
                {
                    attributeNames.Add(GetRandomString(sizeOfAttribute, false, rnd));
                    attributeValues.Add(GetRandomString(sizeOfAttributeContent, true, rnd));
                }
                try
                {
                    double t1 = MXGapproach(numberOfElements, numberOfAttributes, elements, attributeNames, attributeValues);
                    double t2 = MSXMLapproach(numberOfElements, numberOfAttributes, elements, attributeNames, attributeValues);
                    tSum1 += t1;
                    tSum2 += t2;
                    numberOfActualIterations++;
                }
                catch(Exception ex)
                {
                    Console.WriteLine("One iteration could not be executed and will be skipped: " + ex.Message);
                }

                //Console.WriteLine(t1 + " ms for MXG approach");
                //Console.WriteLine(t2 + " ms for MSXML approach");

            }
            Console.WriteLine("----------------------------------------\nBenchmark finished...");
            Console.WriteLine("Number of planned iterations: " + numberOfIterations);
            Console.WriteLine("Number of executed iterations: " + numberOfActualIterations);
            Console.WriteLine("MSXML approach: Total: " + tSum2 + " ms (" + tSum2 / ((double)numberOfActualIterations) + " ms avg.)");
            Console.WriteLine("MXG approach:   Total: " + tSum1 + " ms (" + tSum1 / ((double)numberOfActualIterations) + " ms avg.)");
            Console.WriteLine("MXG performance factor: " + tSum2 / tSum1 + " (f>1 = faster than MSXML)");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static string GetRandomString(int length, bool fullAscii, Random instance)
        {
            StringBuilder sb = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                if (fullAscii)
                {
                    sb.Append(ASCII_CHARS[instance.Next(0, ASCII_CHARS_LENGTH)]);
                }
                else
                {
                    sb.Append(AZ_CHARS[instance.Next(0, AZ_CHARS_LENGTH)]);
                }
            }
            string r = sb.ToString();
            if (length >2 && (r[0] == 'x'|| r[0] == 'X')&& (r[1] == 'm' || r[1] == 'M')&& (r[2] == 'l' || r[2] == 'L')) // This can lead to many errors
            {
                return GetRandomString(length, fullAscii, instance);
            }
            else
            {
                return r;
            }
        }

        private static int GetConsoleValue(string message, out bool cancel)
        {
            int result;
            while (true)
            {
                Console.WriteLine(message);
                Console.WriteLine("Insert a number to continue or 'x' to cancel...");
                string value = Console.ReadLine();
                if (value.ToLower() == "x")
                {
                    cancel = true;
                    break;
                }
                if (int.TryParse(value, out result))
                {
                    cancel = false;
                    return result;
                }
                else
                {
                    Console.WriteLine("Invalid value. Try again...");
                }
            }
            return -1;
        }

        private static double MXGapproach(int numberOfElements, int numerOfAttributes, List<string>elemetNames, List<string>attributeNames, List<string>attributeValues)
        {
            DateTime now = DateTime.Now;
            XmlDocument doc = new XmlDocument(numberOfElements);
            XmlElement root = doc.AddChild("root");
            for (int i = 0; i < numberOfElements; i++)
            {
                XmlElement element = new XmlElement(elemetNames[i], null, null, 0, numerOfAttributes);
                for (int j = 0; j < numerOfAttributes; j++)
                {
                    element.AddAttribute(attributeNames[j], attributeValues[j], true, false);
                   // element.AddAttribute(attributeNameTemplate + j, attributeValueTemplate, true, false);
                }
                root.AddChild(element);
            }
            String output = doc.GetXmlString();
            TimeSpan t = DateTime.Now - now;
            return t.TotalMilliseconds;
        }

        private static double MSXMLapproach(int numberOfElements, int numerOfAttributes, List<string> elemetNames, List<string> attributeNames, List<string> attributeValues)
        {
            DateTime now = DateTime.Now;
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            System.Xml.XmlDeclaration header = doc.CreateXmlDeclaration("1.0", "UTF-8", "yes");
            System.Xml.XmlElement root = doc.DocumentElement;
            doc.InsertBefore(header, root);
            System.Xml.XmlElement rootElement = doc.CreateElement(string.Empty, "root", string.Empty);
            doc.AppendChild(rootElement);
            for (int i = 0; i < numberOfElements; i++)
            {

                System.Xml.XmlElement element = doc.CreateElement(string.Empty, elemetNames[i], string.Empty);
                for (int j = 0; j < numerOfAttributes; j++)
                {
                    //element.SetAttribute(attributeNameTemplate + j, attributeValueTemplate);
                    element.SetAttribute(attributeNames[j], attributeValues[j]);
                }
                rootElement.AppendChild(element);
            }
            string output = doc.OuterXml;
            TimeSpan t = DateTime.Now - now;
            return t.TotalMilliseconds;
        }
    }
}
