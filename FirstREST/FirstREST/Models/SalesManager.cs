using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Models
{
    using Net;
    using Primavera.Model;
    using Utils;

    public class SalesManager
    {
        public class SalesByCategoryLine
        {
            public String FamilyId { get; set; }
            public String FamilyDescription { get; set; }
            public Double Total { get; set; }

            public SalesByCategoryLine(String familyId, String familyDescription, Double total)
            {
                FamilyId = familyId;
                FamilyDescription = familyDescription;
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
        public class NetIncomeByIntervalLine
        {
            public DateTime InitialDate { get; set; }
            public DateTime FinalDate { get; set; }
            public Double Total { get; set; }

            public NetIncomeByIntervalLine(DateTime initialDate, DateTime finalDate, Double total)
            {
                InitialDate = initialDate;
                FinalDate = finalDate;
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
            var query = from document in enumerable
                        where document.DocumentType == "FA" || document.DocumentType == "ND" || document.DocumentType == "NC"
                        select document.Value.Value;

            // Calculate the net sales:
            return query.Sum();
        }
        public static async Task<IEnumerable<NetIncomeByIntervalLine>> GetNetIncomeByInterval(DateTime initialDate, DateTime finalDate, TimeIntervalType timeInterval)
        {
            // Build path and make request:
            var path = PathBuilder.Build(PathConstants.BasePathApiPrimavera, "sale", initialDate, finalDate);
            var documents = await NetHelper.MakeRequest<Sale>(path);

            // Query:
            return from document in documents
                   where document.DocumentType == "FA" || document.DocumentType == "NC" || document.DocumentType == "ND"
                   group document by
                       new DateTime(document.DocumentDate.Year,
                           timeInterval == TimeIntervalType.Month ? document.DocumentDate.Month : 1, 1)
                into interval
                   select new NetIncomeByIntervalLine(
                       interval.Key, timeInterval == TimeIntervalType.Month ? interval.Key.AddMonths(1) : interval.Key.AddYears(1), interval.Select(x => x.Value.Value).Sum()
                       );
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
                                        family.Select(s => s.Product.FamilyDescription).FirstOrDefault(),
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