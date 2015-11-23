using System;
using System.Collections.Generic;
using Dashboard.Models.Primavera.Model;

namespace Dashboard.Models.PagesData
{
    public class BalanceSheet
    {
        public String moeda { get; set; }
        Dictionary<String, Metric> metrics;

        public BalanceSheet(Dictionary<int, Dictionary<string, ClassLine>> balance_sheet) 
        {
            //prepare a list with the metrics
            Dictionary<String, Metric> metrics = new Dictionary<string, Metric>();
            
            metrics.Add("cash", new Metric("cash", "11", true));
            metrics.Add("accounts_receivable", new Metric("accounts_receivable", "21", true));
            metrics.Add("accounts_payable", new Metric("accounts_payable", "22", true));
            //metrics.Add("inventory", new Metric("inventory", "31", true));
            //metrics.Add("total_current_assets", new Metric("total_current_assets", "?", true));
            //metrics.Add("total_non_current_assets", new Metric("total_non_current_assets", "?", true));
            //metrics.Add("total_assets", new Metric("total_assets", "?", true));

            //metrics.Add("current_liabilities", new Metric("current_liabilities", "?", false));
            //metrics.Add("long_term_debt", new Metric("long_term_debt", "?", false));
            //metrics.Add("total_liabilities", new Metric("total_liabilities", "?", false));
            //metrics.Add("net_worth", new Metric("net_worth", "?", false));
            //metrics.Add("total_liabilities_and_net_worth", new Metric("total_liabilities_and_net_worth", "?", false));

            //calculate the metrics
            foreach (KeyValuePair<int, Dictionary<string, ClassLine>> entry in balance_sheet) //for each year
            {
                Year year_data;

                //calculate each metric - cash
                foreach (KeyValuePair<String, Metric> metric in metrics)
                {
                    Metric in_analysis_metric = metric.Value;

                    ClassLine class_data;
                    entry.Value.TryGetValue(in_analysis_metric.class_code, out class_data);
                    year_data = calculateMetric(entry.Key, class_data, in_analysis_metric.left_T_side);
                    in_analysis_metric.years_data.Add(year_data);
                }
            }
        }

        private Year calculateMetric(int year, ClassLine class_data, bool t_left_side)
        {
            Year year_data = new Year();
            year_data.year = year;

            for (int i = 0; i < 12; i++)
            {
                //Double month = class_data.values[i + 16] - class_data.values[i + 1]; //CR - DB 
                //year_data.addMonth(month);
            }

            return year_data;
        }
    }
}