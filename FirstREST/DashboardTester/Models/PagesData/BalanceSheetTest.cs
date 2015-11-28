using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dashboard.Models.PagesData;
using Dashboard.Models.Primavera;
using Dashboard.Models.Primavera.Model;
using System.Collections.Generic;

namespace DashboardTester.Models.PagesData
{
    [TestClass]
    public class BalanceSheetTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var balanceSheet = PriIntegration.GetBalanceSheet();
            BalanceSheet sheet = new BalanceSheet(balanceSheet);
            String i = sheet.moeda;
            Dictionary<String, Metric> m = sheet.metrics;
        }
    }
}
