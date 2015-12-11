using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Dashboard.Controllers.API
{
    using Models;
    using Models.Utils;

    public class PurchasesController : ApiController
    {
        [ActionName("net_purchases")]
        public Double GetNetIncome(DateTime initialDate, DateTime finalDate)
        {
            return PurchasesManager.GetNetPurchases(initialDate, finalDate);
        }

        [ActionName("top_suppliers")]
        public IEnumerable<PurchasesManager.TopSuppliersLine> GetTopCostumers(DateTime initialDate, DateTime finalDate, Int32 limit)
        {
            return PurchasesManager.GetTopSuppliers(initialDate, finalDate, limit);
        }

        [ActionName("top_products")]
        public IEnumerable<PurchasesManager.TopProductsLine> getTopProducts(DateTime initialDate, DateTime finalDate, Int32 limit)
        {
            return PurchasesManager.GetTopProducts(initialDate, finalDate, limit);
        }

        [ActionName("purchases_by_category")]
        public IEnumerable<PurchasesManager.PurchasesByCategoryLine> GetSalesByCategory(DateTime initialDate, DateTime finalDate, Int32 limit)
        {
            return PurchasesManager.GetPurchasesByCategory(initialDate, finalDate, limit);
        }

        [ActionName("net_purchases_by_interval")]
        public IEnumerable<PurchasesManager.NetPurchasesByIntervalLine> GetNetIncomeByInterval(DateTime initialDate, DateTime finalDate, TimeIntervalType timeInterval)
        {
            return PurchasesManager.GetNetPurchasesByInterval(initialDate, finalDate, timeInterval);
        }
    }
}