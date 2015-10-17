using System;
using FirstREST.Lib_Primavera;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace FirstREST.Tests
{
    [TestClass]
    public class SalesTest
    {
        [TestMethod]
        public void GetSalesTest()
        {
            PriIntegration.GetSales();
            //Assert.AreEqual("hello", PriIntegration.testSQL("SELECT name FROM syscolumns WHERE id=OBJECT_ID('CabecDoc')", new List<string>(new string[] {"name"})));
            //Assert.AreEqual("hello", PriIntegration.testSQL("SELECT name FROM syscolumns WHERE id=OBJECT_ID('LinhasDoc')", new List<string>(new string[] { "name" })));
        }
    }
}
