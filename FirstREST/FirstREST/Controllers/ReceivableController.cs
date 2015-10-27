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
    public class ReceivableController : ApiController
    {
        // GET api/receivable
        public IEnumerable<Pending> Get(String initialDate, String finalDate)
        {
            return PriIntegration.GetPendingReceivables(DateTime.Parse(initialDate), DateTime.Parse(finalDate));
        }
    }
}
