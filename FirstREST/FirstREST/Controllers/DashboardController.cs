using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using FirstREST.Lib_Primavera.Model;


namespace FirstREST.Controllers
{
    public class DashboardController : Controller
    {
        public ActionResult Index()
        {
            //Get last month's first and last day
            var today = DateTime.Today;
            var month = new DateTime(today.Year, today.Month, 1);
            var first = month.AddMonths(-1);
            var last = month.AddDays(-1);

            //Get payables
            PayableController payable_controller = new PayableController();
            IEnumerable<Pending> pendings_p = payable_controller.Get(first, last);
            double payables = 0;
            foreach (Pending pending in pendings_p)
            {
                payables += pending.PendingValue.Value;
            }

            //Get receivables
            ReceivableController receivable_controller = new ReceivableController();
            IEnumerable<Pending> pendings_r = receivable_controller.Get(first, last);
            double receivables = 0;
            foreach (Pending pending in pendings_r)
            {
                receivables += pending.PendingValue.Value;
            }

            //Get Net Purchases Gloval Value
            PurchaseController purchase_controller = new PurchaseController();
            IEnumerable<Purchase> purchases = purchase_controller.Get(first, last, "VFA");
            double net_purchases_global_values = 0;
            foreach (Purchase purchase in purchases)
            {
                net_purchases_global_values += purchase.Value.Value;
            }

            //Get Net Sales Gloval Value
            SaleController sale_controller = new SaleController();
            IEnumerable<Sale> sales = sale_controller.Get(first, last, "ECL");
            double net_sales_global_values = 0;
            foreach (Sale sale in sales)
            {
                net_sales_global_values += sale.Value.Value;
            }

            //Not done yet
            float labor_cost_per_employee = 5;


            ViewData["payables"] = payables;
            ViewData["receivables"] = receivables;
            ViewData["net_purchases_global_values"] = net_purchases_global_values;
            ViewData["net_sales_global_values"] = net_sales_global_values;
            ViewData["labor_cost_per_employee"] = labor_cost_per_employee;
            return View();
        }
    }

}
