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

namespace Dashboard.Models.Primavera
{
    using Dashboard.Properties;
    using Model;

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
                return DateTime.MinValue;
            }
        }

        public static Dictionary<int, Dictionary<string, ClassLine>> GetBalanceSheet()
        {
            //OUTPUT
            Dictionary<int, Dictionary<string, ClassLine>> output = new Dictionary<int, Dictionary<string, ClassLine>>();

            //STABLISH PRIMAVERA COMMUNICATION
            if (!InitializeCompany())
                return output;
            StdBELista balanceSheetQuery = PriEngine.Engine.Consulta("SELECT * FROM AcumuladosContas ORDER BY Ano DESC");

            //COMPLETE OUTPUT
            Dictionary<string, ClassLine> year_balance = new Dictionary<string, ClassLine>();
            int past_year = 0;

            while (!balanceSheetQuery.NoFim())
            {
                ClassLine line = new ClassLine();
                line.ano = balanceSheetQuery.Valor("Ano");

                //CHECK IF IT IS A NEW YEAR TO CREATE A NEW BALANCE
                if (line.ano != past_year && past_year != 0) //in the first case the years are different but we want to continue
                {
                    output.Add(past_year, year_balance);
                    year_balance = new Dictionary<string, ClassLine>();
                }

                processLine(balanceSheetQuery, line);
                if(line.tipoLancamento == "000")
                    year_balance.Add(line.conta.ToString(), line);

                past_year = line.ano;
                balanceSheetQuery.Seguinte();
            }

            //if there are no more lines the cycle will end, so we have to add the data outside
            output.Add(past_year, year_balance);

            return output;
        }

        public static void processLine(StdBELista balanceSheetQuery, ClassLine line)
        {
            line.conta = balanceSheetQuery.Valor("Conta");
            line.moeda = balanceSheetQuery.Valor("Moeda");
            
            //line.values.Add(d);
            line.values.Add((double) balanceSheetQuery.Valor("Mes00CR"));
            line.values.Add((double) balanceSheetQuery.Valor("Mes01CR"));
            line.values.Add((double) balanceSheetQuery.Valor("Mes02CR"));
            line.values.Add((double) balanceSheetQuery.Valor("Mes03CR"));
            line.values.Add((double) balanceSheetQuery.Valor("Mes04CR"));
            line.values.Add((double) balanceSheetQuery.Valor("Mes05CR"));
            line.values.Add((double) balanceSheetQuery.Valor("Mes06CR"));
            line.values.Add((double) balanceSheetQuery.Valor("Mes07CR"));
            line.values.Add((double) balanceSheetQuery.Valor("Mes08CR"));
            line.values.Add((double) balanceSheetQuery.Valor("Mes09CR"));
            line.values.Add((double) balanceSheetQuery.Valor("Mes10CR"));
            line.values.Add((double) balanceSheetQuery.Valor("Mes11CR"));
            line.values.Add((double) balanceSheetQuery.Valor("Mes12CR"));
            line.values.Add((double) balanceSheetQuery.Valor("Mes13CR"));
            line.values.Add((double) balanceSheetQuery.Valor("Mes14CR"));
            line.values.Add((double) balanceSheetQuery.Valor("Mes15CR"));

            line.values.Add((double) balanceSheetQuery.Valor("Mes00DB"));
            line.values.Add((double) balanceSheetQuery.Valor("Mes01DB"));
            line.values.Add((double) balanceSheetQuery.Valor("Mes02DB"));
            line.values.Add((double) balanceSheetQuery.Valor("Mes03DB"));
            line.values.Add((double) balanceSheetQuery.Valor("Mes04DB"));
            line.values.Add((double) balanceSheetQuery.Valor("Mes05DB"));
            line.values.Add((double) balanceSheetQuery.Valor("Mes06DB"));
            line.values.Add((double) balanceSheetQuery.Valor("Mes07DB"));
            line.values.Add((double) balanceSheetQuery.Valor("Mes08DB"));
            line.values.Add((double) balanceSheetQuery.Valor("Mes09DB"));
            line.values.Add((double) balanceSheetQuery.Valor("Mes10DB"));
            line.values.Add((double) balanceSheetQuery.Valor("Mes11DB"));
            line.values.Add((double) balanceSheetQuery.Valor("Mes12DB"));
            line.values.Add((double) balanceSheetQuery.Valor("Mes13DB"));
            line.values.Add((double) balanceSheetQuery.Valor("Mes14DB"));
            line.values.Add((double) balanceSheetQuery.Valor("Mes15DB"));

            line.values.Add((double) balanceSheetQuery.Valor("Mes01OR"));
            line.values.Add((double) balanceSheetQuery.Valor("Mes02OR"));
            line.values.Add((double) balanceSheetQuery.Valor("Mes03OR"));
            line.values.Add((double) balanceSheetQuery.Valor("Mes04OR"));
            line.values.Add((double) balanceSheetQuery.Valor("Mes05OR"));
            line.values.Add((double) balanceSheetQuery.Valor("Mes06OR"));
            line.values.Add((double) balanceSheetQuery.Valor("Mes07OR"));
            line.values.Add((double) balanceSheetQuery.Valor("Mes08OR"));
            line.values.Add((double) balanceSheetQuery.Valor("Mes09OR"));
            line.values.Add((double) balanceSheetQuery.Valor("Mes10OR"));
            line.values.Add((double) balanceSheetQuery.Valor("Mes11OR"));
            line.values.Add((double) balanceSheetQuery.Valor("Mes12OR"));

            line.tipoLancamento = balanceSheetQuery.Valor("TipoLancamento");
            line.naturezaOR = balanceSheetQuery.Valor("NaturezaOR");
        }

        // Pending payments:
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
                "SELECT ValorPendente, Moeda, TipoDoc, Entidade, TipoEntidade, Estado, DataVenc, DataDoc " +
                "FROM Pendentes " +
                "WHERE TipoEntidade = " + (receivables ? "'C'" : "'F'") +
                " AND DataDoc >= '" + initialDate.ToString("yyyyMMdd") + "' AND DataDoc <= '" + finalDate.ToString("yyyyMMdd") + "' " +
                "ORDER BY DataDoc"
                );
            while(!pendingDocumentsQuery.NoFim())
            {
                Pending pendingDocument = new Pending();
                pendingDocument.PendingValue = new Money(pendingDocumentsQuery.Valor("ValorPendente"), pendingDocumentsQuery.Valor("Moeda"));
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

        // Purchases & Sales:
        public static List<Purchase> GetPurchases(DateTime initialDate, DateTime finalDate)
        {
            // Create an empty list of purchases:
            List<Purchase> purchases = new List<Purchase>();

            if (!InitializeCompany())
                return purchases;

            StdBELista purchasesQuery = PriEngine.Engine.Consulta(
                "SELECT CabecCompras.Id AS CabecComprasId, CabecCompras.Nome AS CabecComprasNome, CabecCompras.Entidade AS CabecComprasEntidade, CabecCompras.Moeda AS CabecComprasMoeda, CabecCompras.DataDoc AS CabecComprasDataDoc, CabecCompras.TipoDoc AS CabecComprasTipoDoc, CabecCompras.DataVencimento AS CabecComprasDataVencimento, CabecCompras.DataDescarga AS CabecComprasDataDescarga, " +
                "LinhasCompras.Id AS LinhasComprasId, LinhasCompras.PrecoLiquido AS LinhasComprasPrecoLiquido, " +
                "Artigo.Marca AS ArtigoMarca, Artigo.Modelo AS ArtigoModelo, Artigo.Descricao AS ArticoDescricao, Artigo.TipoArtigo AS ArtigoTipoArtigo, " +
                "Familias.Familia AS FamiliaId, Familias.Descricao AS FamiliaDescricao, " +
                "Iva.Taxa AS IvaTaxa " +
                "FROM CabecCompras " +
                "INNER JOIN LinhasCompras ON LinhasCompras.IdCabecCompras=CabecCompras.Id " +
                "INNER JOIN Artigo ON Artigo.Artigo=LinhasCompras.Artigo " +
                "INNER JOIN Familias ON Artigo.Familia=Familias.Familia " +
                "INNER JOIN Iva ON LinhasCompras.CodIva = Iva.Iva " +
                "WHERE CabecCompras.DataDoc >= '" + initialDate.ToString("yyyyMMdd") + "' AND CabecCompras.DataDoc <= '" + finalDate.ToString("yyyyMMdd") + "' " +
                "ORDER BY CabecCompras.DataDoc"
                );

            while (!purchasesQuery.NoFim())
            {
                Purchase purchase = new Purchase();

                // Set values:
                purchase.ID = purchasesQuery.Valor("LinhasComprasId");
                purchase.DocumentDate = ParseDate(purchasesQuery, "CabecComprasDataDoc");
                purchase.DocumentType = purchasesQuery.Valor("CabecComprasTipoDoc");
                purchase.DueDate = ParseDate(purchasesQuery, "CabecComprasDataVencimento");
                purchase.ReceptionDate = ParseDate(purchasesQuery, "CabecComprasDataDescarga");
                purchase.EntityId = purchasesQuery.Valor("CabecComprasEntidade");
                purchase.EntityName = purchasesQuery.Valor("CabecComprasNome");
                purchase.Value = new Money(purchasesQuery.Valor("LinhasComprasPrecoLiquido"), purchasesQuery.Valor("CabecComprasMoeda"));
                purchase.Iva = purchasesQuery.Valor("IvaTaxa") / 100.0;

                Product product = new Product();
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
        public static List<Sale> GetSales(DateTime initialDate, DateTime finalDate)
        {
            // Create an empty list of sales
            List<Sale> sales = new List<Sale>();

            //Initialize company
            if (!InitializeCompany())
                return sales;

            //DataDescarga always null ?
            StdBELista salesQuery = PriEngine.Engine.Consulta(
                "SELECT CabecDoc.Id AS CabecDocId, CabecDoc.Nome AS CabecDocNome, CabecDoc.Entidade AS CabecDocEntidade, CabecDoc.Moeda AS CabecDocMoeda, CabecDoc.TipoDoc AS CabecDocTipoDoc, CabecDoc.Data AS CabecDocData, CabecDoc.DataVencimento AS CabecDocDataVencimento, CabecDoc.DataCarga AS CabecDocDataCarga, CabecDoc.DataDescarga AS CabecDocsDataDescarga, " +
                "LinhasDoc.Id AS LinhasDocId, LinhasDoc.PrecoLiquido AS LinhasDocPrecoLiquido, " +
                "Artigo.Marca AS ArtigoMarca, Artigo.Modelo AS ArtigoModelo, Artigo.Descricao AS ArticoDescricao, Artigo.TipoArtigo AS ArtigoTipoArtigo, " +
                "Familias.Familia AS FamiliaId, Familias.Descricao AS FamiliaDescricao, " +
                "Iva.Taxa AS IvaTaxa " +
                "FROM CabecDoc " +
                "INNER JOIN LinhasDoc ON LinhasDoc.IdCabecDoc = CabecDoc.Id " +
                "INNER JOIN Artigo ON Artigo.Artigo = LinhasDoc.Artigo " +
                "INNER JOIN Familias ON Artigo.Familia = Familias.Familia " +
                "INNER JOIN Iva ON LinhasDoc.CodIva = Iva.Iva " +
                "WHERE CabecDoc.Data >= '" + initialDate.ToString("yyyyMMdd") + "' AND CabecDoc.Data <= '" + finalDate.ToString("yyyyMMdd") + "' " +
                "ORDER BY CabecDoc.Data"
                );

            while (!salesQuery.NoFim())
            {
                Sale sale = new Sale();

                sale.ID = salesQuery.Valor("LinhasDocId");
                sale.DocumentDate = ParseDate(salesQuery, "CabecDocData");
                sale.DocumentType = salesQuery.Valor("CabecDocTipoDoc");
                sale.DueDate = ParseDate(salesQuery, "CabecDocDataVencimento");
                sale.ReceptionDate = ParseDate(salesQuery, "CabecDocsDataDescarga");
                sale.ClientId = salesQuery.Valor("CabecDocEntidade");
                sale.ClientName = salesQuery.Valor("CabecDocNome");
                sale.Value = new Money(salesQuery.Valor("LinhasDocPrecoLiquido"), salesQuery.Valor("CabecDocMoeda"));
                sale.Iva = salesQuery.Valor("IvaTaxa") / 100.0;

                Product product = new Product();
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
        
        // Human Resources related:
        public static List<Employee> GetEmployees(DateTime initialDate, DateTime finalDate)
        {
            // Create an empty list of employees:
            List<Employee> employees = new List<Employee>();

            if (!InitializeCompany())
                return employees;

            StdBELista list = PriEngine.Engine.Consulta(
                "SELECT IdGDOC, Nome, Sexo, Vencimento, TipoMoeda, DataAdmissao, DataDemissao " +
                "FROM Funcionarios " +
                "WHERE DataAdmissao <= '" + finalDate.ToString("yyyyMMdd") + "' " +
                " AND (DataDemissao >= '" + initialDate.ToString("yyyyMMdd") + "' OR DataDemissao IS NULL) " +
                "ORDER BY DataAdmissao"
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
            List<Absence> absences = new List<Absence>();

            if (!InitializeCompany())
                return absences;

            // Get Data from Absence of the employee with ID=employeeId
            StdBELista list = PriEngine.Engine.Consulta(
                "SELECT CadastroFaltas.Funcionario AS CadastroFaltasFuncionario, CadastroFaltas.Data AS CadastroFaltasData, Funcionarios.Nome AS FuncionariosNome " +
                "FROM CadastroFaltas " +
                "INNER JOIN Funcionarios ON CadastroFaltas.Funcionario = Funcionarios.Codigo " +
                "WHERE CadastroFaltas.Data >= '" + initialDate.ToString("yyyyMMdd") + "' AND CadastroFaltas.Data <= '" + finalDate.ToString("yyyyMMdd") + "' " +
                "ORDER BY CadastroFaltas.Data"
                );

            while (!list.NoFim())
            {
                Absence absence = new Absence();
                absence.EmployeeId = list.Valor("CadastroFaltasFuncionario");
                absence.EmployeeName = list.Valor("FuncionariosNome");
                absence.Date = list.Valor("CadastroFaltasData");

                // Add absence to the list:
                absences.Add(absence);

                // Next item:
                list.Seguinte();
            }

            return absences;
        }
        public static List<OvertimeHours> GetOvertimeHours(DateTime initialDate, DateTime finalDate)
        {
            // Create an empty list of absences:
            List<OvertimeHours> overtimeHours = new List<OvertimeHours>();

            if (!InitializeCompany())
                return overtimeHours;

            // Get Data from Absence of the employee with ID=employeeId BD
            StdBELista list = PriEngine.Engine.Consulta(
                "SELECT CadastroHExtras.Funcionario, CadastroHExtras.Data, CadastroHExtras.Tempo, Funcionarios.Nome AS FuncionariosNome " +
                "FROM CadastroHExtras " +
                "INNER JOIN Funcionarios " + 
                "ON CadastroHExtras.Funcionario = Funcionarios.Codigo " +
                "WHERE CadastroHExtras.Data >= '" + initialDate.ToString("yyyyMMdd") + "' AND CadastroHExtras.Data <= '" + finalDate.ToString("yyyyMMdd") + "' " +
                "ORDER BY CadastroHExtras.Data "
                );

            // Get Data from Absence of the employee with ID=employeeId Not BD
            //StdBELista list = PriEngine.Engine.RecursosHumanos.CadastroHorasExtra.LstCadastroHorasExtra();
            // TODO edit
            while (!list.NoFim())
            {
                OvertimeHours overtimeHoursObj = new OvertimeHours();
                overtimeHoursObj.EmployeeId = list.Valor("Funcionario");
                overtimeHoursObj.EmployeeName = list.Valor("FuncionariosNome");
                overtimeHoursObj.Date = list.Valor("Data");
                overtimeHoursObj.Time = list.Valor("Tempo").ToString();

                // Add absence to the list:
                overtimeHours.Add(overtimeHoursObj);

                // Next item:
                list.Seguinte();
            }

            return overtimeHours;
        }
        public static GenderCounter GetGenderCount(DateTime initialDate, DateTime finalDate)
        {
            if (!InitializeCompany())
                return new GenderCounter(-1,-1);

            StdBELista list = PriEngine.Engine.Consulta(
                "SELECT Sexo " +
                "FROM Funcionarios " +
                "WHERE DataAdmissao <= '" + finalDate.ToString("yyyyMMdd") + "' " +
                " AND (DataDemissao >= '" + initialDate.ToString("yyyyMMdd") + "' OR DataDemissao IS NULL) "
                );

            int males = 0;
            int females = 0;

            while (!list.NoFim())
            {
                if (list.Valor("Sexo") == "1")
                    males++;
                else
                    females++;

                list.Seguinte();
            }

            return new GenderCounter(males, females);
        } // Returns <male,female> format

        // Function to initialize the default company:
        private static bool InitializeCompany()
        {
            return PriEngine.InitializeCompany(Settings.Default.Company.Trim(), Settings.Default.User.Trim(), Settings.Default.Password.Trim());
        }

        // Function to test SQL queries:
        public static String TestSQL(String sql, List<String> columns)
        {
            String response = "";
            if (PriEngine.InitializeCompany(Settings.Default.Company.Trim(), Settings.Default.User.Trim(), Settings.Default.Password.Trim()) == true)
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