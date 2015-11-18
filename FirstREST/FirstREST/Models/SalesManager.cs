using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;

namespace FirstREST.Models
{
    using Lib_Primavera.Model;

    public class SalesManager
    {
        private const String BASE_URI = BaseURL.VALUE + "/api";

        private static String BuildRequestURI(String controller, DateTime initialDate, DateTime finalDate)
        {
            return BASE_URI + controller + "?initialDate=" + initialDate.ToString("yyyy-MM-dd") + "&finalDate=" + finalDate.ToString("yyyy-MM-dd");
        }
        private static String BuildRequestURI(String controller, DateTime initialDate, DateTime finalDate, String documentType)
        {
            return BASE_URI + controller + "?initialDate=" + initialDate.ToString("yyyy-MM-dd") + "&finalDate=" + finalDate.ToString("yyyy-MM-dd") + "&documentType=" + documentType;
        }

        private static async Task<Double> GetSaleValues(DateTime initialDate, DateTime finalDate)
        {
            // Create a HTTP Client:
            var client = new HttpClient();

            // Build request URI:
            var uri = BuildRequestURI("/Sale", initialDate, finalDate, "FA");

            // Make a request:
            var response = await client.GetAsync(uri);

            // Get response:
            var sales = await response.Content.ReadAsAsync<IEnumerable<Purchase>>();

            var query = from item in sales
                        select item.Value.Value;

            return query.Sum();
        }

        private static async Task<Double> GetPendingValues(DateTime initialDate, DateTime finalDate)
        {
            // Create a HTTP Client:
            var client = new HttpClient();

            // Build request URI:
            var uri = BuildRequestURI("/Receivable", initialDate, finalDate);

            // Make a request:
            var response = await client.GetAsync(uri);

            // Get response:
            var pendings = await response.Content.ReadAsAsync<IEnumerable<Pending>>();

            var creditNote = from item in pendings
                             where item.DocumentType == "NC"
                             select item.PendingValue.Value;

            var debitNote = from item in pendings
                            where item.DocumentType == "ND"
                            select item.PendingValue.Value;

            return debitNote.Sum() - creditNote.Sum();
        }

        public static async Task<Double> GetNetSales(DateTime initialDate, DateTime finalDate)
        {
            var sales = await GetSaleValues(initialDate, finalDate);
            var pendings = await GetPendingValues(initialDate, finalDate);

            return sales + pendings;
        }
    }
}