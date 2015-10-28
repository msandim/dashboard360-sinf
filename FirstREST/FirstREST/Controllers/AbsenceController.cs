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

    public class AbsenceController : ApiController
    {
        //GET api/Absence
        public IEnumerable<Absence> Get(DateTime initialDate, DateTime finalDate)
        {
            return PriIntegration.GetAbsences(initialDate, finalDate);
        }

        //GET api/Absence/001
        public IEnumerable<Absence> Get(String id)
        {
            return PriIntegration.GetAbsences(id);
        }
    }
}