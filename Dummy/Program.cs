/*
 * MXG - Minimalist XML Generator
 * Copyright Raphael Stoeckli © 2019
 * This library is licensed under the MIT License.
 * You find a copy of the license in project folder or on: http://opensource.org/licenses/MIT
 */

using MXG.Core;
using System;

namespace Dummy
{
    /// <summary>
    /// This class is only intended for performance profiling and other test-like checks
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            for (int n = 0; n < 500; n++)
            {
                DateTime now = DateTime.Now;
                XmlDocument doc = new XmlDocument();
                for (int i = 0; i < 500; i++)
                {
                    XmlElement element = new XmlElement("xyz");
                    for (int j = 0; j < 100; j++)
                    {
                        element.AddAttribute("asd" + j, "xxx", true, false);
                    }
                    doc.AddChild(element);
                }
                String output = doc.GetXmlString();
                TimeSpan t = DateTime.Now - now;
                System.Diagnostics.Debug.WriteLine(t.TotalMilliseconds);
                //Console.WriteLine(t.TotalMilliseconds + " ms");
                
            }

        }
    }
}
