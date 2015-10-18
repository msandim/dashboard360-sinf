using System;
using FirstREST.Lib_Primavera;
using FirstREST.Lib_Primavera.Model;
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
            
            List<Sale> sales = PriIntegration.GetSales();

            //Used to test output, don't now why but Console.Writeline() doesn't work
            String response = "";
            foreach (Sale sale in sales)
                response += sale.ID + " : " + sale.PayedOn + " : " + sale.ProductDeliveredOn + " : " + sale.Category + "\n";
            Assert.AreEqual("hello", response);
            

            // Check columns of table CabecDoc
            //Assert.AreEqual("hello", PriIntegration.testSQL("SELECT name FROM syscolumns WHERE id=OBJECT_ID('CabecDoc')", new List<string>(new string[] {"name"})));

            // Check columns of table CabecDoc
            //Assert.AreEqual("hello", PriIntegration.testSQL("SELECT name FROM syscolumns WHERE id=OBJECT_ID('LinhasDoc')", new List<string>(new string[] {"name"})))

            //Assert.AreEqual("hello", PriIntegration.testSQL("SELECT CategoriaID FROM CabecDoc", new List<string>(new string[] { "CategoriaID" })));
            //Assert.AreEqual("hello", PriIntegration.testSQL("SELECT CategoriaID FROM LinhasDoc", new List<string>(new string[] { "CategoriaID" })));
          
        }
    }
}
