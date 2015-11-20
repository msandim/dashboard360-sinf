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

    public class PayableController : ApiController
    {
        // GET api/payable
        public IEnumerable<Pending> Get(DateTime initialDate, DateTime finalDate)
        {
            return PriIntegration.GetPendingPayables(initialDate, finalDate);
        }
    }
}
