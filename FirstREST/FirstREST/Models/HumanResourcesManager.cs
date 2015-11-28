using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dashboard.Models.Utils;

namespace Dashboard.Models
{
    using Net;
    using Primavera.Model;

    public class HumanResourcesManager
    {
        public class EmployeeCountByIntervalLine
        {
            public DateTime Date { get; set; }
            public Int32 Count { get; set; }

            public EmployeeCountByIntervalLine(DateTime date, Int32 count)
            {
                Date = date;
                Count = count;
            }
        }
        public class EmployeeAbsenceCount
        {
            public String EmployeeId { get; set; }
            public String EmployeeName { get; set; }
            public Int32 Count { get; set; }

            public EmployeeAbsenceCount(String employeeId, String employeeName, Int32 count)
            {
                EmployeeId = employeeId;
                EmployeeName = employeeName;
                Count = count;
            }
        }
        public class OvertimeHourCount
        {
            public String EmployeeId { get; set; }
            public String EmployeeName { get; set; }
            public Int32 Count { get; set; }

            public OvertimeHourCount(String employeeId, String employeeName, Int32 count)
            {
                EmployeeId = employeeId;
                EmployeeName = employeeName;
                Count = count;
            }
        }

        private static Cache<Employee> _cachedData;
        private static Cache<Employee> CachedData
        {
            get { return _cachedData ?? (_cachedData = new Cache<Employee>(PathConstants.BasePathApiPrimavera, "employee")); }
        }

        private static Cache<GenderCounter> _genderCounterCache;
        private static Cache<GenderCounter> GenderCounterCachedData
        {
            get { return _genderCounterCache ?? (_genderCounterCache = new Cache<GenderCounter>(PathConstants.BasePathApiPrimavera, "gender_count")); }
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

        public static IEnumerable<EmployeeCountByIntervalLine> GetEmployeeCountByInterval(DateTime initialDate, DateTime finalDate, TimeIntervalType timeInterval)
        {
            CachedData.UpdateData(initialDate, finalDate);
            var employees = CachedData.CachedData;

            // Query:
            var employeesQuery = from employee in employees
                where employee.HiredOn <= finalDate &&
                      (employee.FiredOn >= initialDate || employee.FiredOn == DateTime.MinValue)
                select employee;

            var dateTimes = new List<DateTime>();
            if (timeInterval == TimeIntervalType.Year)
            {
                var temp = new DateTime(initialDate.Year, 1, 1);
                for (int i = 0; i < finalDate.Year - initialDate.Year + 1; i++)
                    dateTimes.Add(temp.AddYears(i));
            }
            else
            {
                var temp = new DateTime(initialDate.Year, initialDate.Month, 1);
                int months = ((finalDate.Year - initialDate.Year) * 12) + finalDate.Month - initialDate.Month + 1;
                for (int i = 0; i < months; i++)
                    dateTimes.Add(temp.AddMonths(i));
            }

            // Empty:
            var empty = from date in dateTimes
                        select new EmployeeCountByIntervalLine(date, 0);

            var output = new LinkedList<EmployeeCountByIntervalLine>();
            foreach (var e in empty)
            {
                DateTime beginDate = e.Date;
                DateTime endDate = timeInterval == TimeIntervalType.Month ? beginDate.AddMonths(1).AddDays(-1) : new DateTime(beginDate.Year, 12, 31);

                foreach (var employee in employeesQuery)
                {
                    if (employee.HiredOn <= finalDate && (employee.FiredOn >= beginDate || employee.FiredOn == DateTime.MinValue))
                        e.Count++;
                }

                output.AddLast(e);
            }

            return output.OrderBy(x => x.Date); 
        }

        public static GenderCounter GetGenderCount(DateTime initialDate, DateTime finalDate)
        {
            GenderCounterCachedData.UpdateData(initialDate, finalDate);

            return GenderCounterCachedData.CachedData.FirstOrDefault(x => x.InitialDate == initialDate && x.FinalDate == finalDate);
        }
    }
}
