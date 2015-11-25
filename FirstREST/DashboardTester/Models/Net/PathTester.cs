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
            var path = PathConstants.BasePathApiPrimavera;

            path.AddParameter("initialDate", "2015-01-02");
            path.AddParameter("finalDate", "2015-01-05");

            var actual = path.ToString();
            var expected = PathConstants.BasePathApiPrimavera + "?initialDate=2015-01-02&finalDate=2015-01-05";

            Assert.AreEqual(expected, actual);
        }
    }
}
