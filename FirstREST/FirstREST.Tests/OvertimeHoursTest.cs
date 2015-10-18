using System;
using FirstREST.Lib_Primavera;
using FirstREST.Lib_Primavera.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace FirstREST.Tests
{
    [TestClass]
    public class OvertimeHoursTest
    {
        [TestMethod]
        public void GetOvertimeHoursTest()
        {
            List<OvertimeHours> list = PriIntegration.GetOvertimeHours("001");

            //Used to test output, don't now why but Console.Writeline() doesn't work
            /*
            String response = "";
            foreach (OvertimeHours overtimeHours in list)
                response += overtimeHours.Date + " : " + overtimeHours.Tempo + "\n";
            Assert.AreEqual("hello", response);
            */

            // Check columns of CadastroHExtras table
            //Assert.AreEqual("hello", PriIntegration.testSQL("SELECT name FROM syscolumns WHERE id=OBJECT_ID('CadastroHExtras')", new List<string>(new string[] { "name" })));

            // Check all Datas e Tempo from CadastroHExtras
            //Assert.AreEqual("hello", PriIntegration.testSQL("SELECT Data, Tempo FROM CadastroHExtras", new List<string>(new string[] { "Data, Tempo" })));
            
        }
    }
}
