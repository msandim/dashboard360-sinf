using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;

namespace FirstREST.Models
{
    using Lib_Primavera.Model;

    public class PurchasesManager
    {
        private const String BASE_URI = BaseURL.VALUE + "/api/Purchase";

        private static String BuildRequestURI(DateTime initialDate, DateTime finalDate, String documentType)
        {
            return BASE_URI + "?initialDate=" + initialDate.ToString("yyyy-MM-dd") + "&finalDate=" + finalDate.ToString("yyyy-MM-dd") + "&documentType=" + documentType;
        }

        public static async Task<Double> GetNetPurchases(DateTime initialDate, DateTime finalDate, String documentType)
        {
            // Create a HTTP Client:
            var client = new HttpClient();

            // Build request URI:
            var uri = BuildRequestURI(initialDate, finalDate, documentType);

            // Make a request:
            var response = await client.GetAsync(uri);

            // Get response:
            var purchases = await response.Content.ReadAsAsync<IEnumerable<Sale>>();

            // Get a query list of all net sale values:
            var netPurchasesQuery = from purchase in purchases
                                    select purchase.Value.Value;

            // Sum all values in the query list:
            return netPurchasesQuery.Sum();
        }
    }
}