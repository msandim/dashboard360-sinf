using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dashboard.Models.Net;

namespace DashboardTester.Models.Net
{
    [TestClass]
    public class PathBuilderTester
    {
        [TestMethod]
        public void TestPathBuilder()
        {
            var builder = new PathBuilder
            {
                BasePath = PathConstants.BasePathApiPrimavera,
                InitialDate = new DateTime(2014, 5, 10),
                FinalDate = new DateTime(2015, 10, 13),
                DocumentType = "Testing"
            };


            var actual = builder.Build().ToString();
            var expected = PathConstants.BasePathApiPrimavera + "?initialDate=2014-05-10&finalDate=2015-10-13&documentType=Testing";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestPathBuilder2()
        {
            var builder = new PathBuilder
            {
                BasePath = PathConstants.BasePathApiPrimavera,
                InitialDate = new DateTime(2014, 5, 10),
                FinalDate = new DateTime(2015, 10, 13)
            };

            var actual = builder.Build().ToString();
            var expected = PathConstants.BasePathApiPrimavera + "?initialDate=2014-05-10&finalDate=2015-10-13";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestPathBuilder3()
        {
            var actual = PathBuilder.Build(PathConstants.BasePathApiPrimavera, "action", new DateTime(2014, 5, 10), new DateTime(2015, 10, 13), "type").ToString();
            var expected = PathConstants.BasePathApiPrimavera + "/action?initialDate=2014-05-10&finalDate=2015-10-13&documentType=type";

            Assert.AreEqual(expected, actual);
        }
    }
}
