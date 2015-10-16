using System;
using FirstREST.Lib_Primavera;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FirstREST.Tests.Lib_Primavera
{

    [TestClass]
    public class EmployeeTest
    {
        [TestMethod]
        public void GetEmployeesTest()
        {
            PriIntegration.GetEmployees();
        }
    }
}
