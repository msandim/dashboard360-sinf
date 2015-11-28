using System;
using System.Web.Mvc;

using System.Globalization;
using Dashboard.Models.Utils;

namespace Dashboard.Controllers
{
    using Models;

    public class DashboardController : Controller
    {
        public ActionResult Index()
        {
            //Get last month's first and last day
            var today = DateTime.Today;
            var month = new DateTime(today.Year, today.Month, 1);
            var first = month.AddMonths(-1);
            var last = month.AddDays(-1);

            // Store the month we're seeing:
            ViewBag.currentDate = month.AddMonths(-1).ToString("MMMM", new CultureInfo("en-US")) + " " + today.Year;

            //Get payables
            ViewBag.PayablesValue = FormatUtils.FormatCurrency(FinancialManager.GetPayables(first, last));

            //Get receivables
            ViewBag.ReceivablesValue = FormatUtils.FormatCurrency(FinancialManager.GetReceivables(first, last));

            //Get Net Purchases Global Value
            ViewBag.NetPurchasesValue = FormatUtils.FormatCurrency(PurchasesManager.GetNetPurchases(first, last));

            //Get Net Sales Global Value
            ViewBag.NetSalesValue = FormatUtils.FormatCurrency(SalesManager.GetNetSales(first, last));

            //Get Gross Purchases Global Value
            ViewBag.GrossPurchasesValue = FormatUtils.FormatCurrency(PurchasesManager.GetGrossPurchases(first, last));

            //Get Gross Sales Global Value
            ViewBag.GrossSalesValue = FormatUtils.FormatCurrency(SalesManager.GetGrossSales(first, last));

            //Get Labor Cost per Employee
            ViewBag.LaborCostValue = FormatUtils.FormatCurrency(HumanResourcesManager.GetHumanResourcesSpendings(first, last));

            return View();
        }
    }

}