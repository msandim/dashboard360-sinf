using System;
using System.Linq;

namespace Dashboard.Models
{
    using Net;
    using Primavera.Model;

    public class PurchasesManager
    {
        private static Cache<Purchase> _cachedData;
        private static Cache<Purchase> CachedData
        {
            get { return _cachedData ?? (_cachedData = new Cache<Purchase>(PathConstants.BasePathApiPrimavera, "purchase")); }
        }

        public static Double GetNetPurchases(DateTime initialDate, DateTime finalDate)
        {
            CachedData.UpdateData(initialDate, finalDate);
            var documents = CachedData.CachedData;

            // Query documents:
            var query = from document in documents
                where initialDate <= document.DocumentDate && document.DocumentDate <= finalDate &&
                      (document.DocumentType == "VFA" || document.DocumentType == "VND" ||
                       document.DocumentType == "VNC")
                select document.Value.Value;

            // Calculate the net sales:
            return -query.Sum();
        }
    }
}