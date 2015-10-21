using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FirstREST.Controllers
{
    public class SalesController : ApiController
    {
        //GET api/Sales
        public IEnumerable<Lib_Primavera.Model.Sale> Get()
        {
            return Lib_Primavera.PriIntegration.GetSales();
        }
    }


}
