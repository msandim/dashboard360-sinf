using System;
using System.Collections.Generic;

namespace Dashboard.Models.Primavera.Model
{
    public class BalanceSheet
    {
        public String moeda { get; set; }
        public int yearLevel { get; set; }
        public int yearLevelValue { get; set; }

        public Money cash { get; set; }
        public Money accounts_receivable { get; set; }
        public Money inventory { get; set; }

        public Money total_current_assets { get; set; }
        
        public Money total_non_current_assets { get; set; }
        
        public Money total_assets { get; set; }


        public Money accounts_payable { get; set; }
        public Money current_liabilities { get; set; }
        
        public Money long_term_debt { get; set; }
        
        public Money total_liabilities { get; set; }

        public Money net_worth { get; set; }

        public Money total_liabilities_and_net_worth { get; set; }

        //-----------------------------------------

        public Money net_sales { get; set; }
        public Money cost_of_goods_sold { get; set; }
        public Money gross_profit_on_sales { get; set; }

        //-----------------------------------------
        //7 - 6 ganhos de nao sei que
        //...revenues
        BalanceSheet(Dictionary<string, ClassLine> balance_sheet)
        {

        }
    }
}