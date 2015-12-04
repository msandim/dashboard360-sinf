using System;
using System.Collections.Generic;
using System.Web.Http;
using Dashboard.Models;
using Dashboard.Models.Primavera.Model;
using Dashboard.Models.Utils;

namespace Dashboard.Controllers.API
{
    public class HumanResourcesController : ApiController
    {
        [ActionName("employee_count_by_interval")]
        public IEnumerable<HumanResourcesManager.EmployeeCountByIntervalLine> GetEmployeeCountByInterval(DateTime initialDate, DateTime finalDate, TimeIntervalType timeInterval)
        {
            return HumanResourcesManager.GetEmployeeCountByInterval(initialDate, finalDate, timeInterval);
        }

        [ActionName("gender_count")]
        public GenderCounter GetGenderCount(DateTime initialDate, DateTime finalDate)
        {
            return HumanResourcesManager.GetGenderCount(initialDate, finalDate); 
        }

        [ActionName("absence_count")]
        public IEnumerable<HumanResourcesManager.EmployeeAbsenceCount> GetAbsenceCount(DateTime initialDate, DateTime finalDate, Int32 limit)
        {
            return HumanResourcesManager.GetEmployeeAbsenceCount(initialDate, finalDate, limit);
        }

        [ActionName("overtime_hours_count")]
        public IEnumerable<HumanResourcesManager.OvertimeHourCount> GetOvertimeHoursCount(DateTime initialDate, DateTime finalDate, Int32 limit)
        {
            return HumanResourcesManager.GetOvertimeHourCount(initialDate, finalDate, limit);
        }
    }
}