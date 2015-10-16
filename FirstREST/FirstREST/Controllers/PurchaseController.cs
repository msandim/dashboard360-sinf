using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FirstREST.Controllers
{
    public class PurchaseController : ApiController
    {
        public IEnumerable<Lib_Primavera.Model.Purchase> Get()
        {
            return Lib_Primavera.PriIntegration.GetPurchases();
        }
    }
}
