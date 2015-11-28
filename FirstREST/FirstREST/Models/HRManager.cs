using System;
using System.Linq;

namespace Dashboard.Models
{
    using Net;
    using Primavera.Model;

    public class HRManager
    {
        private static Cache<Employee> _cachedData;
        private static Cache<Employee> CachedData
        {
            get { return _cachedData ?? (_cachedData = new Cache<Employee>(PathConstants.BasePathApiPrimavera, "employee")); }
        }

        public static Double GetHumanResourcesSpendings(DateTime initialDate, DateTime finalDate)
        {
            CachedData.UpdateData(initialDate, finalDate);
            var documents = CachedData.CachedData;

            // Query documents:
            var query = from document in documents
                where document.HiredOn <= finalDate &&
                      (document.FiredOn >= initialDate || document.FiredOn == DateTime.MinValue)
                select document.Salary.Value;

            // Calculate spendings total:
            return query.Sum();
        }
    }
}
 