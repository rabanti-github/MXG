using MXG.Relationships;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXGTest.Relationships
{
    [TestFixture, Description("Test class for XmlContentTypes class")]
    class XmlContentTypesTest
    {

        [Description("Test the constructor of the XmlContentTypes class with the available properties")]
        [TestCase( "NS1", 10, "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><Types xmlns=\"NS1\"/>")]
        [TestCase("", 0, "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><Types xmlns=\"\"/>")]
        [TestCase(null, -5, "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><Types xmlns/>")]
        public void ConstructorTest(string nameSpace, int estimatedElements, string expectedString)
        {
            XmlContentTypes types = new XmlContentTypes(nameSpace, estimatedElements);

            Assert.Multiple(() => {
                Assert.IsNotNull(types);
                Assert.AreEqual(types.GetXmlString(), expectedString);
            });
        }

        [Description("Test the AddDefaultContentType method")]
        public void AddDefaultContentTypeTest(string contentType, string extension)
        {
            XmlContentTypes types = new XmlContentTypes("NS1");
            types.AddDefaultContentType(contentType, extension);

        }



    }
}
