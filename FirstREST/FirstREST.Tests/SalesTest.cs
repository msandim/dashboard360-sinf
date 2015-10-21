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
            // Test GetSales()
            List<Sale> sales = PriIntegration.GetSales();

            String response = "";
            foreach (Sale sale in sales)
                response += "ID: " + sale.ID + " Item: " + sale.Item + " Category: " + sale.Category + " Comission: " + sale.Comission + "\n"
                    + " PayedOn: " + sale.PayedOn + " ProductDeliveredOn: " + sale.ProductDeliveredOn + " EmployeeId: " + sale.EmployeeId + "\n"
                    + " Costumer: " + sale.Costumer + " Value: " + sale.Value.Value + " Currency: " + sale.Value.Currency + "\n \n";
            Assert.AreEqual("hello", response);

            
            // Check columns of table CabecDoc
            //Assert.AreEqual("hello", PriIntegration.testSQL("SELECT name FROM syscolumns WHERE id=OBJECT_ID('CabecDoc')", new List<string>(new string[] {"name"})));

            // Check columns of table CabecDoc
            //Assert.AreEqual("hello", PriIntegration.testSQL("SELECT name FROM syscolumns WHERE id=OBJECT_ID('LinhasDoc')", new List<string>(new string[] {"name"})))

            //Assert.AreEqual("hello", PriIntegration.testSQL("SELECT Data, DataCarga, DataDescarga, DataUltimaActualizacao, DataVencimento FROM CabecDoc", new List<string>(new string[] { "Data", "DataCarga", "DataDescarga", "DataUltimaActualizacao", "DataVencimento" })));
            //Assert.AreEqual("hello", PriIntegration.testSQL("SELECT CategoriaID FROM LinhasDoc", new List<string>(new string[] { "CategoriaID" })));

            //Assert.AreEqual("hello", PriIntegration.testSQL("SELECT IdCabecDoc, Id, Artigo, CategoriaID, Comissao, Data, DataEntrega, PrecUnit, Quantidade, Unidade, Vendedor FROM LinhasDoc", new List<string>(new string[] { "IdCabecDoc", "Id", "Artigo", "CategoriaID", "Comissao", "Data", "DataEntrega", "PrecUnit", "Quantidade", "Unidade", "Vendedor" })));

            //Assert.AreEqual("hello", PriIntegration.testSQL("SELECT Entidade, Utilizador, Nome FROM CabecDoc", new List<string>(new string[] { "Entidade", "Utilizador", "Nome" })));
          
        }
    }
}
