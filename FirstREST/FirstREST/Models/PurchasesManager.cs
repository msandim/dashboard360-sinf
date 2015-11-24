using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Models
{
    using Models.Net;
    using Models.Primavera.Model;

    public class PurchasesManager
    {
        private static async Task<Double> GetPurchaseValues(DateTime initialDate, DateTime finalDate)
        {
            // Build path and make request:
            var path = PathBuilder.Build(PathConstants.BasePathAPIPrimavera, "purchase", initialDate, finalDate, "VFA");
            var purchases = await NetHelper.MakeRequest<Purchase>(path);

            // Make a query, to select all monetary values of the Purchases:
            var query = from item in purchases
                        select item.Value.Value;

            // Calculate the sum:
            return query.Sum();
        }

        private static async Task<Double> GetPendingValues(DateTime initialDate, DateTime finalDate)
        {
            // Build path and make request:
            var path = PathBuilder.Build(PathConstants.BasePathAPIPrimavera, "payable", initialDate, finalDate);
            var pendings = await NetHelper.MakeRequest<Pending>(path);

            // Make a query, to select all monetary values of the Credit Notes:
            var creditNotes = from item in pendings
                              where item.DocumentType == "VNC"
                              select item.PendingValue.Value;

            // Make a query, to select all monetary values of the Debit Notes:
            var debitNotes = from item in pendings
                             where item.DocumentType == "VND"
                             select item.PendingValue.Value;

            // Calculate the difference between the sum of the debit notes and the sum of the credit notes:
            return debitNotes.Sum() - creditNotes.Sum();
        }

        public static async Task<Double> GetNetPurchases(DateTime initialDate, DateTime finalDate)
        {
            // Get purchases sum:
            var purchasesSum = await GetPurchaseValues(initialDate, finalDate);

            // Get the difference between debit and credit notes:
            var pendingsSum = await GetPendingValues(initialDate, finalDate);

            // Calculate the sum:
            return purchasesSum + pendingsSum;
        }
    }
}