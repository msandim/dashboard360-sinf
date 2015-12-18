using System;
using System.Collections.Generic;
using System.Linq;
using Dashboard.Models.Caching;
using Dashboard.Models.Utils;

namespace Dashboard.Models
{
    using Net;
    using Primavera.Model;

    public class PurchasesManager
    {
        public class PurchasesByCategoryLine
        {
            public String FamilyId { get; set; }
            public String FamilyDescription { get; set; }
            public Double Total { get; set; }

            public PurchasesByCategoryLine(String familyId, String familyDescription, Double total)
            {
                FamilyId = familyId;
                FamilyDescription = familyDescription;
                Total = total;
            }
        }
        public class TopSuppliersLine
        {
            public String ClientId { get; set; }
            public String ClientName { get; set; }
            public Double Total { get; set; }

            public TopSuppliersLine(string clientId, string clientName, double total)
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
        public class NetPurchasesByIntervalLine
        {
            public DateTime Date { get; set; }
            public Double Total { get; set; }

            public NetPurchasesByIntervalLine(DateTime date, Double total)
            {
                Date = date;
                Total = total;
            }
        }


        private static Cache<Purchase> _cachedData;
        private static Cache<Purchase> CachedData
        {
            get { return _cachedData ?? (_cachedData = new Cache<Purchase>(PathConstants.BasePathApiPrimavera, "purchase")); }
        }

        public static Double GetNetPurchases(DateTime initialDate, DateTime finalDate)
        {
            CachedData.UpdateData(initialDate, finalDate);
            var documents = CachedData.CachedData;

            // Query documents:
            var query = from document in documents
                where initialDate <= document.DocumentDate && document.DocumentDate <= finalDate &&
                      (document.DocumentType == "VFA" || document.DocumentType == "VND" ||
                       document.DocumentType == "VNC")
                select document.Value.Value;

            // Calculate the net sales:
            return -query.Sum();
        }
        public static Double GetGrossPurchases(DateTime initialDate, DateTime finalDate)
        {
            CachedData.UpdateData(initialDate, finalDate);
            var documents = CachedData.CachedData;

            // Query documents:
            var query = from document in documents
                        where initialDate <= document.DocumentDate && document.DocumentDate <= finalDate &&
                              (document.DocumentType == "VFA" || document.DocumentType == "VND" ||
                               document.DocumentType == "VNC")
                        select document.Value.Value * (1.0 + document.Iva);

            // Calculate the net sales:
            return -query.Sum();
        }

        public static IEnumerable<NetPurchasesByIntervalLine> GetNetPurchasesByInterval(DateTime initialDate, DateTime finalDate, TimeIntervalType timeInterval)
        {
            CachedData.UpdateData(initialDate, finalDate);
            var documents = CachedData.CachedData;

            // Query:
            var query = from document in documents
                        where initialDate <= document.DocumentDate && document.DocumentDate <= finalDate &&
                              (document.DocumentType == "VFA" || document.DocumentType == "VND" || document.DocumentType == "VNC")
                        group document by
                            new DateTime(document.DocumentDate.Year,
                                timeInterval == TimeIntervalType.Month ? document.DocumentDate.Month : 1, 1)
                into interval
                        select new NetPurchasesByIntervalLine(
                            interval.Key, -interval.Select(x => x.Value.Value).Sum() // Negative value because purchases come as negative
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
                        select new NetPurchasesByIntervalLine(date, 0.0);

            var finalQuery = from e in empty
                             join realData in query on e.Date equals realData.Date into g
                             from realDataJoin in g.DefaultIfEmpty()
                             select new NetPurchasesByIntervalLine(e.Date, realDataJoin == null ? 0.0 : realDataJoin.Total);

            return finalQuery.OrderBy(x => x.Date);
        }
        public static IEnumerable<PurchasesByCategoryLine> GetPurchasesByCategory(DateTime initialDate, DateTime finalDate, Int32 limit)
        {
            CachedData.UpdateData(initialDate, finalDate);
            var documents = CachedData.CachedData;

            // Query:
            var topPurchasesQuery = from document in documents
                                where initialDate <= document.DocumentDate && document.DocumentDate <= finalDate &&
                                      (document.DocumentType == "VFA" || document.DocumentType == "VND" || document.DocumentType == "VNC")
                                group document by document.Product.FamilyId
                into family
                                select new PurchasesByCategoryLine(
                                    family.Key,
                                    family.Select(s => s.Product.FamilyDescription).FirstOrDefault(),
                                    - family.Select(s => s.Value.Value).Sum() // Negative value in purchases
                                    );

            // Order by descending on total:
            topPurchasesQuery = topPurchasesQuery.OrderByDescending(sale => sale.Total);

            // Take the top limit:
            return topPurchasesQuery.Take(limit);
        }
        public static IEnumerable<TopSuppliersLine> GetTopSuppliers(DateTime initialDate, DateTime finalDate, Int32 limit)
        {
            CachedData.UpdateData(initialDate, finalDate);
            var documents = CachedData.CachedData;

            // Perform query to order sales by descending order on value:
            var topSuppliersQuery = from document in documents
                                  where initialDate <= document.DocumentDate && document.DocumentDate <= finalDate &&
                                        (document.DocumentType == "VFA" || document.DocumentType == "VND" || document.DocumentType == "VNC")
                                  group document by document.SupplierId
                into client
                                  select new TopSuppliersLine(
                                      client.Key,
                                      client.Select(s => s.SupplierName).FirstOrDefault(),
                                      - client.Select(s => s.Value.Value).Sum() // Negative value in purchases
                                      );

            // Order by descending on total:
            topSuppliersQuery = topSuppliersQuery.OrderByDescending(client => client.Total);

            // Take the top limit:
            return topSuppliersQuery.Take(limit);
        }
        public static IEnumerable<TopProductsLine> GetTopProducts(DateTime initialDate, DateTime finalDate, Int32 limit)
        {
            CachedData.UpdateData(initialDate, finalDate);
            var documents = CachedData.CachedData;

            // Perform query to order sales by descending order on value:
            var topProductsQuery = from document in documents
                                   where initialDate <= document.DocumentDate && document.DocumentDate <= finalDate &&
                                         (document.DocumentType == "VFA" || document.DocumentType == "VND" || document.DocumentType == "VNC")
                                   group document by document.Product.Id
                                       into product
                                       select new TopProductsLine(
                                          product.Key,
                                          product.Select(s => s.Product.Description).FirstOrDefault(),
                                          product.Select(s => s.Product.FamilyDescription).FirstOrDefault(),
                                          - product.Select(s => s.Value.Value).Sum()
                                          );

            // Order by descending on total:
            topProductsQuery = topProductsQuery.OrderByDescending(product => product.Total);

            // Take the top limit:
            return topProductsQuery.Take(limit);
        }
    }
}