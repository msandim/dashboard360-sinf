using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FirstREST.Controllers
{
    using Lib_Primavera;
    using Lib_Primavera.Model;

    public class OvertimeHourController : ApiController
    {
        //GET api/OvertimeHours
        public IEnumerable<OvertimeHours> Get(DateTime initialDate, DateTime finalDate)
        {
            return PriIntegration.GetOvertimeHours(initialDate, finalDate);
        }

        //GET api/OvertimeHours/001
        /*
        public IEnumerable<OvertimeHours> Get(String id)
        {
            return Lib_Primavera.PriIntegration.GetOvertimeHours(id);
        }
        */
    }
}
