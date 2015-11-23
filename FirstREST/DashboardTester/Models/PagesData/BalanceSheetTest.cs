using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dashboard.Models.PagesData;
using Dashboard.Models.Primavera;

namespace DashboardTester.Models.PagesData
{
    [TestClass]
    public class BalanceSheetTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var balanceSheet = PriIntegration.GetBalanceSheet();
            //BalanceSheet sheet = new BalanceSheet();
            int i = 0;

            
        }
    }
}
