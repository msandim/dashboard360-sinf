using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace Dashboard.Controllers.API
{
    using Models;
    using Models.Primavera.Model;

    public class SalesController : ApiController
    {
        [ActionName("net_income")]
        public async Task<Double> GetNetIncome(DateTime initialDate, DateTime finalDate)
        {
            return await SalesManager.GetNetIncome(initialDate, finalDate);
        }

        [ActionName("top_costumers")]
        public async Task<Double> GetTopCostumers(DateTime initialDate, DateTime finalDate)
        {
            return 0.0;
        }

        [ActionName("sales_by_category")]
        public async Task<IEnumerable<SalesManager.SalesByCategoryLine>> GetSalesByCategory(DateTime initialDate, DateTime finalDate, Int32 limit)
        {
            return await SalesManager.GetSalesByCategory(initialDate, finalDate, limit);
        }
    }
}