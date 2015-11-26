using System;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Models
{
    using Net;
    using Primavera.Model;

    public class PurchasesManager
    {
        public static async Task<Double> GetNetPurchases(DateTime initialDate, DateTime finalDate)
        {
            // Build path and make request:
            var path = PathBuilder.Build(PathConstants.BasePathApiPrimavera, "purchase", initialDate, finalDate);
            var documents = await NetHelper.MakeRequest<Purchase>(path);

            // Query documents:
            var query = from document in documents
                        where document.DocumentType == "VFA" || document.DocumentType == "VND" || document.DocumentType == "VNC"
                        select document.Value.Value;

            // Calculate the net sales:
            return -query.Sum();
        }
    }
}