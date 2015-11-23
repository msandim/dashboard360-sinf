using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Models
{
    using Models.Primavera.Model;

    public class FinancialManager
    {
        public static async Task<Double> GetPayables(DateTime initialDate, DateTime finalDate)
        {
            /*// Create a HTTP Client:
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
            return payablesQuery.Sum();*/
            return 0;
        }

        public static async Task<Double> GetReceivables(DateTime initialDate, DateTime finalDate)
        {
            /*// Create a HTTP Client:
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
            return receivablesQuery.Sum();*/
            return 0;
        }
    }
}