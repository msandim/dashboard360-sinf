using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FirstREST.Controllers
{
    public class AbsenceController : ApiController
    {
        //GET api/Absence?employeeId=001
        public IEnumerable<Lib_Primavera.Model.Absence> Get(String employeeId)
        {
            return Lib_Primavera.PriIntegration.GetAbsences(employeeId);
        }
    }
}