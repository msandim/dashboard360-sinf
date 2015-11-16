using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FirstREST.Lib_Primavera;
using FirstREST.Lib_Primavera.Model;

namespace FirstREST.Controllers
{
    public class EmployeeController : ApiController
    {
        // GET api/employee
        public IEnumerable<Employee> Get(DateTime initialDate, DateTime finalDate)
        {
            return PriIntegration.GetEmployees(initialDate, finalDate);
        }
    }
}
