using System;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Models
{
    using Net;
    using Primavera.Model;

    public class HRManager
    {
        public static async Task<Double> GetHumanResourcesSpendings(DateTime initialDate, DateTime finalDate)
        {
            // Build path and make request:
            var path = PathBuilder.Build(PathConstants.BasePathApiPrimavera, "employee", initialDate, finalDate);
            var documents = await NetHelper.MakeRequest<Employee>(path);

            // Query documents:
            var query = from document in documents
                        select document.Salary.Value;

            // Calculate spendings total:
            return query.Sum();
        }
    }
}
