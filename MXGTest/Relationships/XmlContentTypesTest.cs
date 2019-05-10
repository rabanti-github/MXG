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
        [TestCase("NS1", 10, "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><Types xmlns=\"NS1\"/>")]
        [TestCase("", 0, "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><Types xmlns=\"\"/>")]
        [TestCase(null, -5, "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><Types xmlns/>")]
        public void ConstructorTest(string nameSpace, int estimatedElements, string expectedString)
        {
            XmlContentTypes types = new XmlContentTypes(nameSpace, estimatedElements);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(types);
                Assert.AreEqual(types.GetXmlString(), expectedString);
            });
        }

        [Description("Test the AddDefaultContentType method")]
        [TestCase("CT1", "EXT1", "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><Types xmlns=\"NS1\"><Default ContentType=\"CT1\" Extension=\"EXT1\"/></Types>")]
        [TestCase(null, "EXT2", "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><Types xmlns=\"NS1\"><Default ContentType Extension=\"EXT2\"/></Types>")]
        [TestCase("CT2", null, "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><Types xmlns=\"NS1\"><Default ContentType=\"CT2\" Extension/></Types>")]
        [TestCase(null, null, "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><Types xmlns=\"NS1\"><Default ContentType Extension/></Types>")]
        [TestCase("", "", "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><Types xmlns=\"NS1\"><Default ContentType=\"\" Extension=\"\"/></Types>")]
        public void AddDefaultContentTypeTest(string contentType, string extension, string expectedString)
        {
            XmlContentTypes types = new XmlContentTypes("NS1");
            types.AddDefaultContentType(contentType, extension);
            Assert.Multiple(() =>
            {
                Assert.IsNotNull(types);
                Assert.AreEqual(types.GetXmlString(), expectedString);
            });
        }

        [Description("Test the AddOverrideContentType method")]
        [TestCase("CT1", "P1", "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><Types xmlns=\"NS1\"><Override ContentType=\"CT1\" Extension=\"P1\"/></Types>")]
        [TestCase(null, "P2", "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><Types xmlns=\"NS1\"><Override ContentType Extension=\"P2\"/></Types>")]
        [TestCase("CT2", null, "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><Types xmlns=\"NS1\"><Override ContentType=\"CT2\" Extension/></Types>")]
        [TestCase(null, null, "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><Types xmlns=\"NS1\"><Override ContentType Extension/></Types>")]
        [TestCase("", "", "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><Types xmlns=\"NS1\"><Override ContentType=\"\" Extension=\"\"/></Types>")]
        public void AddOverrideContentTypeTest(string contentType, string partName, string expectedString)
        {
            XmlContentTypes types = new XmlContentTypes("NS1");
            types.AddOverrideContentType(contentType, partName);
            Assert.Multiple(() =>
            {
                Assert.IsNotNull(types);
                Assert.AreEqual(types.GetXmlString(), expectedString);
            });
        }
    }
}
