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
            Path path = new Path(PathConstants.BASE_PATH_API_PRIMAVERA);

            path.AddParameter("initialDate", "2015-01-02");
            path.AddParameter("finalDate", "2015-01-05");

            String actual = path.ToString();
            String expected = PathConstants.BASE_PATH_API_PRIMAVERA + "?initialDate=2015-01-02&finalDate=2015-01-05";

            Assert.AreEqual(expected, actual);
        }
    }
}
