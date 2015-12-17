using System;
using System.Collections.Generic;
using System.Linq;
using Dashboard.Models.Caching;
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

        private static UniqueCache<Employee> _employeeCache;
        private static UniqueCache<Employee> EmployeeCache
        {
            get { return _employeeCache ?? (_employeeCache = new UniqueCache<Employee>(PathConstants.BasePathApiPrimavera, "employee")); }
        }

        private static UniqueCache<GenderCounter> _genderCountCache;
        private static UniqueCache<GenderCounter> GenderCountCache
        {
            get { return _genderCountCache ?? (_genderCountCache = new UniqueCache<GenderCounter>(PathConstants.BasePathApiPrimavera, "gender_count")); }
        }

        private static Cache<Absence> _absenceCache;
        private static Cache<Absence> AbsenceCache
        {
            get { return _absenceCache ?? (_absenceCache = new Cache<Absence>(PathConstants.BasePathApiPrimavera, "absence")); }
        }

        private static Cache<OvertimeHours> _overtimeHoursCache;
        private static Cache<OvertimeHours> OvertimeHoursCache
        {
            get { return _overtimeHoursCache ?? (_overtimeHoursCache = new Cache<OvertimeHours>(PathConstants.BasePathApiPrimavera, "overtime_hour")); }
        }

        //private static Cache<>

        public static Double GetHumanResourcesSpendings(DateTime initialDate, DateTime finalDate)
        {
            //EmployeeCache.UpdateData(initialDate, finalDate);
            //var documents = EmployeeCache.CachedData;

            //NetHelper.MakeRequest<>()

            // Query documents:
            var query = from document in documents
                        where document.HiredOn <= finalDate &&
                              (document.FiredOn >= initialDate || document.FiredOn == DateTime.MinValue)
                        select document.Salary.Value;

            // Calculate spendings total:
            return query.Sum();
            //return 2.0;
        }

        public static IEnumerable<EmployeeCountByIntervalLine> GetEmployeeCountByInterval(DateTime initialDate, DateTime finalDate, TimeIntervalType timeInterval)
        {
            EmployeeCache.UpdateData(initialDate, finalDate);
            var employees = EmployeeCache.CachedData;

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
                    if (employee.HiredOn <= endDate && (employee.FiredOn >= beginDate || employee.FiredOn == DateTime.MinValue))
                        e.Count++;
                }

                output.AddLast(e);
            }
             
            return output.OrderBy(x => x.Date); 
        }

        public static GenderCounter GetGenderCount(DateTime initialDate, DateTime finalDate)
        {
            GenderCountCache.UpdateData(initialDate, finalDate);

            return GenderCountCache.CachedData.FirstOrDefault();
        }

        public static IEnumerable<EmployeeAbsenceCount> GetEmployeeAbsenceCount(DateTime initialDate, DateTime finalDate, Int32 limit)
        {
            AbsenceCache.UpdateData(initialDate, finalDate);
            var absences = AbsenceCache.CachedData;

            // Query:
            var absencesQuery = from absence in absences
                where initialDate <= absence.Date && absence.Date <= finalDate
                group absence by absence.EmployeeId
                into employee
                select new EmployeeAbsenceCount(
                    employee.Key,
                    employee.Select(s => s.EmployeeName).FirstOrDefault(),
                    employee.Count()
                    );

            // Order by descending on Count:
            absencesQuery = absencesQuery.OrderByDescending(employee => employee.Count);

            // Take the top limit:
            return absencesQuery.Take(limit);
        }

        public static IEnumerable<OvertimeHourCount> GetOvertimeHourCount(DateTime initialDate, DateTime finalDate, Int32 limit)
        {
            OvertimeHoursCache.UpdateData(initialDate, finalDate);
            var overtimeHours = OvertimeHoursCache.CachedData; 

            // Query:
            var overtimeHoursQuery = from overtimeHour in overtimeHours 
                                where initialDate <= overtimeHour.Date && overtimeHour.Date <= finalDate
                                group overtimeHour by overtimeHour.EmployeeId
                into employee
                                select new OvertimeHourCount(
                                    employee.Key,
                                    employee.Select(s => s.EmployeeName).FirstOrDefault(),
                                    employee.Count()
                                    );

            // Order by descending on Count:
            overtimeHoursQuery = overtimeHoursQuery.OrderByDescending(employee => employee.Count);

            // Take the top limit:
            return overtimeHoursQuery.Take(limit);
        }
    }
}
