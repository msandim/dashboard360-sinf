using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FirstREST.Controllers
{
    public class OvertimeHoursController : ApiController
    {
  
        //GET api/OvertimeHours?employeeId=001
        public IEnumerable<Lib_Primavera.Model.OvertimeHours> Get(String employeeId)
        {
            return Lib_Primavera.PriIntegration.GetOvertimeHours(employeeId);
        }
    }
}
