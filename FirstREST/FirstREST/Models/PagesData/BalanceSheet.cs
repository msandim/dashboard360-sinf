using System;
using System.Collections.Generic;
using Dashboard.Models.Primavera.Model;

namespace Dashboard.Models.PagesData
{
    public class BalanceSheet
    {
        public String moeda { get; set; }
        public Dictionary<String, Metric> metrics;

        public BalanceSheet(Dictionary<int, Dictionary<string, ClassLine>> balance_sheet)       
        {
            //prepare a list with the metrics
            metrics = new Dictionary<string, Metric>();

            prepareDirectMetrics();
            
            foreach (KeyValuePair<int, Dictionary<string, ClassLine>> entry in balance_sheet) //for each year
            {
                moeda = entry.Value["11"].moeda; //methods for the first class line - calculate moeda
                Year year_data; //for each year calculate all the metrics
               
                foreach (KeyValuePair<String, Metric> metric in metrics)
                {
                    Metric in_analysis_metric = metric.Value; //for each metric

                    ClassLine class_data;
                    if(entry.Value.TryGetValue(in_analysis_metric.class_code, out class_data)) //go get the class line obtained from primavera
                    {
                        year_data = calculateMetric(entry.Key, class_data, in_analysis_metric.left_T_side); //calculate the metric for the year
                        in_analysis_metric.years_data.Add(year_data); //add year to metric
                    }
                    //metrics[metric.Key] = in_analysis_metric; //consult metric to the dictionary for update -> overrides old value if exists
                }
            }

            prepareIndirectMetrics();
        }

        private void prepareDirectMetrics()
        {
            //-- 1 -- Meios financeiros liquidos
            metrics.Add("cash", new Metric("cash", "11", true)); //************************
            metrics.Add("depositos ordem", new Metric("depositos ordem", "12", true));
            metrics.Add("depositos prazo", new Metric("depositos prazo", "13", true));
            metrics.Add("outros depositos", new Metric("outros depositos", "14", true));
            metrics.Add("titulos negociaveis", new Metric("titulos negociaveis", "15", true));
            metrics.Add("outras aplicacoes", new Metric("outras aplicacoes", "18", true));
            metrics.Add("ajustes de aplicacoes", new Metric("ajustes de aplicacoes", "19", true));

            //-- 2 -- Contas a receber e a pagar
            metrics.Add("accounts_receivable", new Metric("accounts_receivable", "21", true)); //************************
            metrics.Add("accounts_payable", new Metric("accounts_payable", "22", true)); //************************
            metrics.Add("emprestimos obtidos", new Metric("emprestimos obtidos", "23", true)); //***********************
            metrics.Add("estados e outros entes", new Metric("estados e outros entes", "24", true)); //********************
            metrics.Add("acionistas", new Metric("acionistas", "25", true));
            metrics.Add("outros devedores e credores", new Metric("outros devedores e credores", "26", true));
            metrics.Add("acrescimos e diferimentos", new Metric("acrescimos e diferimentos", "27", true));
            metrics.Add("ajuste dividas a receber", new Metric("ajuste dividas a receber", "28", true));
            metrics.Add("provisoes", new Metric("provisoes", "29", true));

            //-- 3 -- Inventários e activos biologicos                     
            metrics.Add("purchases", new Metric("purchases", "31", true)); //***************************
            metrics.Add("mercadorias", new Metric("mercadorias", "32", true)); //*******************************
            metrics.Add("produtos acabados", new Metric("produtos acabados", "33", true));
            metrics.Add("subprodutos", new Metric("subprodutos", "34", true));
            metrics.Add("produtos e trabalhos", new Metric("produtos e trabalhos", "35", true));
            metrics.Add("materias primas", new Metric("materias primas", "36", true));
            metrics.Add("adiantamento", new Metric("adiantamento", "37", true));
            metrics.Add("regularizacao", new Metric("regularizacao", "38", true));
            metrics.Add("ajustamentos", new Metric("ajustamentos", "39", true));

            //-- 4 -- Investimentos
            metrics.Add("investimentos", new Metric("investimentos", "41", true));
            metrics.Add("imobilizacoes1", new Metric("imobilizacoes1", "42", true));
            metrics.Add("imobilizacoes2", new Metric("imobilizacoes2", "43", true));
            metrics.Add("imobilizacoes3", new Metric("imobilizacoes3", "44", true));
            metrics.Add("imobilizacoes4", new Metric("imobilizacoes4", "45", true));
            metrics.Add("imobilizacoes5", new Metric("imobilizacoes5", "46", true));
            metrics.Add("amortizacoes", new Metric("amortizacoes", "48", true));
            metrics.Add("ajust finan", new Metric("ajust finan", "49", true));

            //-- 5 -- capital proprio e resultados                   
            metrics.Add("capital", new Metric("capital", "51", true));
            metrics.Add("acoes quotas proprias", new Metric("acoes quotas proprias", "52", true));
            metrics.Add("prestacoes", new Metric("prestacoes", "53", true));
            metrics.Add("premio emissao acoes", new Metric("premio emissao acoes", "54", true));
            metrics.Add("ajustes partes cap", new Metric("ajustes partes cap", "55", true));
            metrics.Add("reservas reav", new Metric("reservas reav", "56", true));
            metrics.Add("reservas", new Metric("reservas", "57", true));
            metrics.Add("resultados transitados", new Metric("resultados transitados", "59", true));

            //-- 6 -- gastos
            metrics.Add("custo merc. vend", new Metric("Custo merc. vend", "61", true)); //***************************
            metrics.Add("forn serv externos", new Metric("Forn serv externos", "62", true));
            metrics.Add("impostos", new Metric("impostos", "63", true));
            metrics.Add("custos pessoal", new Metric("custos pessoal", "64", true)); //***************************
            metrics.Add("outros custos", new Metric("outros custos", "65", true));
            metrics.Add("amortizacoes e ajust externo", new Metric("amortizacoes e ajust externo", "66", true));
            metrics.Add("provisoes exercicio", new Metric("provisoes exercicio", "67", true));
            metrics.Add("custos perdas financeiras", new Metric("custos perdas financeiras", "68", true));
            metrics.Add("custos perdas extra", new Metric("custos perdas extra", "69", true));

            //-- 7 -- vendas
            metrics.Add("sales", new Metric("sales", "71", true)); //**************************************
            metrics.Add("prestacoes serv", new Metric("prestacoes serv", "72", true));
            metrics.Add("proveitos suplementares", new Metric("prestacoes serv", "73", true));
            metrics.Add("subsidios explo", new Metric("subsidios explo", "74", true));
            metrics.Add("trabalhos propria", new Metric("trabalhos propria", "75", true));
            metrics.Add("outros proveitos e ganhos", new Metric("outros proveitos e ganhos", "76", true));
            metrics.Add("reversoes", new Metric("reversoes", "77", true));
            metrics.Add("proveitos e ganhos", new Metric("proveitos e ganhos", "78", true));
            metrics.Add("proveitos e ganhos extra", new Metric("proveitos e ganhos extra", "79", true));

            //-- 8 -- resultados
            metrics.Add("resultados operacionais", new Metric("resultados operacionais", "81", true));
            metrics.Add("resultados financeiros", new Metric("resultados financeiros", "82", true));
            metrics.Add("resultados correntes", new Metric("resultados correntes", "83", true));
            metrics.Add("resultados extra", new Metric("resultados extra", "84", true));
            metrics.Add("resultados antes imposto", new Metric("resultados antes imposto", "85", true));
            metrics.Add("imposto sem rendimento do exercicio", new Metric("imposto sem rendimento do exercicio", "86", true));
            metrics.Add("resultado liquido do exercicio", new Metric("resultado liquido do exercicio", "88", true));
            metrics.Add("dividendos antecipados", new Metric("dividendos antecipados", "89", true));

            //-- 9 -- mais coisas fofas
            metrics.Add("contas reflectidas", new Metric("contas reflectidas", "91", true));
            metrics.Add("periodizacao custos", new Metric("periodizacao custos", "92", true));
            metrics.Add("existencias", new Metric("existencias", "93", true));
            metrics.Add("centro custo", new Metric("centro custo", "94", true));
            metrics.Add("custo producao", new Metric("custo producao", "95", true));
            metrics.Add("desvios", new Metric("desvios", "96", true));
            metrics.Add("dif incorporacao", new Metric("dif incorporacao", "97", true));
            metrics.Add("resultados funcoes", new Metric("resultados funcoes", "98", true));
        }

        private Year calculateMetric(int year, ClassLine class_data, bool t_left_side)
        {
            Year year_data = new Year();
            year_data.year = year;

            for (int i = 0; i < 12; i++)
            {
                Double month = class_data.values[i + 1] - class_data.values[i + 17]; //CR - DB 
                year_data.addMonth(month);
            }
            return year_data;
        }

        private void prepareIndirectMetrics()
        {
            //-- 1 -- Meios financeiros liquidos
            calculateIndirectMetric("1 meios financeiros liquidos", new List<string>(get1MeiosFinanceirosLiquidosSubmetrics()));

            //-- 2 -- Contas a receber e a pagar
            calculateIndirectMetric("2 contas a receber e a pagar", new List<string>(get2ContasReceberPagarSubmetrics()));

            //-- 3 -- Inventários e activos biologicos  //*******************************************************************************        
            calculateIndirectMetric("3 inventario", new List<string>(get3InventorySubmetrics()));

            //-- 4 -- Investimentos 
            calculateIndirectMetric("4 investimentos", new List<string>(get4InvestimentosSubmetrics()));

            //-- 5 -- capital proprio e resultados //*************************************************************************************                   
            calculateIndirectMetric("5 capital proprio", new List<string>(get5EquitySubmetrics()));

            //-- 6 -- gastos //********************************************************************************************************** 
            calculateIndirectMetric("6 gastos", new List<string>(get6GastosSubmetrics()));

            //-- 7 -- vendas //********************************************************************************************************** 
            calculateIndirectMetric("7 vendas", new List<string>(get7VendasSubmetrics()));

            //-- 8 -- resultados
            calculateIndirectMetric("8 resultados", new List<string>(get8ResultadosSubmetrics()));

            //-- 9 -- mais coisas fofas
            calculateIndirectMetric("9 coisas", new List<string>(get9CoisasSubmetrics()));

            //----------------------------------ASSETS--------------------------------------

            //-- TOTAL CURRENT ASSETS
            calculateIndirectMetric("total_current_assets", new List<string>(getTotalCurrentAssetsSubmetrics())); 

            //-- TOTAL NON CURRENT ASSETS
            calculateIndirectMetric("total_non_current_assets", new List<string>(getTotalNonCurrentAssetsSubmetrics()));

            //-- TOTAL ASSETS
            calculateIndirectMetric("total_assets", new List<string>(getTotalAssetsSubmetrics()));

            //-- TOTAL CURRENT LIABILITIES
            calculateIndirectMetric("total_current_liabilities", new List<string>(getTotalCurrentLiabilitiesSubmetrics()));

            //-- TOTAL LONG TERM DEBT
            calculateIndirectMetric("total_long_term_debt", new List<string>(getTotalLongTermDebtSubmetrics()));

            //-- TOTAL LIABILITIES
            calculateIndirectMetric("total_liabilities", new List<string>(getTotalLiabilitiesSubmetrics()));

        }

        private void calculateIndirectMetric(String metric_name, List<String> submetrics_names)
        {
            Metric result_metric = new Metric(metric_name, metric_name, true);

            foreach (String submetric_name in submetrics_names)
            {
                Metric submetric = metrics[submetric_name];
                result_metric = result_metric + submetric;
            }

            metrics.Add(metric_name, result_metric);
        }

        private string[] getTotalCurrentLiabilitiesSubmetrics()
        {
            String[] res = new String[] 
            { 
                "accounts_payable",
                "estados e outros entes"
                
            };

            return res;
        }

        private string[] getTotalLongTermDebtSubmetrics()
        {
            String[] res = new String[] 
            { 
                "emprestimos obtidos"
            };

            return res;          
        }

        private string[] getTotalLiabilitiesSubmetrics()
        {
            String[] res = new String[] 
            { 
                "total_current_liabilities",
                "total_long_term_debt"
            };

            return res;
        }

        private string[] getTotalCurrentAssetsSubmetrics()
        {
             String[] res = new String[] 
            { 
                "1 meios financeiros liquidos",
                "accounts_receivable",
                "3 inventario"
            };

            return res;
        }

        private string[] getTotalNonCurrentAssetsSubmetrics()
        {
            String[] res = new String[] 
            { 
                "4 investimentos"
            };

            return res;
        }

        private string[] getTotalAssetsSubmetrics()
        {
            String[] res = new String[] 
            { 
                "total_current_assets",
                "total_non_current_assets"

            };

            return res;
        }

        private string[] get1MeiosFinanceirosLiquidosSubmetrics()
        {
            String[] res = new String[] 
            { 
                "cash", 
                "depositos ordem", 
                "depositos prazo", 
                "outros depositos", 
                "titulos negociaveis", 
                "outras aplicacoes",
                "ajustes de aplicacoes"
            };

            return res;
        }

        private string[] get2ContasReceberPagarSubmetrics()
        {
            String[] res = new String[] 
            { 
                "accounts_receivable", 
                "accounts_payable",
                "emprestimos obtidos",
                "estados e outros entes",
                "acionistas",
                "outros devedores e credores",
                "acrescimos e diferimentos",
                "ajuste dividas a receber",
                "provisoes"
            };

            return res;
        }

        private string[] get3InventorySubmetrics()
        {
            String[] res = new String[] 
            { 
                "purchases", 
                "mercadorias", 
                "produtos acabados", 
                "subprodutos", 
                "produtos e trabalhos", 
                "materias primas",
                "adiantamento",
                "regularizacao", 
                "ajustamentos"
            };

            return res;
        }

        private string[] get4InvestimentosSubmetrics()
        {
            String[] res = new String[] 
            { 
                "investimentos",
                "imobilizacoes1",
                "imobilizacoes2",
                "imobilizacoes3",
                "imobilizacoes4",
                "imobilizacoes5",
                "amortizacoes",
                "ajust finan"
            };

            return res;
        }

        private string[] get5EquitySubmetrics()
        {
            String[] res = new String[] 
            { 
                "capital",
                "acoes quotas proprias",
                "prestacoes",
                "premio emissao acoes",
                "ajustes partes cap",
                "reservas reav",
                "reservas",
                "resultados transitados"
            };

            return res;
        }

        private string[] get6GastosSubmetrics()
        {
            String[] res = new String[] 
            { 
                "custo merc. vend",
                "forn serv externos",
                "impostos",
                "custos pessoal",
                "outros custos",
                "amortizacoes e ajust externo",
                "provisoes exercicio",
                "custos perdas financeiras",
                "custos perdas extra" 
            };

            return res;
        }

        private string[] get7VendasSubmetrics()
        {
            String[] res = new String[] 
            { 
                "sales", 
                "prestacoes serv",
                "proveitos suplementares",
                "subsidios explo",
                "trabalhos propria",
                "outros proveitos e ganhos",
                "reversoes",
                "proveitos e ganhos",
                "proveitos e ganhos extra"
            };

            return res;
        }

        private string[] get8ResultadosSubmetrics()
        {
            String[] res = new String[] 
            { 
                "resultados operacionais", 
                "resultados financeiros",
                "resultados correntes",
                "resultados extra",
                "resultados antes imposto",
                "imposto sem rendimento do exercicio",
                "resultado liquido do exercicio",
                "dividendos antecipados"
            };

            return res;
        }

        private string[] get9CoisasSubmetrics()
        {
            String[] res = new String[] 
            { 
                "contas reflectidas", 
                "periodizacao custos",
                "existencias",
                "centro custo",
                "custo producao",
                "desvios",
                "dif incorporacao",
                "resultados funcoes"
            };

            return res;
        }
    }
}