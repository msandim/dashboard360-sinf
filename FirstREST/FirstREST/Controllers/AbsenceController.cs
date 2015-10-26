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
        //GET api/Absence
        public IEnumerable<Lib_Primavera.Model.Absence> Get(String initialDate, String finalDate)
        {
            return Lib_Primavera.PriIntegration.GetAbsences(DateTime.Parse(initialDate), DateTime.Parse(finalDate));
        }

        //GET api/Absence/001
        public IEnumerable<Lib_Primavera.Model.Absence> Get(String id)
        {
            return Lib_Primavera.PriIntegration.GetAbsences(id);
        }
    }
}