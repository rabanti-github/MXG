using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXG.Core
{
    /// <summary>
    /// Static class to provide invariant values that are used multiple times by other classes 
    /// </summary>
    public static class Constants
    {
        public const int DEFAULT_STRING_BUILDER_SIZE = 512;

        public const char SANITIZING_CHAR = ' ';
        public const char TAG_END_CHAR = '>';
        public const char TAG_START_CHAR = '<';
        public const char NAMESPACE_DELIMITER = ':';
        public const char EMPTY_CHAR = ' ';

        public const string ESCAPE_LT = "&lt;";
        public const string ESCAPE_GT = "&gt;";
        public const string ESCAPE_AMP = "&amp;";
        public const string ESCAPE_QUOT = "&quot;";
        public const string EQUAL_TOKEN = "=\"";
        public const string DEFAULT_XML_DECLARATION = "<?xml version=\"1.0\"";
        public const string ENCODING_NAME = " encoding=\"";
        public const string STANDALONE_NAME_YES = " standalone=\"yes\"";
        public const string STANDALONE_NAME_NO = " standalone=\"no\"";
        public const string CLOSING_QUOT = "\"";
        public const string XML_TERMINATOR = "?>";
        public const string UTF8_NAME = "UTF-8";
        public const string EMPTY_TAG_TERMINATOR = "/>";
        public const string TAG_TERMINATOR = "</";
        public const string XMLNS_NAME = "xmlns";
        public const string TYPES_NAME = "Types";
        public const string DEFAULT_NAME = "Default";
        public const string OVERRIDE_NAME = "Override";
        public const string CONTENT_TYPE_NAME = "ContentType";
        public const string EXTENSION_NAME = "Extension";
        public const string ID_NAME = "Id";
        public const string TYPE_NAME = "Type";
        public const string TARGET_NAME = "Target";
        public const string RELATIONSHIPS_NAME = "Relationships";
        public const string RELATIONSHIP_NAME = "Relationship";
    }
}
