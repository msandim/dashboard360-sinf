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
        public class TopCostumersLine
        {
            public String ClientId { get; set; }
            public String ClientName { get; set; }
            public Double Total { get; set; }

            public TopCostumersLine(string clientId, string clientName, double total)
            {
                ClientId = clientId;
                ClientName = clientName;
                Total = total;
            }
        }

        public static async Task<Double> GetNetSales(DateTime initialDate, DateTime finalDate)
        {
            // Build path and make request:
            var path = PathBuilder.Build(PathConstants.BasePathApiPrimavera, "sale", initialDate, finalDate);
            var documents = await NetHelper.MakeRequest<Sale>(path);

            var enumerable = documents as IList<Sale> ?? documents.ToList();

            // Query documents:
            var query =   from document in enumerable
                          where document.DocumentType == "FA" || document.DocumentType == "ND" || document.DocumentType == "NC"
                          select document.Value.Value;

            // Calculate the net sales:
            return query.Sum();
        }
        private static async Task<Double> GetNetIncomeByInterval(DateTime initialDate, DateTime finalDate)
        {
            //var netSales = await GetNetSales(initialDate, finalDate);


            return 0.0;
        }

        public static async Task<IEnumerable<SalesByCategoryLine>> GetSalesByCategory(DateTime initialDate, DateTime finalDate, Int32 limit)
        {
            // Build path and make request:
            var path = PathBuilder.Build(PathConstants.BasePathApiPrimavera, "sale", initialDate, finalDate);
            var documents = await NetHelper.MakeRequest<Sale>(path);

            // Query:
            var topSalesQuery = from document in documents
                        where document.DocumentType == "FA" || document.DocumentType == "NC" || document.DocumentType == "ND"
                        group document by document.Product.FamilyId into family
                        select new SalesByCategoryLine(
                            family.Key, 
                            family.Select(s => s.Value.Value).Sum()
                            );

            // Order by descending on total:
            topSalesQuery = topSalesQuery.OrderByDescending(sale => sale.Total);

            // Take the top limit:
            return topSalesQuery.Take(limit);
        }

        public static async Task<IEnumerable<TopCostumersLine>> GetTopCostumers(DateTime initialDate, DateTime finalDate, Int32 limit)
        {
            // Build path and make request:
            var path = PathBuilder.Build(PathConstants.BasePathApiPrimavera, "sale", initialDate, finalDate);
            var documents = await NetHelper.MakeRequest<Sale>(path);

            // Perform query to order sales by descending order on value:
            var topClientsQuery = from document in documents
                                    where document.DocumentType == "FA" || document.DocumentType == "NC" || document.DocumentType == "ND"
                                    group document by document.ClientId into client
                                    select new TopCostumersLine(
                                        client.Key,
                                        client.Select(s => s.ClientName).FirstOrDefault(),
                                        client.Select(s => s.Value.Value).Sum()
                                    );

            // Order by descending on total:
            topClientsQuery = topClientsQuery.OrderByDescending(client => client.Total);

            // Take the top limit:
            return topClientsQuery.Take(limit);
        }
    }
}