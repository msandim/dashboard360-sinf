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
        //GET api/overtimeHours
        public IEnumerable<OvertimeHours> Get(DateTime initialDate, DateTime finalDate)
        {
            return PriIntegration.GetOvertimeHours(initialDate, finalDate);
        }
    }
}
