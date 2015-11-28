using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Dashboard.Controllers.API
{
    using Models;
    using Models.Utils;

    public class SalesController : ApiController
    {
        [ActionName("net_income")]
        public Double GetNetIncome(DateTime initialDate, DateTime finalDate)
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

        [ActionName("net_income_by_interval")]
        public IEnumerable<SalesManager.NetIncomeByIntervalLine> GetNetIncomeByInterval(DateTime initialDate, DateTime finalDate, TimeIntervalType timeInterval)
        {
            return SalesManager.GetNetIncomeByInterval(initialDate, finalDate, timeInterval);
        }
    }
}