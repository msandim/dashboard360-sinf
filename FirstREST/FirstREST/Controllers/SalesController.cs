using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Dashboard.Controllers
{
    using Models;

    public class SalesController : Controller
    {
        public async Task<ActionResult> Index()
        {
            ViewBag.NetSalesValue = await SalesManager.GetNetSales(DateTime.Parse("2014-04-01"), DateTime.Parse("2016-06-01"));

            return View();
        }
    }
}
