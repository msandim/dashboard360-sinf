using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Models
{
    using Models.Primavera.Model;

    public class PurchasesManager
    {
        private static async Task<Double> GetPurchaseValues(DateTime initialDate, DateTime finalDate)
        {
            /*// Create a HTTP Client:
            var client = new HttpClient();

            // Build request URI:
            var uri = BuildRequestURI("/Purchase", initialDate, finalDate, "VFA");

            // Make a request:
            var response = await client.GetAsync(uri);

            // Get response:
            //var purchases = await response.Content.ReadAsAsync<IEnumerable<Purchase>>();
            var purchases = new List<Purchase>();

            var query = from item in purchases
                        select item.Value.Value;

            return query.Sum();*/
            return 0;
        }

        private static async Task<Double> GetPendingValues(DateTime initialDate, DateTime finalDate)
        {
            /*// Create a HTTP Client:
            var client = new HttpClient();

            // Build request URI:
            var uri = BuildRequestURI("/Payable", initialDate, finalDate);

            // Make a request:
            var response = await client.GetAsync(uri);

            // Get response:
            //var pendings = await response.Content.ReadAsAsync<IEnumerable<Pending>>();
            var pendings = new List<Pending>();

            var creditNote = from item in pendings
                             where item.DocumentType == "VNC"
                             select item.PendingValue.Value;

            var debitNote = from item in pendings
                            where item.DocumentType == "VND"
                            select item.PendingValue.Value;

            return debitNote.Sum() - creditNote.Sum();*/
            return 0;
        }

        public static async Task<Double> GetNetPurchases(DateTime initialDate, DateTime finalDate)
        {
            var purchases = await GetPurchaseValues(initialDate, finalDate);
            var pendings = await GetPendingValues(initialDate, finalDate);

            return purchases + pendings;
        }
    }
}