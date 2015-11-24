using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Models
{
    using Models.Net;
    using Models.Primavera.Model;

    public class FinancialManager
    {
        private static async Task<Double> GetPendings(String action, DateTime initialDate, DateTime finalDate)
        {
            // Build path and make request:
            var path = PathBuilder.Build(PathConstants.BasePathAPIPrimavera, action, initialDate, finalDate);
            var pendings = await NetHelper.MakeRequest<Pending>(path);

            // Get a query list of all pending values:
            var pendingsQuery = from pending in pendings
                                select pending.PendingValue.Value;

            // Sum all the pendings:
            return pendingsQuery.Sum();
        }

        public static async Task<Double> GetPayables(DateTime initialDate, DateTime finalDate)
        {
            return await GetPendings("payable", initialDate, finalDate);
        }

        public static async Task<Double> GetReceivables(DateTime initialDate, DateTime finalDate)
        {
            return await GetPendings("receivable", initialDate, finalDate);
        }
    }
}