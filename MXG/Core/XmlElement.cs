﻿/*
 * MXG - Minimalist XML Generator
 * Copyright Raphael Stoeckli © 2019
 * This library is licensed under the MIT License.
 * You find a copy of the license in project folder or on: http://opensource.org/licenses/MIT
 */

using System.Collections.Generic;
using System.Text;

namespace MXG.Core
{
    /// <summary>
    /// Class represents a concrete XML element, that is part of an XML document
    /// </summary>
    /// <seealso cref="MXG.Core.AbstractXmlElement" />
    public class XmlElement : AbstractXmlElement
    {
        private int estimatedChildCount;
        private int estimatedAttributeCount;

        /// <summary>
        /// Gets the children of the element
        /// </summary>
        /// <value>
        /// List of child elements
        /// </value>
        /// <remarks>To increase the performance, this list is only initialized if one or more child element(s) is added. Therefore, it is null until the first child is added</remarks>
        public List<XmlElement> Children { get; private set; } = null;

        /// <summary>
        /// Gets the attributes of the element
        /// </summary>
        /// <value>
        /// List of attributes
        /// </value>
        /// <remarks>To increase the performance, this list is only initialized if one or more attribute(s) is added. Therefore, it is null until the first attribute is added</remarks>
        public List<XmlAttribute> Attributes { get; private set; } = null;

        /// <summary>
        /// Gets the name space of the element
        /// </summary>
        /// <value>
        /// Name space (can be null)
        /// </value>
        public string NameSpace { get; private set; } = null;

        /// <summary>
        /// Gets whether a name space was defined for this element
        /// </summary>
        /// <value>
        /// True, if a name space was defined, otherwise false
        /// </value>
        public bool HasNameSpace { get; private set; } = false;

        /// <summary>
        /// Static method to create an element
        /// </summary>
        /// <param name="name">Name of the element</param>
        /// <param name="nameSpace">Name space of the element</param>
        /// <param name="skipNameCheck">If true, the name and name space will not be validated (optional, default: true)</param>
        /// <param name="content">Inner string content of the element (optional, default: null)</param>
        /// <param name="escapeContent">If true, the content will be escaped (optional, default: false)</param>
        /// <param name="estimatedChildCount">Estimated child count (for list initialization)</param>
        /// <param name="estimatedAttributeCount">Estimated attribute count (for list initialization)</param>
        /// <returns></returns>
        /// <exception cref="XmlException">
        /// The XML element name '" + name + "' contains invalid characters or is empty
        /// or
        /// The XML name space '" + name + "' contains invalid characters or is empty
        /// </exception>
        public static XmlElement CreateXmlElement(string name, string nameSpace, bool skipNameCheck = true, string content = null, bool escapeValue = false, int estimatedChildCount = 10, int estimatedAttributeCount = 10)
        {
            XmlElement element = new XmlElement(name, nameSpace,null, estimatedChildCount, estimatedAttributeCount, skipNameCheck);
            if (content != null)
            {
                element.SetContent(content, escapeValue);
            }
            return element;
        }

        
        /// <summary>
        /// Static method to create an element
        /// </summary>
        /// <param name="name">Name of the element</param>
        /// <param name="skipNameCheck">If true, the name will not be validated (optional, default: true)</param>
        /// <param name="content">Inner string content of the element (optional, default: null)</param>
        /// <param name="escapeContent">If true, the content will be escaped (optional, default: false)</param>
        /// <param name="estimatedChildCount">Estimated child count (optional; default: 10; for list initialization)</param>
        /// <param name="estimatedAttributeCount">Estimated attribute count (optional; default: 10; for list initialization)</param>
        /// <returns>XML element</returns>
        public static XmlElement CreateXmlElement(string name, bool skipNameCheck = true, string content = null, bool escapeContent = false, int estimatedChildCount = 10, int estimatedAttributeCount = 10)
        {
            return XmlElement.CreateXmlElement(name, null, skipNameCheck, content, escapeContent, estimatedChildCount, estimatedAttributeCount);
        }
        
        /// <summary>
        /// Constructor with parameters and name check
        /// </summary>
        /// <param name="name">Name of the element (tag name)</param>
        /// <param name="nameSpace">Optional name space</param>
        /// <param name="value">Optional value (text content)</param>
        /// <param name="estimatedAttributeCount">Estimated number of attributes</param>
        /// <param name="estimatedChildCount">Estimated number of child elements</param>
        /// <param name="skipNameCheck">If true, the name will not be validated (optional, default: true)</param>
        /// <exception cref="XmlException">An XML element must contain a valid name</exception>
        public XmlElement(string name, string nameSpace = null, string value = null, int estimatedChildCount = 10, int estimatedAttributeCount = 10, bool skipNameCheck = false) : base(name, value)
        {
            this.estimatedAttributeCount = estimatedAttributeCount;
            this.estimatedChildCount = estimatedChildCount;
            if (!skipNameCheck)
            {
                if (!Validator.ValidateElementName(name))
                {
                    throw new XmlException("The XML element name '" + name + "' contains invalid characters or is empty");
                }
                if (nameSpace != null)
                {
                    if (!Validator.ValidateElementName(nameSpace))
                    {
                        throw new XmlException("The XML name space '" + name + "' contains invalid characters or is empty");
                    }
                    this.NameSpace = nameSpace;
                    this.HasNameSpace = true;
                }
            }
            else
            {
                if (nameSpace != null)
                {
                    this.NameSpace = nameSpace;
                    this.HasNameSpace = true;
                }
            }
        }

        /// <summary>
        /// Adds a child element to the current element
        /// </summary>
        /// <param name="child">Element to add</param>
        public void AddChild(XmlElement child)
        {
            if (this.Children == null)
            {
                this.Children = new List<XmlElement>(this.estimatedChildCount);
            }
            this.Children.Add(child);
        }

        /// <summary>
        /// Adds an empty attribute to the element
        /// </summary>
        /// <param name="name">Name of the attribute</param>
        /// <param name="skipNameCheck">If true, the name will not be validated (default: true)</param>
        public void AddAttribute(string name, bool skipNameCheck = true)
        {
            if (this.Attributes == null)
            {
                this.Attributes = new List<XmlAttribute>(this.estimatedAttributeCount);
            }
            this.Attributes.Add(XmlAttribute.CreateXmlAttribute(name, "", skipNameCheck, false));
        }

        /// <summary>
        /// Adds an attribute with value to the element
        /// </summary>
        /// <param name="name">Name of the attribute</param>
        /// <param name="value">Value of the attribute</param>
        /// <param name="skipNameCheck">If true, the name will not be validated (default: true)</param>
        /// <param name="escapeValue">if true, the value will escaped (default: true)</param>
        public void AddAttribute(string name, string value, bool skipNameCheck = true, bool escapeValue = true)
        {
            if (this.Attributes == null)
            {
                this.Attributes = new List<XmlAttribute>(this.estimatedAttributeCount);
            }
            this.Attributes.Add(XmlAttribute.CreateXmlAttribute(name, value, skipNameCheck, escapeValue));
        }

        /// <summary>
        /// Method to append the current element with all sub-elements to the passed string builder
        /// </summary>
        /// <param name="builder">Document string builder</param>
        public override void AppendXmlString(StringBuilder builder)
        {
            builder.Append(Constants.TAG_START_CHAR);
            if (this.HasNameSpace)
            {
                builder.Append(this.NameSpace).Append(Constants.NAMESPACE_DELIMITER);
            }
            builder.Append(this.Name);
            int len, i;
            if (this.Attributes != null && this.Attributes.Count > 0)
            {
                len = this.Attributes.Count;
                for (i = 0; i < len; i++)
                {
                  //  builder.Append(Constants.EMPTY_CHAR);
                    this.Attributes[i].AppendXmlString(builder);
                }
            }
            if ((this.Children == null || this.Children.Count == 0) && this.IsEmpty)
            {
                builder.Append(Constants.EMPTY_TAG_TERMINATOR);
            }
            else
            {
                builder.Append(Constants.TAG_END_CHAR);
                if (!this.IsEmpty)
                {
                    builder.Append(this.Value);
                }
                if (this.Children != null)
                {
                    len = this.Children.Count;
                    if (len > 0)
                    {
                        for (i = 0; i < len; i++)
                        {
                            this.Children[i].AppendXmlString(builder);
                        }
                    }
                }
                builder.Append(Constants.TAG_TERMINATOR);
                if (this.HasNameSpace)
                {
                    builder.Append(this.NameSpace).Append(Constants.NAMESPACE_DELIMITER);
                }
                builder.Append(this.Name).Append(Constants.TAG_END_CHAR);
            }
        }

        /// <summary>
        /// Method to set the content of the element
        /// </summary>
        /// <param name="content">Content as string</param>
        /// <param name="escapeContent">If true, the element will be checked and escaped according to the XML specifications</param>
        public override void SetContent(string content, bool escapeContent = true)
        {
            if (escapeContent)
            {
                this.Value = Validator.EscapeXmlChars(content);
            }
            else
            {
                this.Value = content;
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            this.AppendXmlString(builder);
            return builder.ToString();
        }

    }
}
