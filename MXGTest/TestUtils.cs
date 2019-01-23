/*
 * MXG - Minimalist XML Generator
 * Copyright Raphael Stoeckli © 2019
 * This library is licensed under the MIT License.
 * You find a copy of the license in project folder or on: http://opensource.org/licenses/MIT
 */

using MXG.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXGTest
{
    /// <summary>
    /// Class for static test utils
    /// </summary>
    public static class TestUtils
    {
        /// <summary>
        /// Splits a string into two-dimensional tokens
        /// </summary>
        /// <param name="input">Input value</param>
        /// <param name="delimiter">Split delimiter for 1st dimension</param>
        /// <param name="subDelimiter">Split delimiter for 2nd dimension</param>
        /// <returns>string array. If null was passed, an empty array is returned</returns>
        public static string[][] SplitValues(string input, char delimiter, char subDelimiter)
        {
            return SplitValuesInternal(input, delimiter, subDelimiter);
        }
        /// <summary>
        /// Splits a string into one-dimensional tokens
        /// </summary>
        /// <param name="input">Input value</param>
        /// <param name="delimiter">Split delimiter</param>
        /// <returns>string array. If null was passed, an empty array is returned</returns>
        public static string[] SplitValues(string input, char delimiter)
        {
            return SplitValuesInternal(input, null, delimiter)[0];
        }

        /// <summary>
        /// Splits a string into one- or two-dimensional tokens
        /// </summary>
        /// <param name="input">Input value</param>
        /// <param name="delimiter">Split delimiter for 1st dimension</param>
        /// <param name="subDelimiter">Split delimiter for 2nd dimension</param>
        /// <returns>string array. If null was passed, an empty array is returned</returns>
        private static string[][] SplitValuesInternal(string input, char? delimiter, char subDelimiter)
        {
            if (input == null)
            {
                return new string[0][];
            }
            if (delimiter == null)
            {
                String[] line = input.Split(subDelimiter);
                return new string[1][] { line };
            }
            else
            {
                string[] lines = input.Split((char)delimiter);
                string[][] output = new string[lines.Length][];
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = (string)lines[i];
                    string[] lines2 = line.Split(subDelimiter);
                    output[i] = lines2;
                }
                return output;
            }
        }

        /// <summary>
        /// Gets an XML element by tokens
        /// </summary>
        /// <param name="tokens">Expected Token parts [0] name, [1] name space, [2] value</param>
        /// <returns>XmlElement or null if invalid tokens were passed</returns>
        public static XmlElement GetElement(string[] tokens)
        {
            XmlElement element;
            if (tokens.Length == 1)
            {
                element = new XmlElement(tokens[0]);
    }
            else if (tokens.Length == 2)
            {
                element = new XmlElement(tokens[0], tokens[1]);
}
            else if (tokens.Length == 3)
            {
                element = new XmlElement(tokens[0], tokens[1], tokens[2]);
            }
            else // Not applicable
            {
                return null;
            }
            return element;
        }

        /// <summary>
        /// Appends XML attributes to an XmlElement by tokens
        /// </summary>
        /// <param name="element">XmlElement reference</param>
        /// <param name="token">Attribute string to be tokenized</param>
        /// <param name="delimiter">Delimiter character</param>
        /// <param name="subdelimiter">Sub-Delimiter character</param>
        /// <param name="nullSurrogate">null surrogate string (will be replaced by null)</param>
        public static void AppendXmlAttributes(ref XmlElement element, string token, char delimiter, char subdelimiter, string nullSurrogate)
        {
            string[][] attributes = TestUtils.SplitValues(token, delimiter, subdelimiter);
            foreach (string[] attribute in attributes)
            {
                if (attribute[1] == nullSurrogate)
                {
                    element.AddAttribute(attribute[0]);
                }
                else
                {
                    element.AddAttribute(attribute[0], attribute[1]);
                }
            }
        }

    }
}
