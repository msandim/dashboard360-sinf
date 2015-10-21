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
            // Test GetAbsences(employeeID)
            List<Absence> absences_001 = PriIntegration.GetAbsences("001");

            String response_001 = "";
            foreach (Absence absence in absences_001)
                response_001 += "Funcionario: " + absence.EmployeeId + " Date: " + absence.Date + "\n";
            Assert.AreEqual("hello", response_001);

            // Test GetAbsences()
            List<Absence> absences = PriIntegration.GetAbsences();

            String response = "";
            foreach (Absence absence in absences)
                response += "Funcionario: " + absence.EmployeeId + " Date: " + absence.Date + "\n";
            Assert.AreEqual("hello", response);
           

            // Check columns of CadastroFaltas
            //Assert.AreEqual("hello", PriIntegration.testSQL("SELECT name FROM syscolumns WHERE id=OBJECT_ID('CadastroFaltas')", new List<string>(new string[] { "name" })));

            // Check dates of all Absences
            //Assert.AreEqual("hello", PriIntegration.testSQL("SELECT Funcionario, Falta, Data, Horas, Tempo FROM CadastroFaltas Where Falta='F01'", new List<string>(new string[] { "Funcionario", "Falta", "Data", "Horas", "Tempo" })));
        }


    }
}
