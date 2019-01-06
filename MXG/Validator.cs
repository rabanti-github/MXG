/*
 * MXG - Minimalist XML Generator
 * Copyright Raphael Stoeckli © 2019
 * This library is licensed under the MIT License.
 * You find a copy of the license in project folder or on: http://opensource.org/licenses/MIT
 */

using System.Collections.Generic;
using System.Text;

namespace MXG
{
    public class Validator
    {
        private const string ESCAPE_LT = "&lt;";
        private const string ESCAPE_GT = "&gt;";
        private const string  ESCAPE_AMP = "&amp;";
        private const string ESCAPE_QUOT = "&quot;";
        private const char SANITIZING_CHAR = ' ';

        /// <summary>
        /// Method to escape XML characters in an XML attribute
        /// </summary>
        /// <param name="input">Input string to process</param>
        /// <returns>Escaped string</returns>
        public static string EscapeXmlAttributeChars(string input)
        {
            return Validator.EscapeXml(input, true);
        }

        /// <summary>
        /// Method to escape XML characters between two XML tags
        /// </summary>
        /// <param name="input">Input string to process</param>
        /// <returns>Escaped string</returns>
        /// <remarks>Note: The XML specs allow characters up to the character value of 0x10FFFF. However, the C# char range is only up to 0xFFFF. PicoXLSX will neglect all values above this level in the sanitizing check. Illegal characters like 0x1 will be replaced with a white space (0x20)</remarks>
        public static string EscapeXmlChars(string input)
        {
            return Validator.EscapeXml(input, false);
        }

        /// <summary>
        /// Method to validate the name of an XML element
        /// </summary>
        /// <param name="name">Name to validate</param>
        /// <returns>True if the name is valid, otherwise false</returns>
        /// <remarks>A name that starts with any kind of lower/upper case combination of 'XML' will be treated as invalid, since this is a reserved term, according to the XML specifications</remarks>
        public static bool ValidateElementName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }
            if (!Validator.ValidateXmlName(name))
            {
                return false;
            }
            if (name.Length >= 3)
            {
                if ((name[0] == 'x' || name[0] == 'X') && (name[1] == 'm' || name[1] == 'M') && (name[2] == 'l' || name[2] == 'L'))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Method to validate a XML attribute name regarding
        /// </summary>
        /// <param name="name">Name to check</param>
        /// <returns>True if the name is valid, otherwise false</returns>
        public static bool ValidateAttributeName(string name)
        {
            return Validator.ValidateXmlName(name);
        }


        /// <summary>
        /// Method to validate a XML element, name space or attribute name regarding general validity
        /// </summary>
        /// <param name="name">Name to check</param>
        /// <returns>True if the name is valid, otherwise false</returns>
        private static bool ValidateXmlName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }
            if (!Validator.ValidateStartCharacter(name[0])) // First character
            {
                return false;
            }
            int len = name.Length;
            for(int i = 1; i < len; i++)
            {
                if (!Validator.ValidateStartCharacter(name[i]))
                {   // Check additional, allowed characters after first character
                    if (!(name[i] == 0x2D ||    // -
                        name[i] == 0x2E ||      // .
                        name[i] == 0xB7 ||      // mid dot
                        (name[i] >= 0x30 && name[i] <= 0x39) ||  // 0-9
                        (name[i] >= 0x300 && name[i] <= 0x036F) ||
                        (name[i] >= 0x203F && name[i] <= 0x2040)
                        ))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Method to escape XML characters between two XML tags
        /// </summary>
        /// <param name="input">Input string to process</param>
        /// <returns>Escaped string</returns>
        /// <remarks>Note: The XML specs allow characters up to the character value of 0x10FFFF. However, the C# char range is only up to 0xFFFF. PicoXLSX will neglect all values above this level in the sanitizing check. Illegal characters like 0x1 will be replaced with a white space (0x20)</remarks>
        private static string EscapeXml(string input, bool escapeQuotes)
        {
            if (input == null) { return ""; }
            int len = input.Length;
            List<ValidatorTuple> invalidCharacters = new List<ValidatorTuple>(len);
            int i;
            for (i = 0; i < len; i++)
            {
                if ((input[i] < 0x9) || 
                    (input[i] > 0xA && input[i] < 0xD) || 
                    (input[i] > 0xD && input[i] < 0x20) || 
                    (input[i] > 0xD7FF && input[i] < 0xE000) || 
                    (input[i] > 0xFFFD))
                {
                    invalidCharacters.Add(new ValidatorTuple(i, ValidatorTuple.CaracterType.IllegalCharacter));
                    continue;
                } // Note: XML specs allow characters up to 0x10FFFF. However, the C# char range is only up to 0xFFFF; Higher values are neglected here 
                if (input[i] == 0x3C) // <
                {
                    invalidCharacters.Add(new ValidatorTuple(i, ValidatorTuple.CaracterType.Lt));
                }
                else if (input[i] == 0x3E) // >
                {
                    invalidCharacters.Add(new ValidatorTuple(i, ValidatorTuple.CaracterType.Gt));
                }
                else if (input[i] == 0x26) // &
                {
                    invalidCharacters.Add(new ValidatorTuple(i, ValidatorTuple.CaracterType.Amp));
                }
                else if (escapeQuotes && input[i] == '"')
                {
                    invalidCharacters.Add(new ValidatorTuple(i, ValidatorTuple.CaracterType.Quot));
                }
            }
            if (invalidCharacters.Count == 0)
            {
                return input;
            }

            StringBuilder sb = new StringBuilder(len);
            int lastIndex = 0;
            len = invalidCharacters.Count;
            for (i = 0; i < len; i++)
            {
                sb.Append(input.Substring(lastIndex, invalidCharacters[i].character - lastIndex));
                switch(invalidCharacters[i].characterType)
                {
                    case ValidatorTuple.CaracterType.IllegalCharacter:
                        sb.Append(Validator.SANITIZING_CHAR); // Whitespace as fall back on illegal character
                        break;
                    case ValidatorTuple.CaracterType.Lt:  // replace <
                        sb.Append(Validator.ESCAPE_LT);
                        break;
                    case ValidatorTuple.CaracterType.Gt: // replace >
                        sb.Append(Validator.ESCAPE_GT);
                        break;
                    case ValidatorTuple.CaracterType.Amp: // replace &
                        sb.Append(Validator.ESCAPE_AMP);
                        break;
                    case ValidatorTuple.CaracterType.Quot: // replace "
                        sb.Append(Validator.ESCAPE_QUOT);
                        break;
                }
                lastIndex = invalidCharacters[i].character + 1;
            }
            sb.Append(input.Substring(lastIndex));
            return sb.ToString();
        }

        /// <summary>
        /// Private method to validate the start character of an XML entity (element or attribute)
        /// </summary>
        /// <param name="c">Character to check</param>
        /// <returns>True if the character is valid, otherwise false</returns>
        private static bool ValidateStartCharacter(char c)
        {
            if (c == 0x3A ||                    // :
                c == 0x5F ||                    // _
                (c >= 0x41 && c <= 0x5A) ||     // A-Z
                (c >= 0x61 && c <= 0x7A) ||     // a-z
                (c >= 0xC0 && c <= 0xD6) ||
                (c >= 0xD8 && c <= 0xF6) ||
                (c >= 0xF8 && c <= 0x2FF) ||
                (c >= 0x370 && c <= 0x37D) ||
                (c >= 0x37F && c <= 0x1FFF) ||
                (c >= 0x200C && c <= 0x200D) ||
                (c >= 0x2070 && c <= 0x218F) ||
                (c >= 0x2C00 && c <= 0x2FEF) ||
                (c >= 0x3001 && c <= 0xD7FF) ||
                (c >= 0xF900 && c <= 0xFDCF) ||
                (c >= 0xFDF0 && c <= 0xFFFD) ||
                (c >= 0x10000 && c <= 0xEFFFF))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Struct to handle the validation of XML content
        /// </summary>
        private struct ValidatorTuple
        {
            /// <summary>
            /// Type of the character issue
            /// </summary>
            public enum CaracterType
            {
                /// <summary>
                /// The character is not allowed as XML content
                /// </summary>
                IllegalCharacter,
                /// <summary>
                /// The character represent a less than sign (&lt;), which must be escaped as &amp;lt;
                /// </summary>
                Lt,
                /// <summary>
                /// The character represent a greater than sign (&gt;), which must be escaped as &amp;gt;
                /// </summary>
                Gt,
                /// <summary>
                /// The character represent an ampersand (&amp;), which must be escaped as &amp;amp;
                /// </summary>
                Amp,
                /// <summary>
                /// The character represent an double quote ("), which must be escaped as &amp;quot;
                /// </summary>
                Quot,
                /// <summary>
                /// An undefined / default character
                /// </summary>
                Undefined,
            }

            /// <summary>
            /// Character that is validated
            /// </summary>
            public int character;
            /// <summary>
            /// Issue type of the validated character
            /// </summary>
            public CaracterType characterType;

            /// <summary>
            /// Initializes a new instance of the <see cref="ValidatorTuple"/> struct.
            /// </summary>
            /// <param name="character">Character to validate</param>
            /// <param name="characterType">Issue type of the character</param>
            public ValidatorTuple(int character, CaracterType characterType)
            {
                this.character = character;
                this.characterType = characterType;
            }
        }
    }
}
