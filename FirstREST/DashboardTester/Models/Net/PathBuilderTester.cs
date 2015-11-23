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
            PathBuilder builder = new PathBuilder();

            builder.InitialDate = new DateTime(2014, 5, 10);
            builder.FinalDate = new DateTime(2015, 10, 13);
            builder.DocumentType = "Testing";

            String actual = builder.Build().ToString();
            String expected = PathConstants.BASE_PATH_API_PRIMAVERA + "?initialDate=2014-05-10&finalDate=2015-10-13&documentType=Testing";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestPathBuilder2()
        {
            PathBuilder builder = new PathBuilder();

            builder.InitialDate = new DateTime(2014, 5, 10);
            builder.FinalDate = new DateTime(2015, 10, 13);

            String actual = builder.Build().ToString();
            String expected = PathConstants.BASE_PATH_API_PRIMAVERA + "?initialDate=2014-05-10&finalDate=2015-10-13";

            Assert.AreEqual(expected, actual);
        }
    }
}
