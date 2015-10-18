using System;
using FirstREST.Lib_Primavera;
using FirstREST.Lib_Primavera.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace FirstREST.Tests
{
    [TestClass]
    public class AbsenceTest
    {
        [TestMethod]
        public void GetAbsencesTest()
        {
            List<Absence> absences = PriIntegration.GetAbsences("001");

            //Used to test output, don't now why but Console.Writeline() doesn't work
            /*
                String response = "";
                foreach (Absence absence in absences)
                    response += absence.Date + "\n";
                Assert.AreEqual("hello", response);
            */

            // Check columns of CadastroFaltas
            //Assert.AreEqual("hello", PriIntegration.testSQL("SELECT name FROM syscolumns WHERE id=OBJECT_ID('CadastroFaltas')", new List<string>(new string[] { "name" })));

            // Check dates of all Absences
            //Assert.AreEqual("hello", PriIntegration.testSQL("SELECT Data FROM CadastroFaltas", new List<string>(new string[] { "Data" })));
        }
    }
}
