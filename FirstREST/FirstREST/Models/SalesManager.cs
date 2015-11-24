using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Models
{
    using Net;
    using Primavera.Model;

    public class SalesManager
    {
        public class SalesByCategoryLine
        {
            public String FamilyId { get; set; }
            public Double Total { get; set; }

            public SalesByCategoryLine(String familyId, Double total)
            {
                FamilyId = familyId;
                Total = total;
            }
        }

        private static async Task<Double> GetNetSales(DateTime initialDate, DateTime finalDate)
        {
            // Build path and make request:
            var path = PathBuilder.Build(PathConstants.BasePathAPIPrimavera, "sale", initialDate, finalDate, "FA");
            var sales = await NetHelper.MakeRequest<Sale>(path);

            // Make a query, to select all monetary values of the Sales:
            var query = from item in sales
                        select item.Value.Value;

            // Calculate the sum:
            return query.Sum();
        }
        private static async Task<Double> GetPendingValues(DateTime initialDate, DateTime finalDate)
        {
            // Build path and make request:
            var path = PathBuilder.Build(PathConstants.BasePathAPIPrimavera, "receivable", initialDate, finalDate);
            var pendings = await NetHelper.MakeRequest<Pending>(path);

            // Make a query, to select all monetary values of the Credit Notes:
            var creditNotes = from item in pendings
                             where item.DocumentType == "NC"
                             select item.PendingValue.Value;

            // Make a query, to select all monetary values of the Debit Notes:
            var debitNotes = from item in pendings
                            where item.DocumentType == "ND"
                            select item.PendingValue.Value;

            // Calculate the difference between the sum of the debit notes and the sum of the credit notes:
            return debitNotes.Sum() - creditNotes.Sum();
        }
        public static async Task<Double> GetNetIncome(DateTime initialDate, DateTime finalDate)
        {
            // Get net sales:
            var netSales = await GetNetSales(initialDate, finalDate);

            // Get the difference between debit and credit notes:
            var pendingsSum = await GetPendingValues(initialDate, finalDate);

            // Calculate the sum:
            return netSales + pendingsSum;
        }

        public static async Task<IEnumerable<SalesByCategoryLine>> GetSalesByCategory(DateTime initialDate, DateTime finalDate, Int32 limit)
        {
            // Build path and make request:
            var path = PathBuilder.Build(PathConstants.BasePathAPIPrimavera, "sale", initialDate, finalDate, "ECL"); // TODO check type of document
            var sales = await NetHelper.MakeRequest<Sale>(path);

            // Perform query to order sales by descending order on value:
            var topSalesQuery = from sale in sales
                                group sale by sale.Product.FamilyId into family
                                select new SalesByCategoryLine(family.Key, family.Select(s => s.Value.Value).Sum());

            // Order by descending on total:
            topSalesQuery = topSalesQuery.OrderByDescending(sale => sale.Total);

            // Take the top limit:
            return topSalesQuery.Take(limit);
        }
    }
}