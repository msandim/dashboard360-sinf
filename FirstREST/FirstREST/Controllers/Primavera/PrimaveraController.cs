using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Dashboard.Controllers.Primavera
{
    using Dashboard.Models.Primavera;
    using Dashboard.Models.Primavera.Model;

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
        public IEnumerable<Purchase> GetPurchases(DateTime initialDate, DateTime finalDate, String documentType)
        {
            return PriIntegration.GetPurchases(initialDate, finalDate, documentType);
        }

        [ActionName("receivable")]
        public IEnumerable<Pending> GetPendingReceivables(DateTime initialDate, DateTime finalDate)
        {
            return PriIntegration.GetPendingReceivables(initialDate, finalDate);
        }

        [ActionName("sale")]
        public IEnumerable<Sale> GetSales(DateTime initialDate, DateTime finalDate, String documentType)
        {
            return PriIntegration.GetSales(initialDate, finalDate, documentType);
        }

        [ActionName("balance_sheet")]
        public IEnumerable<ClassLine> GetBalanceSheet()
        {
            return PriIntegration.GetBalanceSheet();
        }
    }
}
