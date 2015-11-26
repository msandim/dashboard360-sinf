using System;
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

            // Store the month we're seeing:
            ViewBag.currentMonth = today.Month;

            //Get payables
            ViewBag.PayablesValue = await FinancialManager.GetPayables(first, last);

            //Get receivables
            ViewBag.ReceivablesValue = await FinancialManager.GetReceivables(first, last);

            //Get Net Purchases Global Value
            ViewBag.NetPurchasesValue = await PurchasesManager.GetNetPurchases(first, last);

            //Get Net Sales Global Value
            ViewBag.NetSalesValue = await SalesManager.GetNetSales(first, last);

            //Get Labor Cost per Employee
            ViewBag.LaborCostValue = await HRManager.GetHumanResourcesSpendings(first, last);

            return View();
        }
    }

}