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
                "AND CabecCompras.TipoDoc='" + documentType + "'"
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
                "AND CabecDoc.TipoDoc='" + documentType + "'"
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
        public static List<Supplier> GetSuppliers()
        {
            // Create an empty list of suppliers:
            List<Model.Supplier> suppliers = new List<Model.Supplier>();

            if (!PriEngine.InitializeCompany(Settings.Default.Company.Trim(), Settings.Default.User.Trim(), Settings.Default.Password.Trim()))
                return suppliers;

            StdBELista list = PriEngine.Engine.Consulta(
                "SELECT Fornecedor, Nome, EncomendasPendentes " +
                "FROM Fornecedores "
                );
            while (!list.NoFim())
            {
                Model.Supplier supplier = new Model.Supplier();

                // Set values
                supplier.ID = list.Valor("Fornecedor");
                supplier.Name = list.Valor("Nome");
                supplier.PendingOrdersValue = list.Valor("EncomendasPendentes");
                // TODO there's no quantity, there's only the value of pending order's value, no currency too

                // Add supplyer to the list:
                suppliers.Add(supplier);

                // Next item:
                list.Seguinte();
            }

            return suppliers;
        }
        public static List<Costumer> GetCostumers()
        {
            // Create an empty list of clients:
            List<Costumer> costumers = new List<Costumer>();

            if (!PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()))
                return costumers;

            StdBELista list = PriEngine.Engine.Consulta(
                "SELECT Fornecedor, Nome, EncomendasPendentes " +
                "FROM Fornecedores "
                );
            while (!list.NoFim())
            {
                Costumer costumer = new Model.Costumer();

                // Set values
                //do secostumer.ID = list.Valor("Fornecedor");

                // Add costumer to the list:
                costumers.Add(costumer);

                // Next item:
                list.Seguinte();
            }

            return costumers;
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
                employee.Salary = new Model.Money(list.Valor("Vencimento"), "Unspecified"); // No currency value
                employee.HiredOn = ParseDate(list, "DataAdmissao");
                employee.FiredOn = ParseDate(list, "DataDemissao");

                // Add employee to the list:
                employees.Add(employee);

                // Next item:
                list.Seguinte();
            }

            return employees;
        }
        public static List<Absence> GetAbsences() // Returns a List of all absences of all employees
        {
            // Create an empty list of absences:
            List<Model.Absence> absences = new List<Model.Absence>();

            if (!PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()))
                return absences;

            // Get Data from Absence of the employee with ID=employeeId
            StdBELista list = PriEngine.Engine.Consulta(
                "SELECT Funcionario, Data FROM CadastroFaltas"
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
        public static List<OvertimeHours> GetOvertimeHours()
        {
            // Create an empty list of absences:
            List<Model.OvertimeHours> overtimeHours = new List<Model.OvertimeHours>();

            if (!PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()))
                return overtimeHours;

            /*
            // Get Data from Absence of the employee with ID=employeeId BD
            StdBELista list = PriEngine.Engine.Consulta(
                "SELECT Funcionario, Data, Tempo FROM CadastroHExtras"
                );
            */

            // Get Data from Absence of the employee with ID=employeeId Not BD
            StdBELista list = PriEngine.Engine.RecursosHumanos.CadastroHorasExtra.LstCadastroHorasExtra();

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

        // Examples
        # region Cliente

        public static List<Model.Cliente> ListaClientes()
        {


            StdBELista objList;

            List<Model.Cliente> listClientes = new List<Model.Cliente>();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {

                //objList = PriEngine.Engine.Comercial.Clientes.LstClientes();

                objList = PriEngine.Engine.Consulta("SELECT Cliente, Nome, Moeda, NumContrib as NumContribuinte, Fac_Mor AS campo_exemplo FROM  CLIENTES");


                while (!objList.NoFim())
                {
                    listClientes.Add(new Model.Cliente
                    {
                        CodCliente = objList.Valor("Cliente"),
                        NomeCliente = objList.Valor("Nome"),
                        Moeda = objList.Valor("Moeda"),
                        NumContribuinte = objList.Valor("NumContribuinte"),
                        Morada = objList.Valor("campo_exemplo")
                    });
                    objList.Seguinte();

                }

                return listClientes;
            }
            else
                return null;
        }

        public static Lib_Primavera.Model.Cliente GetCliente(string codCliente)
        {


            GcpBECliente objCli = new GcpBECliente();


            Model.Cliente myCli = new Model.Cliente();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {

                if (PriEngine.Engine.Comercial.Clientes.Existe(codCliente) == true)
                {
                    objCli = PriEngine.Engine.Comercial.Clientes.Edita(codCliente);
                    myCli.CodCliente = objCli.get_Cliente();
                    myCli.NomeCliente = objCli.get_Nome();
                    myCli.Moeda = objCli.get_Moeda();
                    myCli.NumContribuinte = objCli.get_NumContribuinte();
                    myCli.Morada = objCli.get_Morada();
                    return myCli;
                }
                else
                {
                    return null;
                }
            }
            else
                return null;
        }

        public static Lib_Primavera.Model.RespostaErro UpdCliente(Lib_Primavera.Model.Cliente cliente)
        {
            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();


            GcpBECliente objCli = new GcpBECliente();

            try
            {

                if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
                {

                    if (PriEngine.Engine.Comercial.Clientes.Existe(cliente.CodCliente) == false)
                    {
                        erro.Erro = 1;
                        erro.Descricao = "O cliente não existe";
                        return erro;
                    }
                    else
                    {

                        objCli = PriEngine.Engine.Comercial.Clientes.Edita(cliente.CodCliente);
                        objCli.set_EmModoEdicao(true);

                        objCli.set_Nome(cliente.NomeCliente);
                        objCli.set_NumContribuinte(cliente.NumContribuinte);
                        objCli.set_Moeda(cliente.Moeda);
                        objCli.set_Morada(cliente.Morada);

                        PriEngine.Engine.Comercial.Clientes.Actualiza(objCli);

                        erro.Erro = 0;
                        erro.Descricao = "Sucesso";
                        return erro;
                    }
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir a empresa";
                    return erro;

                }

            }

            catch (Exception ex)
            {
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }

        }


        public static Lib_Primavera.Model.RespostaErro DelCliente(string codCliente)
        {

            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();
            GcpBECliente objCli = new GcpBECliente();


            try
            {

                if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
                {
                    if (PriEngine.Engine.Comercial.Clientes.Existe(codCliente) == false)
                    {
                        erro.Erro = 1;
                        erro.Descricao = "O cliente não existe";
                        return erro;
                    }
                    else
                    {

                        PriEngine.Engine.Comercial.Clientes.Remove(codCliente);
                        erro.Erro = 0;
                        erro.Descricao = "Sucesso";
                        return erro;
                    }
                }

                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir a empresa";
                    return erro;
                }
            }

            catch (Exception ex)
            {
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }

        }



        public static Lib_Primavera.Model.RespostaErro InsereClienteObj(Model.Cliente cli)
        {

            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();


            GcpBECliente myCli = new GcpBECliente();

            try
            {
                if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
                {

                    myCli.set_Cliente(cli.CodCliente);
                    myCli.set_Nome(cli.NomeCliente);
                    myCli.set_NumContribuinte(cli.NumContribuinte);
                    myCli.set_Moeda(cli.Moeda);
                    myCli.set_Morada(cli.Morada);

                    PriEngine.Engine.Comercial.Clientes.Actualiza(myCli);

                    erro.Erro = 0;
                    erro.Descricao = "Sucesso";
                    return erro;
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir empresa";
                    return erro;
                }
            }

            catch (Exception ex)
            {
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }


        }



        #endregion Cliente;   // -----------------------------  END   CLIENTE    -----------------------
        #region Artigo

        public static Lib_Primavera.Model.Artigo GetArtigo(string codArtigo)
        {

            GcpBEArtigo objArtigo = new GcpBEArtigo();
            Model.Artigo myArt = new Model.Artigo();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {

                if (PriEngine.Engine.Comercial.Artigos.Existe(codArtigo) == false)
                {
                    return null;
                }
                else
                {
                    objArtigo = PriEngine.Engine.Comercial.Artigos.Edita(codArtigo);
                    myArt.CodArtigo = objArtigo.get_Artigo();
                    myArt.DescArtigo = objArtigo.get_Descricao();

                    return myArt;
                }

            }
            else
            {
                return null;
            }

        }

        public static List<Model.Artigo> ListaArtigos()
        {

            StdBELista objList;

            Model.Artigo art = new Model.Artigo();
            List<Model.Artigo> listArts = new List<Model.Artigo>();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {

                objList = PriEngine.Engine.Comercial.Artigos.LstArtigos();

                while (!objList.NoFim())
                {
                    art = new Model.Artigo();
                    art.CodArtigo = objList.Valor("artigo");
                    art.DescArtigo = objList.Valor("descricao");

                    listArts.Add(art);
                    objList.Seguinte();
                }

                return listArts;

            }
            else
            {
                return null;

            }

        }

        #endregion Artigo
        #region DocCompra


        public static List<Model.DocCompra> VGR_List()
        {

            StdBELista objListCab;
            StdBELista objListLin;
            Model.DocCompra dc = new Model.DocCompra();
            List<Model.DocCompra> listdc = new List<Model.DocCompra>();
            Model.LinhaDocCompra lindc = new Model.LinhaDocCompra();
            List<Model.LinhaDocCompra> listlindc = new List<Model.LinhaDocCompra>();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {
                objListCab = PriEngine.Engine.Consulta("SELECT id, NumDocExterno, Entidade, DataDoc, NumDoc, TotalMerc, Serie From CabecCompras where TipoDoc='VGR'");
                while (!objListCab.NoFim())
                {
                    dc = new Model.DocCompra();
                    dc.id = objListCab.Valor("id");
                    dc.NumDocExterno = objListCab.Valor("NumDocExterno");
                    dc.Entidade = objListCab.Valor("Entidade");
                    dc.NumDoc = objListCab.Valor("NumDoc");
                    dc.Data = objListCab.Valor("DataDoc");
                    dc.TotalMerc = objListCab.Valor("TotalMerc");
                    dc.Serie = objListCab.Valor("Serie");
                    objListLin = PriEngine.Engine.Consulta("SELECT idCabecCompras, Artigo, Descricao, Quantidade, Unidade, PrecUnit, Desconto1, TotalILiquido, PrecoLiquido, Armazem, Lote from LinhasCompras where IdCabecCompras='" + dc.id + "' order By NumLinha");
                    listlindc = new List<Model.LinhaDocCompra>();

                    while (!objListLin.NoFim())
                    {
                        lindc = new Model.LinhaDocCompra();
                        lindc.IdCabecDoc = objListLin.Valor("idCabecCompras");
                        lindc.CodArtigo = objListLin.Valor("Artigo");
                        lindc.DescArtigo = objListLin.Valor("Descricao");
                        lindc.Quantidade = objListLin.Valor("Quantidade");
                        lindc.Unidade = objListLin.Valor("Unidade");
                        lindc.Desconto = objListLin.Valor("Desconto1");
                        lindc.PrecoUnitario = objListLin.Valor("PrecUnit");
                        lindc.TotalILiquido = objListLin.Valor("TotalILiquido");
                        lindc.TotalLiquido = objListLin.Valor("PrecoLiquido");
                        lindc.Armazem = objListLin.Valor("Armazem");
                        lindc.Lote = objListLin.Valor("Lote");

                        listlindc.Add(lindc);
                        objListLin.Seguinte();
                    }

                    dc.LinhasDoc = listlindc;

                    listdc.Add(dc);
                    objListCab.Seguinte();
                }
            }
            return listdc;
        }


        public static Model.RespostaErro VGR_New(Model.DocCompra dc)
        {
            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();


            GcpBEDocumentoCompra myGR = new GcpBEDocumentoCompra();
            GcpBELinhaDocumentoCompra myLin = new GcpBELinhaDocumentoCompra();
            GcpBELinhasDocumentoCompra myLinhas = new GcpBELinhasDocumentoCompra();

            PreencheRelacaoCompras rl = new PreencheRelacaoCompras();
            List<Model.LinhaDocCompra> lstlindv = new List<Model.LinhaDocCompra>();

            try
            {
                if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
                {
                    // Atribui valores ao cabecalho do doc
                    //myEnc.set_DataDoc(dv.Data);
                    myGR.set_Entidade(dc.Entidade);
                    myGR.set_NumDocExterno(dc.NumDocExterno);
                    myGR.set_Serie(dc.Serie);
                    myGR.set_Tipodoc("VGR");
                    myGR.set_TipoEntidade("F");
                    // Linhas do documento para a lista de linhas
                    lstlindv = dc.LinhasDoc;
                    PriEngine.Engine.Comercial.Compras.PreencheDadosRelacionados(myGR, rl);
                    foreach (Model.LinhaDocCompra lin in lstlindv)
                    {
                        PriEngine.Engine.Comercial.Compras.AdicionaLinha(myGR, lin.CodArtigo, lin.Quantidade, lin.Armazem, "", lin.PrecoUnitario, lin.Desconto);
                    }


                    PriEngine.Engine.IniciaTransaccao();
                    PriEngine.Engine.Comercial.Compras.Actualiza(myGR, "Teste");
                    PriEngine.Engine.TerminaTransaccao();
                    erro.Erro = 0;
                    erro.Descricao = "Sucesso";
                    return erro;
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir empresa";
                    return erro;

                }

            }
            catch (Exception ex)
            {
                PriEngine.Engine.DesfazTransaccao();
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }
        }


        #endregion DocCompra
        #region DocsVenda

        public static Model.RespostaErro Encomendas_New(Model.DocVenda dv)
        {
            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();
            GcpBEDocumentoVenda myEnc = new GcpBEDocumentoVenda();

            GcpBELinhaDocumentoVenda myLin = new GcpBELinhaDocumentoVenda();

            GcpBELinhasDocumentoVenda myLinhas = new GcpBELinhasDocumentoVenda();

            PreencheRelacaoVendas rl = new PreencheRelacaoVendas();
            List<Model.LinhaDocVenda> lstlindv = new List<Model.LinhaDocVenda>();

            try
            {
                if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
                {
                    // Atribui valores ao cabecalho do doc
                    //myEnc.set_DataDoc(dv.Data);
                    myEnc.set_Entidade(dv.Entidade);
                    myEnc.set_Serie(dv.Serie);
                    myEnc.set_Tipodoc("ECL");
                    myEnc.set_TipoEntidade("C");
                    // Linhas do documento para a lista de linhas
                    lstlindv = dv.LinhasDoc;
                    PriEngine.Engine.Comercial.Vendas.PreencheDadosRelacionados(myEnc, rl);
                    foreach (Model.LinhaDocVenda lin in lstlindv)
                    {
                        PriEngine.Engine.Comercial.Vendas.AdicionaLinha(myEnc, lin.CodArtigo, lin.Quantidade, "", "", lin.PrecoUnitario, lin.Desconto);
                    }


                    // PriEngine.Engine.Comercial.Compras.TransformaDocumento(

                    PriEngine.Engine.IniciaTransaccao();
                    PriEngine.Engine.Comercial.Vendas.Actualiza(myEnc, "Teste");
                    PriEngine.Engine.TerminaTransaccao();
                    erro.Erro = 0;
                    erro.Descricao = "Sucesso";
                    return erro;
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir empresa";
                    return erro;

                }

            }
            catch (Exception ex)
            {
                PriEngine.Engine.DesfazTransaccao();
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }
        }



        public static List<Model.DocVenda> Encomendas_List()
        {

            StdBELista objListCab;
            StdBELista objListLin;
            Model.DocVenda dv = new Model.DocVenda();
            List<Model.DocVenda> listdv = new List<Model.DocVenda>();
            Model.LinhaDocVenda lindv = new Model.LinhaDocVenda();
            List<Model.LinhaDocVenda> listlindv = new
            List<Model.LinhaDocVenda>();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {
                objListCab = PriEngine.Engine.Consulta("SELECT id, Entidade, Data, NumDoc, TotalMerc, Serie From CabecDoc where TipoDoc='ECL'");

                while (!objListCab.NoFim())
                {
                    dv = new Model.DocVenda();
                    dv.id = objListCab.Valor("id");
                    dv.Entidade = objListCab.Valor("Entidade");
                    dv.NumDoc = objListCab.Valor("NumDoc");
                    dv.Data = objListCab.Valor("Data");
                    dv.TotalMerc = objListCab.Valor("TotalMerc");
                    dv.Serie = objListCab.Valor("Serie");
                    objListLin = PriEngine.Engine.Consulta("SELECT idCabecDoc, Artigo, Descricao, Quantidade, Unidade, PrecUnit, Desconto1, TotalILiquido, PrecoLiquido from LinhasDoc where IdCabecDoc='" + dv.id + "' order By NumLinha");
                    listlindv = new List<Model.LinhaDocVenda>();

                    while (!objListLin.NoFim())
                    {
                        lindv = new Model.LinhaDocVenda();
                        lindv.IdCabecDoc = objListLin.Valor("idCabecDoc");
                        lindv.CodArtigo = objListLin.Valor("Artigo");
                        lindv.DescArtigo = objListLin.Valor("Descricao");
                        lindv.Quantidade = objListLin.Valor("Quantidade");
                        lindv.Unidade = objListLin.Valor("Unidade");
                        lindv.Desconto = objListLin.Valor("Desconto1");
                        lindv.PrecoUnitario = objListLin.Valor("PrecUnit");
                        lindv.TotalILiquido = objListLin.Valor("TotalILiquido");
                        lindv.TotalLiquido = objListLin.Valor("PrecoLiquido");

                        listlindv.Add(lindv);
                        objListLin.Seguinte();
                    }

                    dv.LinhasDoc = listlindv;
                    listdv.Add(dv);
                    objListCab.Seguinte();
                }
            }
            return listdv;
        }




        public static Model.DocVenda Encomenda_Get(string numdoc)
        {


            StdBELista objListCab;
            StdBELista objListLin;
            Model.DocVenda dv = new Model.DocVenda();
            Model.LinhaDocVenda lindv = new Model.LinhaDocVenda();
            List<Model.LinhaDocVenda> listlindv = new List<Model.LinhaDocVenda>();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {


                string st = "SELECT id, Entidade, Data, NumDoc, TotalMerc, Serie From CabecDoc where TipoDoc='ECL' and NumDoc='" + numdoc + "'";
                objListCab = PriEngine.Engine.Consulta(st);
                dv = new Model.DocVenda();
                dv.id = objListCab.Valor("id");
                dv.Entidade = objListCab.Valor("Entidade");
                dv.NumDoc = objListCab.Valor("NumDoc");
                dv.Data = objListCab.Valor("Data");
                dv.TotalMerc = objListCab.Valor("TotalMerc");
                dv.Serie = objListCab.Valor("Serie");
                objListLin = PriEngine.Engine.Consulta("SELECT idCabecDoc, Artigo, Descricao, Quantidade, Unidade, PrecUnit, Desconto1, TotalILiquido, PrecoLiquido from LinhasDoc where IdCabecDoc='" + dv.id + "' order By NumLinha");
                listlindv = new List<Model.LinhaDocVenda>();

                while (!objListLin.NoFim())
                {
                    lindv = new Model.LinhaDocVenda();
                    lindv.IdCabecDoc = objListLin.Valor("idCabecDoc");
                    lindv.CodArtigo = objListLin.Valor("Artigo");
                    lindv.DescArtigo = objListLin.Valor("Descricao");
                    lindv.Quantidade = objListLin.Valor("Quantidade");
                    lindv.Unidade = objListLin.Valor("Unidade");
                    lindv.Desconto = objListLin.Valor("Desconto1");
                    lindv.PrecoUnitario = objListLin.Valor("PrecUnit");
                    lindv.TotalILiquido = objListLin.Valor("TotalILiquido");
                    lindv.TotalLiquido = objListLin.Valor("PrecoLiquido");
                    listlindv.Add(lindv);
                    objListLin.Seguinte();
                }

                dv.LinhasDoc = listlindv;
                return dv;
            }
            return null;
        }

        #endregion DocsVenda

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