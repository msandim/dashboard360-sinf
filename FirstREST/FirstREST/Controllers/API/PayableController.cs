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
    public class PayableController : ApiController
    {
        // GET api/payable
        public IEnumerable<Pending> Get(DateTime initialDate, DateTime finalDate)
        {
            return PriIntegration.GetPendingPayables(initialDate, finalDate);
        }
    }
}
