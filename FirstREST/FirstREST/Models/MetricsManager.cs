using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace Dashboard.Models
{
    using Net;
    using PagesData;
    using Primavera.Model;
    
    public class MetricsManager
    {
        public static async Task<Dictionary<int, Dictionary<string, ClassLine>>> MakeRequestAsync(Path path)
        {
                // Create a HTTP Client:
                var client = new HttpClient();

                // Make a request:
                var response = await client.GetAsync(path.ToString());

                // Get data:
                return await response.Content.ReadAsAsync<Dictionary<int, Dictionary<string, ClassLine>>>();
        }

        public static Dictionary<int, Dictionary<string, ClassLine>> MakeRequest(Path path)
        {
                var task = Task.Run(() => MakeRequestAsync(path));
                return task.Result;
        }


        public static BalanceSheet GetBalanceSheet()
        {
            return new BalanceSheet(MakeRequest(PathBuilder.Build(PathConstants.BasePathApiPrimavera, "balance_sheet")).OrderBy(s => s.Key));
        }
    }
}
