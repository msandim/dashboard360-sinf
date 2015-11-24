using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Dashboard.Controllers
{
    using Models;

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

            //Get Net Purchases Global Value
            ViewBag.NetPurchasesValue = await PurchasesManager.GetNetPurchases(first, last);

            //Get Net Sales Global Value
            ViewBag.NetSalesValue = await SalesManager.GetNetIncome(first, last);

            //Get Labor Cost per Employee
            ViewBag.LaborCostValue = 0;

            return View();
        }
    }

}