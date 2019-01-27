using MXG.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXGTest
{
    [TestFixture, Description("Test class for XmlException class")]
    public class XmlExceptionTest
    {
        [Description("Default exception test")]
        [TestCase("error message", false, "error message", false)]
        [TestCase(null, false, null, false)]
        [TestCase("error message", true, "error message", true)]
        public void ConstructorTest(string message, bool createInnerException, string expected, bool expectedInnerException)
        {
            XmlException exp;
            if (message == null)
            {
                exp = new XmlException();
            }
            else
            {
                if (createInnerException)
                {
                    exp = new XmlException(message, new Exception());
                }
                else
                {
                    exp = new XmlException(message);
                }
            }
            if (expected != null)
            {
                Assert.Multiple(() =>
                {
                    Assert.AreEqual(exp.Message, expected);
                    Assert.AreEqual(this.hasInnerException(exp), expectedInnerException);
                });
            }
            else
            {
                Assert.Multiple(() =>
                {
                    Assert.NotNull(exp.Message);
                    Assert.AreEqual(this.hasInnerException(exp), expectedInnerException);
                });
            }

        }

        private bool hasInnerException(XmlException exp)
        {
            if (exp.InnerException == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}
