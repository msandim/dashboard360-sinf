using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;

namespace FirstREST.Models
{
    using Lib_Primavera.Model;

    public class FinancialManager
    {
        private const String BASE_URI = BaseURL.VALUE + "/api/";

        private static String BuildRequestURI(String controller, DateTime initialDate, DateTime finalDate)
        {
            return BASE_URI + controller + "?initialDate=" + initialDate.ToString("yyyy-MM-dd") + "&finalDate=" + finalDate.ToString("yyyy-MM-dd");
        }

 
        public static async Task<Double> GetPayables(DateTime initialDate, DateTime finalDate)
        {
            // Create a HTTP Client:
            var client = new HttpClient();

            // Build request URI:
            var uri = BuildRequestURI("Payable", initialDate, finalDate);

            // Make a request:
            var response = await client.GetAsync(uri);

            // Get response:
            var pendings = await response.Content.ReadAsAsync<IEnumerable<Pending>>();

            // Get a query list of all net sale values:
            var payablesQuery = from pending in pendings
                                select pending.PendingValue.Value;

            // Sum all values in the query list:
            return payablesQuery.Sum();
        }

        public static async Task<Double> GetReceivables(DateTime initialDate, DateTime finalDate)
        {
            // Create a HTTP Client:
            var client = new HttpClient();

            // Build request URI:
            var uri = BuildRequestURI("Receivable", initialDate, finalDate);

            // Make a request:
            var response = await client.GetAsync(uri);

            // Get response:
            var pendings = await response.Content.ReadAsAsync<IEnumerable<Pending>>();

            // Get a query list of all net sale values:
            var receivablesQuery = from pending in pendings
                                    select pending.PendingValue.Value;

            // Sum all values in the query list:
            return receivablesQuery.Sum();
        }
    }
}