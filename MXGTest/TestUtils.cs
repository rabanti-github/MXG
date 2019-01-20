/*
 * MXG - Minimalist XML Generator
 * Copyright Raphael Stoeckli © 2019
 * This library is licensed under the MIT License.
 * You find a copy of the license in project folder or on: http://opensource.org/licenses/MIT
 */

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
        /// Splits a string into tokens
        /// </summary>
        /// <param name="input">Input value</param>
        /// <param name="delimiter">Split delimiter</param>
        /// <returns>string array. If null was passed, an empty array is returned</returns>
        public static string[] SplitValues(string input, char delimiter)
        {
            if (input == null)
            {
                return new string[0];
            }
            return input.Split(delimiter);
        }

    }
}
