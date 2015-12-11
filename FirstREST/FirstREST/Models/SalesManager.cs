using System;
using System.Collections.Generic;
using System.Linq;
using Dashboard.Models.Caching;

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
        public class TopProductsLine
        {
            public String ProductId { get; set; }
            public String ProductName { get; set; }
            public String ProductFamily { get; set; }
            public Double Total { get; set; }

            public TopProductsLine(string productId, string productName, string productFamily, double total)
            {
                ProductId = productId;
                ProductName = productName;
                ProductFamily = productFamily;
                Total = total;
            }
        }
        public class NetSalesByIntervalLine
        {
            public DateTime Date { get; set; }
            public Double Total { get; set; }

            public NetSalesByIntervalLine(DateTime date, Double total)
            {
                Date = date;
                Total = total;
            }
        }

        private static Cache<Sale> _cachedData;
        private static Cache<Sale> CachedData
        {
            get { return _cachedData ?? (_cachedData = new Cache<Sale>(PathConstants.BasePathApiPrimavera, "sale")); }
        }

        public static Double GetNetSales(DateTime initialDate, DateTime finalDate)
        {
            CachedData.UpdateData(initialDate, finalDate); 
            var documents = CachedData.CachedData;
             
            // Query documents:
            var query = from document in documents
                where initialDate <= document.DocumentDate && document.DocumentDate <= finalDate &&
                      (document.DocumentType == "FA" || document.DocumentType == "ND" || document.DocumentType == "NC")
                select document.Value.Value;

            // Calculate the net sales:
            return query.Sum(); 
        }
        public static Double GetGrossSales(DateTime initialDate, DateTime finalDate)
        {
            CachedData.UpdateData(initialDate, finalDate);
            var documents = CachedData.CachedData;

            // Query documents:
            var query = from document in documents
                        where initialDate <= document.DocumentDate && document.DocumentDate <= finalDate &&
                              (document.DocumentType == "FA" || document.DocumentType == "ND" || document.DocumentType == "NC")
                        select document.Value.Value * (1.0 + document.Iva);

            return query.Sum();
        }

        public static IEnumerable<NetSalesByIntervalLine> GetNetSalesByInterval(DateTime initialDate, DateTime finalDate, TimeIntervalType timeInterval)
        {
            CachedData.UpdateData(initialDate, finalDate);
            var documents = CachedData.CachedData;

            // Query:
            var query = from document in documents
                where initialDate <= document.DocumentDate && document.DocumentDate <= finalDate &&
                      (document.DocumentType == "FA" || document.DocumentType == "ND" || document.DocumentType == "NC")
                group document by
                    new DateTime(document.DocumentDate.Year,
                        timeInterval == TimeIntervalType.Month ? document.DocumentDate.Month : 1, 1)
                into interval
                select new NetSalesByIntervalLine(
                    interval.Key, interval.Select(x => x.Value.Value).Sum()
                    );

            var dateTimes = new List<DateTime>();
            if (timeInterval == TimeIntervalType.Year)
            {
                var temp = new DateTime(initialDate.Year, 1, 1);
                for (int i = 0; i < finalDate.Year - initialDate.Year + 1; i++)
                    dateTimes.Add(temp.AddYears(i));
            }
            else
            {
                var temp = new DateTime(initialDate.Year, initialDate.Month, 1);
                int months = ((finalDate.Year - initialDate.Year) * 12) + finalDate.Month - initialDate.Month + 1;
                for (int i = 0; i < months; i++)
                    dateTimes.Add(temp.AddMonths(i));
            }

            // Empty:
            var empty = from date in dateTimes
                        select new NetSalesByIntervalLine(date, 0.0);

            var finalQuery = from e in empty
                             join realData in query on e.Date equals realData.Date into g
                             from realDataJoin in g.DefaultIfEmpty()
                             select new NetSalesByIntervalLine(e.Date, realDataJoin == null ? 0.0 : realDataJoin.Total);

            return finalQuery.OrderBy(x => x.Date);
        }

        public static IEnumerable<SalesByCategoryLine> GetSalesByCategory(DateTime initialDate, DateTime finalDate, Int32 limit)
        {
            CachedData.UpdateData(initialDate, finalDate);
            var documents = CachedData.CachedData;

            // Query:
            var topSalesQuery = from document in documents
                where initialDate <= document.DocumentDate && document.DocumentDate <= finalDate &&
                      (document.DocumentType == "FA" || document.DocumentType == "ND" || document.DocumentType == "NC")
                group document by document.Product.FamilyId
                into family
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

        public static IEnumerable<TopCostumersLine> GetTopCostumers(DateTime initialDate, DateTime finalDate, Int32 limit)
        {
            CachedData.UpdateData(initialDate, finalDate);
            var documents = CachedData.CachedData;

            // Perform query to order sales by descending order on value:
            var topClientsQuery = from document in documents
                where initialDate <= document.DocumentDate && document.DocumentDate <= finalDate &&
                      (document.DocumentType == "FA" || document.DocumentType == "ND" || document.DocumentType == "NC")
                group document by document.ClientId
                into client
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

        public static IEnumerable<TopProductsLine> GetTopProducts(DateTime initialDate, DateTime finalDate, Int32 limit)
        {
            CachedData.UpdateData(initialDate, finalDate);
            var documents = CachedData.CachedData;

            // Perform query to order sales by descending order on value:
            // Perform query to order sales by descending order on value:
            var topProductsQuery = from document in documents
                                  where initialDate <= document.DocumentDate && document.DocumentDate <= finalDate &&
                                        (document.DocumentType == "FA" || document.DocumentType == "ND" || document.DocumentType == "NC")
                                  group document by document.Product.Id
                                      into product
                                       select new TopProductsLine(
                                          product.Key,
                                          product.Select(s => s.Product.Description).FirstOrDefault(),
                                          product.Select(s => s.Product.FamilyDescription).FirstOrDefault(),
                                          product.Select(s => s.Value.Value).Sum()
                                          );

            // Order by descending on total:
            topProductsQuery = topProductsQuery.OrderByDescending(product => product.Total);

            // Take the top limit:
            return topProductsQuery.Take(limit);
        }
    }
}