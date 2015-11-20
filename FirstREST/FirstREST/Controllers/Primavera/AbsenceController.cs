using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Dashboard.Controllers.Primavera
{
    using Models.Primavera;
    using Models.Primavera.Model;

    public class AbsenceController : ApiController
    {
        //GET api/absence
        public IEnumerable<Absence> Get(DateTime initialDate, DateTime finalDate)
        {
            return PriIntegration.GetAbsences(initialDate, finalDate);
        }
    }
}