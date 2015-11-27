using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace Dashboard.Controllers.API
{
    using Models;
    using Models.Utils;

    public class SalesController : ApiController
    {
        [ActionName("net_income")]
        public async Task<Double> GetNetIncome(DateTime initialDate, DateTime finalDate)
        {
            return await SalesManager.GetNetSales(initialDate, finalDate);
        }

        [ActionName("top_costumers")]
        public async Task<IEnumerable<SalesManager.TopCostumersLine>> GetTopCostumers(DateTime initialDate, DateTime finalDate, Int32 limit)
        {
            return await SalesManager.GetTopCostumers(initialDate, finalDate, limit);
        }

        [ActionName("sales_by_category")]
        public async Task<IEnumerable<SalesManager.SalesByCategoryLine>> GetSalesByCategory(DateTime initialDate, DateTime finalDate, Int32 limit)
        {
            return await SalesManager.GetSalesByCategory(initialDate, finalDate, limit);
        }

        [ActionName("net_income_by_interval")]
        public async Task<IEnumerable<SalesManager.NetIncomeByIntervalLine>> GetNetIncomeByInterval(DateTime initialDate, DateTime finalDate, TimeIntervalType timeInterval)
        {
            return await SalesManager.GetNetIncomeByInterval(initialDate, finalDate, timeInterval);
        }
    }
}