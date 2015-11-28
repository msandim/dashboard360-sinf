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
        public IEnumerable<HumanResourcesManager.EmployeeAbsenceCount> GetAbsenceCount()
        {
            var list = new LinkedList<HumanResourcesManager.EmployeeAbsenceCount>();
            list.AddLast(new HumanResourcesManager.EmployeeAbsenceCount("id", "name_absence", 5));

            return list;
        }

        [ActionName("overtime_hours_count")]
        public IEnumerable<HumanResourcesManager.OvertimeHourCount> GetOvertimeHoursCount()
        {
            var list = new LinkedList<HumanResourcesManager.OvertimeHourCount>();
            list.AddLast(new HumanResourcesManager.OvertimeHourCount("id", "name_over", 6));

            return list;
        }
    }
}