using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Dashboard.Controllers.Primavera
{
    using Models.Primavera;
    using Models.Primavera.Model;

    public class PrimaveraController : ApiController
    {
        [ActionName("absence")]
        public IEnumerable<Absence> GetAbsences(DateTime initialDate, DateTime finalDate)
        {
            return PriIntegration.GetAbsences(initialDate, finalDate);
        }

        [ActionName("gender_count")]
        public GenderCounter GetGenderCount(DateTime initialDate, DateTime finalDate)
        {
            return PriIntegration.GetGenderCount(initialDate, finalDate);
        }

        [ActionName("employee")]
        public IEnumerable<Employee> GetEmployees(DateTime initialDate, DateTime finalDate)
        {
            return PriIntegration.GetEmployees(initialDate, finalDate);
        }

        [ActionName("overtime_hour")]
        public IEnumerable<OvertimeHours> GetOvertimeHours(DateTime initialDate, DateTime finalDate)
        {
            return PriIntegration.GetOvertimeHours(initialDate, finalDate);
        }

        [ActionName("payable")]
        public IEnumerable<Pending> GetPendingPayables(DateTime initialDate, DateTime finalDate)
        {
            return PriIntegration.GetPendingPayables(initialDate, finalDate);
        }

        [ActionName("purchase")]
        public IEnumerable<Purchase> GetPurchases(DateTime initialDate, DateTime finalDate)
        {
            return PriIntegration.GetPurchases(initialDate, finalDate);
        }

        [ActionName("receivable")]
        public IEnumerable<Pending> GetPendingReceivables(DateTime initialDate, DateTime finalDate)
        {
            return PriIntegration.GetPendingReceivables(initialDate, finalDate);
        }

        [ActionName("sale")]
        public IEnumerable<Sale> GetSales(DateTime initialDate, DateTime finalDate)
        {
            return PriIntegration.GetSales(initialDate, finalDate);
        }

        [ActionName("balance_sheet")]
        public Dictionary<int, Dictionary<string, ClassLine>> GetBalanceSheet()
        {
            return PriIntegration.GetBalanceSheet();
        }
    }
}
