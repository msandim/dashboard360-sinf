using System;

namespace Dashboard.Models
{
    public class DashboardManager
    {
        public class ButtonValues
        {
            public Double Payables { get; set; }
            public Double Receivables { get; set; }
            public Double NetPurchases { get; set; }
            public Double NetSales { get; set; }
            public Double GrossPurchases { get; set; }
            public Double GrossSales { get; set; }
            public Double LaborCostValue { get; set; }
            public String Currency { get; set; }
        }

        public static ButtonValues GetButtonValues(DateTime initialDate, DateTime finalDate)
        {
            ButtonValues buttonValues = new ButtonValues();

            buttonValues.Payables = FinancialManager.GetPayables(initialDate, finalDate);
            buttonValues.Receivables = FinancialManager.GetReceivables(initialDate, finalDate);
            buttonValues.NetPurchases = PurchasesManager.GetNetPurchases(initialDate, finalDate);
            buttonValues.NetSales = SalesManager.GetNetSales(initialDate, finalDate);
            buttonValues.GrossPurchases = PurchasesManager.GetGrossPurchases(initialDate, finalDate);
            buttonValues.GrossSales = SalesManager.GetGrossSales(initialDate, finalDate);
            buttonValues.LaborCostValue = HumanResourcesManager.GetHumanResourcesSpendings(initialDate, finalDate);
            buttonValues.Currency = "€";

            return buttonValues;
        }
    }
}