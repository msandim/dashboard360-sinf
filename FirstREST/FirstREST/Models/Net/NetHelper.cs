using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Dashboard.Models.Net
{
    public class NetHelper
    {
        private static async Task<IEnumerable<T>> MakeRequest<T>(Path path)
        {
            // Create a HTTP Client:
            var client = new HttpClient();

            // Make a request:
            var response = await client.GetAsync(path.ToString());

            // Get data:
            return await response.Content.ReadAsAsync<IEnumerable<T>>();
        }
    }
}