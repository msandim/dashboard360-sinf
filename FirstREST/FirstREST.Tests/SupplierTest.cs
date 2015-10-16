using System;
using FirstREST.Lib_Primavera;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FirstREST.Tests.Lib_Primavera
{

    [TestClass]
    public class SupplierTest
    {
        [TestMethod]
        public void GetSuppliersTest()
        {
            PriIntegration.GetSuppliers();
        }
    }
}
