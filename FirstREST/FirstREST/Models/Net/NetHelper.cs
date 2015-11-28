using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Dashboard.Models.Net
{
    public class NetHelper
    {
        public static async Task<IEnumerable<T>> MakeRequestAsync<T>(Path path)
        {
            // Create a HTTP Client:
            var client = new HttpClient();

            // Make a request:
            var response = await client.GetAsync(path.ToString());

            // Get data:
            return await response.Content.ReadAsAsync<IEnumerable<T>>();
        }

        public static IEnumerable<T> MakeRequest<T>(Path path)
        {
            var task = Task.Run(() => MakeRequestAsync<T>(path));
            return task.Result;
        }
    }
}