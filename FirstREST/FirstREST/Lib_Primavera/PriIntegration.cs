using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Interop.ErpBS800;
using Interop.StdPlatBS800;
using Interop.StdBE800;
using Interop.GcpBE800;
using ADODB;
using Interop.IGcpBS800;
using Interop.IRhpBS800;
//using Interop.StdBESql800;
//using Interop.StdBSSql800;
using FirstREST.Lib_Primavera.Model;
using FirstREST.Properties;

namespace FirstREST.Lib_Primavera
{
    public class PriIntegration
    {
        private static DateTime ParseDate(StdBELista list, String key)
        {
            try
            {
                return list.Valor(key);
            }
            catch
            {
                return new DateTime(0);
            }
        }

        public static List<Pending> GetPendingReceivables(DateTime initialDate, DateTime finalDate)
        {
            return GetPendingDocuments(initialDate, finalDate, true);
        }
        public static List<Pending> GetPendingPayables(DateTime initialDate, DateTime finalDate)
        {
            return GetPendingDocuments(initialDate, finalDate, false);
        }
        private static List<Pending> GetPendingDocuments(DateTime initialDate, DateTime finalDate, bool receivables)
        {
            List<Pending> output = new List<Pending>();

            if (!InitializeCompany())
                return output;

            StdBELista pendingDocumentsQuery = PriEngine.Engine.Consulta(
                "SELECT ValorTotal, Moeda, TipoDoc, Entidade, TipoEntidade, Estado, DataVenc, DataDoc " +
                "FROM Pendentes " +
                "WHERE ValorTotal " + (receivables ? ">" : "<") + " 0 " +
                " AND DataDoc >= '" + initialDate.ToString("yyyyMMdd") + "' AND DataDoc <= '" + finalDate.ToString("yyyyMMdd") + "' " +
                "ORDER BY DataDoc"
                );
            while(!pendingDocumentsQuery.NoFim())
            {
                Pending pendingDocument = new Pending();
                pendingDocument.TotalValue = new Money(pendingDocumentsQuery.Valor("ValorTotal"), pendingDocumentsQuery.Valor("Moeda"));
                pendingDocument.DocumentDate = pendingDocumentsQuery.Valor("DataDoc");
                pendingDocument.DocumentType = pendingDocumentsQuery.Valor("TipoDoc");
                pendingDocument.DueDate = pendingDocumentsQuery.Valor("DataVenc");
                pendingDocument.Entity = pendingDocumentsQuery.Valor("Entidade");
                pendingDocument.EntityType = pendingDocumentsQuery.Valor("TipoEntidade");
                pendingDocument.State = pendingDocumentsQuery.Valor("Estado");
                output.Add(pendingDocument);

                pendingDocumentsQuery.Seguinte();
            }

            return output;
        }

        public static List<Purchase> GetPurchases(DateTime initialDate, DateTime finalDate, String documentType)
        {
            // Create an empty list of purchases:
            List<Model.Purchase> purchases = new List<Model.Purchase>();

            if (!PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()))
                return purchases;

            StdBELista purchasesQuery = PriEngine.Engine.Consulta(
                "SELECT CabecCompras.Id AS CabecComprasId, CabecCompras.Nome AS CabecComprasNome, CabecCompras.Entidade AS CabecComprasEntidade, CabecCompras.Moeda AS CabecComprasMoeda, CabecCompras.DataDoc AS CabecComprasDataDoc, CabecCompras.TipoDoc AS CabecComprasTipoDoc, CabecCompras.DataVencimento AS CabecComprasDataVencimento, CabecCompras.DataDescarga AS CabecComprasDataDescarga, " +
                "LinhasCompras.Id AS LinhasComprasId, LinhasCompras.PrecoLiquido AS LinhasComprasPrecoLiquido, " +
                "Artigo.Marca AS ArtigoMarca, Artigo.Modelo AS ArtigoModelo, Artigo.Descricao AS ArticoDescricao, Artigo.TipoArtigo AS ArtigoTipoArtigo, " +
                "Familias.Familia AS FamiliaId, Familias.Descricao AS FamiliaDescricao " +
                "FROM CabecCompras " +
                "INNER JOIN LinhasCompras ON LinhasCompras.IdCabecCompras=CabecCompras.Id " +
                "INNER JOIN Artigo ON Artigo.Artigo=LinhasCompras.Artigo " +
                "INNER JOIN Familias ON Artigo.Familia=Familias.Familia " +
                "WHERE CabecCompras.DataDoc >= '" + initialDate.ToString("yyyyMMdd") + "' AND CabecCompras.DataDoc <= '" + finalDate.ToString("yyyyMMdd") + "' " +
                "AND CabecCompras.TipoDoc='" + documentType + "' " +
                "ORDER BY CabecCompras.DataDoc"
                );

            while (!purchasesQuery.NoFim())
            {
                Purchase purchase = new Model.Purchase();

                // Set values:
                purchase.ID = purchasesQuery.Valor("LinhasComprasId");
                purchase.DocumentDate = ParseDate(purchasesQuery, "CabecComprasDataDoc");
                purchase.DocumentType = purchasesQuery.Valor("CabecComprasTipoDoc");
                purchase.DueDate = ParseDate(purchasesQuery, "CabecComprasDataVencimento");
                purchase.ReceptionDate = ParseDate(purchasesQuery, "CabecComprasDataDescarga");
                purchase.EntityId = purchasesQuery.Valor("CabecComprasEntidade");
                purchase.EntityName = purchasesQuery.Valor("CabecComprasNome");
                purchase.Value = new Model.Money(purchasesQuery.Valor("LinhasComprasPrecoLiquido"), purchasesQuery.Valor("CabecComprasMoeda"));

                Product product = new Model.Product();
                product.Brand = purchasesQuery.Valor("ArtigoMarca");
                product.Model = purchasesQuery.Valor("ArtigoModelo");
                product.Description = purchasesQuery.Valor("ArticoDescricao");
                product.FamilyId = purchasesQuery.Valor("FamiliaId");
                product.FamilyDescription = purchasesQuery.Valor("FamiliaDescricao");
                purchase.Product = product;

                // Add purchase to the list:
                purchases.Add(purchase);

                // Next line in the purchase document:
                purchasesQuery.Seguinte();
            }

            return purchases;
        }
        public static List<Sale> GetSales(DateTime initialDate, DateTime finalDate, String documentType)
        {
            // Create an empty list of sales
            List<Sale> sales = new List<Model.Sale>();

            //Initialize company
            if (!PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()))
                return sales;

            //DataDescarga always null ?
            StdBELista salesQuery = PriEngine.Engine.Consulta(
                "SELECT CabecDoc.Id AS CabecDocId, CabecDoc.Nome AS CabecDocNome, CabecDoc.Entidade AS CabecDocEntidade, CabecDoc.Moeda AS CabecDocMoeda, CabecDoc.TipoDoc AS CabecDocTipoDoc, CabecDoc.Data AS CabecDocData, CabecDoc.DataVencimento AS CabecDocDataVencimento, CabecDoc.DataCarga AS CabecDocDataCarga, CabecDoc.DataDescarga AS CabecDocsDataDescarga, " +
                "LinhasDoc.Id AS LinhasDocId, LinhasDoc.PrecoLiquido AS LinhasDocPrecoLiquido, " +
                "Artigo.Marca AS ArtigoMarca, Artigo.Modelo AS ArtigoModelo, Artigo.Descricao AS ArticoDescricao, Artigo.TipoArtigo AS ArtigoTipoArtigo, " +
                "Familias.Familia AS FamiliaId, Familias.Descricao AS FamiliaDescricao " +
                "FROM CabecDoc " +
                "INNER JOIN LinhasDoc ON LinhasDoc.IdCabecDoc = CabecDoc.Id " +
                "INNER JOIN Artigo ON Artigo.Artigo = LinhasDoc.Artigo " +
                "INNER JOIN Familias ON Artigo.Familia = Familias.Familia " +
                "WHERE CabecDoc.Data >= '" + initialDate.ToString("yyyyMMdd") + "' AND CabecDoc.Data <= '" + finalDate.ToString("yyyyMMdd") + "' " +
                "AND CabecDoc.TipoDoc='" + documentType + "' " +
                "ORDER BY CabecDoc.DataDoc"
                );

            while (!salesQuery.NoFim())
            {
                Sale sale = new Model.Sale();

                sale.ID = salesQuery.Valor("LinhasDocId");
                sale.DocumentDate = ParseDate(salesQuery, "CabecDocData");
                sale.DocumentType = salesQuery.Valor("CabecDocTipoDoc");
                sale.DueDate = ParseDate(salesQuery, "CabecDocDataVencimento");
                sale.ReceptionDate = ParseDate(salesQuery, "CabecDocsDataDescarga");
                sale.ClientId = salesQuery.Valor("CabecDocEntidade");
                sale.ClientName = salesQuery.Valor("CabecDocNome");
                sale.Value = new Model.Money(salesQuery.Valor("LinhasDocPrecoLiquido"), salesQuery.Valor("CabecDocMoeda"));

                Product product = new Model.Product();
                product.Brand = salesQuery.Valor("ArtigoMarca");
                product.Model = salesQuery.Valor("ArtigoModelo");
                product.Description = salesQuery.Valor("ArticoDescricao");
                product.FamilyId = salesQuery.Valor("FamiliaId");
                product.FamilyDescription = salesQuery.Valor("FamiliaDescricao");
                sale.Product = product;

                sales.Add(sale);

                // Next item:
                salesQuery.Seguinte();
            }

            return sales;
        }
        public static List<Employee> GetEmployees()
        {
            // Create an empty list of employees:
            List<Model.Employee> employees = new List<Model.Employee>();

            if (!PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()))
                return employees;

            StdBELista list = PriEngine.Engine.Consulta(
                "SELECT IdGDOC, Nome, Sexo, Vencimento, TipoMoeda, DataAdmissao, DataDemissao " +
                "FROM Funcionarios "
                );
            while (!list.NoFim())
            {
                Employee employee = new Employee();

                // Set values
                employee.ID = list.Valor("IdGDOC");
                employee.Name = list.Valor("Nome");
                employee.Gender = list.Valor("Sexo") == "0" ? Employee.GenderType.Male : Employee.GenderType.Female;
                employee.Salary = new Money(list.Valor("Vencimento"), "Unspecified"); // No currency value
                employee.HiredOn = ParseDate(list, "DataAdmissao");
                employee.FiredOn = ParseDate(list, "DataDemissao");

                // Add employee to the list:
                employees.Add(employee);

                // Next item:
                list.Seguinte();
            }

            return employees;
        }
        public static List<Absence> GetAbsences(DateTime initialDate, DateTime finalDate) // Returns a List of all absences of all employees
        {
            // Create an empty list of absences:
            List<Model.Absence> absences = new List<Model.Absence>();

            if (!PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()))
                return absences;

            // Get Data from Absence of the employee with ID=employeeId
            StdBELista list = PriEngine.Engine.Consulta(
                "SELECT Funcionario, Data FROM CadastroFaltas " +
                "WHERE Data >= '" + initialDate.ToString("yyyyMMdd") + "' AND Data <= '" + finalDate.ToString("yyyyMMdd") + "' " +
                "ORDER BY Data"
                );

            while (!list.NoFim())
            {
                Model.Absence absence = new Model.Absence();
                absence.EmployeeId = list.Valor("Funcionario");
                absence.Date = list.Valor("Data");

                // Add absence to the list:
                absences.Add(absence);

                // Next item:
                list.Seguinte();
            }

            return absences;
        }
        public static List<Absence> GetAbsences(String employeeId)
        {
            // Create an empty list of absences:
            List<Model.Absence> absences = new List<Model.Absence>();

            if (!PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()))
                return absences;

            // Get Data from Absence of the employee with ID=employeeId
            StdBELista list = PriEngine.Engine.Consulta(
                "SELECT Data FROM CadastroFaltas WHERE Funcionario='" + employeeId + "'"
                );

            while (!list.NoFim())
            {
                Model.Absence absence = new Model.Absence();
                absence.EmployeeId = employeeId;
                absence.Date = list.Valor("Data");

                // Add absence to the list:
                absences.Add(absence);

                // Next item:
                list.Seguinte();
            }

            return absences;
        } // Returns a List of all absences of the employee with the given employeeId
        public static List<OvertimeHours> GetOvertimeHours(DateTime initialDate, DateTime finalDate)
        {
            // Create an empty list of absences:
            List<Model.OvertimeHours> overtimeHours = new List<Model.OvertimeHours>();

            if (!PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()))
                return overtimeHours;

            // Get Data from Absence of the employee with ID=employeeId BD
            StdBELista list = PriEngine.Engine.Consulta(
                "SELECT Funcionario, Data, Tempo " +
                "FROM CadastroHExtras " +
                "WHERE Data >= '" + initialDate.ToString("yyyyMMdd") + "' AND Data <= '" + finalDate.ToString("yyyyMMdd") + "' "
                );

            // Get Data from Absence of the employee with ID=employeeId Not BD
            //StdBELista list = PriEngine.Engine.RecursosHumanos.CadastroHorasExtra.LstCadastroHorasExtra();

            while (!list.NoFim())
            {
                Model.OvertimeHours overtimeHoursObj = new Model.OvertimeHours();
                overtimeHoursObj.EmployeeId = list.Valor("Funcionario");
                overtimeHoursObj.Date = list.Valor("Data");
                overtimeHoursObj.Time = list.Valor("Tempo").ToString();

                // Add absence to the list:
                overtimeHours.Add(overtimeHoursObj);

                // Next item:
                list.Seguinte();
            }

            return overtimeHours;
        }
        public static List<OvertimeHours> GetOvertimeHours(String employeeId)
        {
            // Create an empty list of absences:
            List<Model.OvertimeHours> overtimeHours = new List<Model.OvertimeHours>();

            if (!PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()))
                return overtimeHours;

            /*
            // Get Data from Absence of the employee with ID=employeeId BD
            StdBELista list = PriEngine.Engine.Consulta(
                "SELECT Funcionario, Data, Tempo FROM CadastroHExtras WHERE Funcionario='" + employeeId + "'"
                );
            */

            // Get Data from Absence of the employee with ID=employeeId Not BD
            StdBELista list = PriEngine.Engine.RecursosHumanos.CadastroHorasExtra.LstCadastroHorasExtra();

            while (!list.NoFim())
            {
                Model.OvertimeHours overtimeHoursObj = new Model.OvertimeHours();

                if (list.Valor("Funcionario") != employeeId)
                {
                    list.Seguinte();
                    continue;
                }

                overtimeHoursObj.EmployeeId = employeeId;
                overtimeHoursObj.Date = list.Valor("Data");
                overtimeHoursObj.Time = list.Valor("Tempo").ToString();

                // Add absence to the list:
                overtimeHours.Add(overtimeHoursObj);

                // Next item:
                list.Seguinte();
            }

            return overtimeHours;
        }
        public static float GetMaleToFemaleRatio()
        {
            if (!PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()))
                return 1;

            StdBELista list = PriEngine.Engine.RecursosHumanos.Funcionarios.LstFuncionarios();
            int males = 0;
            int females = 0;

            if (list.NoFim())
                return 1;

            while (!list.NoFim())
            {
                if (list.Valor("Sexo") == "1")
                    males++;
                else
                    females++;

                list.Seguinte();
            }

            return (float)males / females;
        }

        private static bool InitializeCompany()
        {
            return PriEngine.InitializeCompany(Settings.Default.Company.Trim(), Settings.Default.User.Trim(), Settings.Default.Password.Trim());
        }

        // Function to test SQL queries:
        public static String testSQL(String sql, List<String> columns)
        {
            String response = "";
            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {
                StdBELista list = PriEngine.Engine.Consulta(sql);
                response += "Numero de linhas: " + list.NumLinhas() + "\n";
                response += "Numero de colunas: " + list.NumColunas() + "\n";
                while (!list.NoFim())
                {
                    foreach (String column in columns)
                        response += column + ": " + list.Valor(column) + ";";
                    response += "\n";
                    list.Seguinte();
                }
            }
            return response;
        }
    }
}