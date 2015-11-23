using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dashboard.Models.Net;

namespace DashboardTester.Models.Net
{
    [TestClass]
    public class PathTester
    {
        [TestMethod]
        public void TestPath()
        {
            Path path = PathConstants.BasePathAPIPrimavera;

            path.AddParameter("initialDate", "2015-01-02");
            path.AddParameter("finalDate", "2015-01-05");

            String actual = path.ToString();
            String expected = PathConstants.BasePathAPIPrimavera + "?initialDate=2015-01-02&finalDate=2015-01-05";

            Assert.AreEqual(expected, actual);
        }
    }
}
