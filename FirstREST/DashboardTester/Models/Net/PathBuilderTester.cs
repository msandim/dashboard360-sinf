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

            builder.BasePath = PathConstants.BasePathAPIPrimavera;
            builder.InitialDate = new DateTime(2014, 5, 10);
            builder.FinalDate = new DateTime(2015, 10, 13);
            builder.DocumentType = "Testing";

            String actual = builder.Build().ToString();
            String expected = PathConstants.BasePathAPIPrimavera + "?initialDate=2014-05-10&finalDate=2015-10-13&documentType=Testing";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestPathBuilder2()
        {
            PathBuilder builder = new PathBuilder();

            builder.BasePath = PathConstants.BasePathAPIPrimavera;
            builder.InitialDate = new DateTime(2014, 5, 10);
            builder.FinalDate = new DateTime(2015, 10, 13);

            String actual = builder.Build().ToString();
            String expected = PathConstants.BasePathAPIPrimavera + "?initialDate=2014-05-10&finalDate=2015-10-13";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestPathBuilder3()
        {
            String actual = PathBuilder.Build(PathConstants.BasePathAPIPrimavera, "action", new DateTime(2014, 5, 10), new DateTime(2015, 10, 13), "type").ToString();
            String expected = PathConstants.BasePathAPIPrimavera + "/action?initialDate=2014-05-10&finalDate=2015-10-13&documentType=type";

            Assert.AreEqual(expected, actual);
        }
    }
}
