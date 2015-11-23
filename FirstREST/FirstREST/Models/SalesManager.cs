using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Models
{
    using Models.Primavera.Model;

    public class SalesManager
    {
        private static async Task<Double> GetSaleValues(int year)
        {
            /*DateTime initialDate = new DateTime();
            DateTime finalDate = new DateTime();
            // Create a HTTP Client:
            var client = new HttpClient();

            // Build request URI:
            var uri = BuildRequestURI("/Sale", initialDate, finalDate, "FA");

            // Make a request:
            var response = await client.GetAsync(uri);

            // Get response:
            //var sales = await response.Content.ReadAsAsync<IEnumerable<Sale>>();
            List<Sale> sales = new List<Sale>();

            var query = from item in sales
                        select item.Value.Value;

            return query.Sum();*/
            return 0;
        }

        private static async Task<Double> GetPendingValues(DateTime initialDate, DateTime finalDate)
        {
            /*// Create a HTTP Client:
            var client = new HttpClient();

            // Build request URI:
            var uri = BuildRequestURI("/Receivable", initialDate, finalDate);

            // Make a request:
            var response = await client.GetAsync(uri);

            // Get response:
            //var pendings = await response.Content.ReadAsAsync<IEnumerable<Pending>>();
            var pendings = new List<Pending>();

            var creditNote = from item in pendings
                             where item.DocumentType == "NC"
                             select item.PendingValue.Value;

            var debitNote = from item in pendings
                            where item.DocumentType == "ND"
                            select item.PendingValue.Value;

            return debitNote.Sum() - creditNote.Sum();*/

            return 0;
        }

        public static async Task<Double> GetNetSales(DateTime initialDate, DateTime finalDate)
        {
            int year = 0;
            //var sales = await GetSaleValues(initialDate, finalDate);
            var sales = await GetSaleValues(year);
            var pendings = await GetPendingValues(initialDate, finalDate);

            return sales + pendings;
        }
    }
}