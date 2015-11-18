using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FirstREST.Controllers
{
    using Models;
    using Lib_Primavera.Model;

    public class DashboardController : Controller
    {
        public async Task<ActionResult> Index()
        {
            //Get last month's first and last day
            var today = DateTime.Today;
            var month = new DateTime(today.Year, today.Month, 1);
            var first = month.AddMonths(-1);
            var last = month.AddDays(-1);

            //Get payables
            ViewBag.PayablesValue = await FinancialManager.GetPayables(first, last);

            //Get receivables
            ViewBag.ReceivablesValue = await FinancialManager.GetReceivables(first, last);

            //Get Net Purchases Gloval Value
            ViewBag.NetPurchasesValue = await PurchasesManager.GetNetPurchases(first, last);

            //Get Net Sales Gloval Value
            ViewBag.NetSalesValue = await SalesManager.GetNetSales(first, last);

            //Get Labor Cost per Employee
            ViewBag.LaborCostValue = 0;

            return View();
        }
    }

}


