using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FirstREST.Lib_Primavera;

namespace FirstREST.Controllers
{
    public class SaleController : ApiController
    {
        //GET api/Sales
        public IEnumerable<Lib_Primavera.Model.Sale> Get(String initialDate, String finalDate, String documentType)
        {
            return PriIntegration.GetSales(DateTime.Parse(initialDate), DateTime.Parse(finalDate), documentType);
        }
    }
}
