using System;
using System.Collections.Generic;
using System.Linq;
using Dashboard.Models.Caching;

namespace Dashboard.Models
{
    using Net;
    using Primavera.Model;

    public class FinancialManager
    {
        private static Cache<Pending> _payableCachedData;
        private static Cache<Pending> PayableCachedData
        {
            get { return _payableCachedData ?? (_payableCachedData = new Cache<Pending>(PathConstants.BasePathApiPrimavera, "payable")); }
        }

        private static Cache<Pending> _receivableCachedData;
        private static Cache<Pending> ReceivableCachedData
        {
            get { return _receivableCachedData ?? (_receivableCachedData = new Cache<Pending>(PathConstants.BasePathApiPrimavera, "receivable")); }
        }

        private static Double CalculateSum(IEnumerable<Pending> pendings, DateTime initialDate, DateTime finalDate)
        {
            // Get a query list of all pending values:
            var pendingsQuery = from pending in pendings
                                where initialDate <= pending.DocumentDate && pending.DocumentDate <= finalDate
                                select pending.PendingValue.Value;

            // Sum all the pendings:
            return pendingsQuery.Sum();
        }

        public static Double GetPayables(DateTime initialDate, DateTime finalDate)
        {
            PayableCachedData.UpdateData(initialDate, finalDate);
            return -CalculateSum(PayableCachedData.CachedData, initialDate, finalDate);
        }

        public static Double GetReceivables(DateTime initialDate, DateTime finalDate)
        {
            ReceivableCachedData.UpdateData(initialDate, finalDate);
            return CalculateSum(ReceivableCachedData.CachedData, initialDate, finalDate);
        }
    }
}