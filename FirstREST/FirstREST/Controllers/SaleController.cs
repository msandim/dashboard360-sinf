using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FirstREST.Lib_Primavera;

namespace FirstREST.Controllers
{
    using Lib_Primavera.Model;

    public class SaleController : ApiController
    {
        //GET api/Sales
        public IEnumerable<Sale> Get(DateTime initialDate, DateTime finalDate, String documentType)
        {
            return PriIntegration.GetSales(initialDate, finalDate, documentType);
        }
    }
}
