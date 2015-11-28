using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Dashboard.Controllers.API
{
    using Models;
    using Models.Utils;

    public class SalesController : ApiController
    {
        [ActionName("net_sales")]
        public Double GetNetSales(DateTime initialDate, DateTime finalDate)
        {
            return SalesManager.GetNetSales(initialDate, finalDate);
        }

        [ActionName("top_costumers")]
        public IEnumerable<SalesManager.TopCostumersLine> GetTopCostumers(DateTime initialDate, DateTime finalDate, Int32 limit)
        {
            return SalesManager.GetTopCostumers(initialDate, finalDate, limit);
        }

        [ActionName("sales_by_category")]
        public IEnumerable<SalesManager.SalesByCategoryLine> GetSalesByCategory(DateTime initialDate, DateTime finalDate, Int32 limit)
        {
            return SalesManager.GetSalesByCategory(initialDate, finalDate, limit);
        }

        [ActionName("net_sales_by_interval")]
        public IEnumerable<SalesManager.NetSalesByIntervalLine> GetNetSalesByInterval(DateTime initialDate, DateTime finalDate, TimeIntervalType timeInterval)
        {
            return SalesManager.GetNetSalesByInterval(initialDate, finalDate, timeInterval);
        }
    }
}