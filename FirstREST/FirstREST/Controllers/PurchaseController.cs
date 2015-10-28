using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FirstREST.Controllers
{
    using Lib_Primavera.Model;

    public class PurchaseController : ApiController
    {
        public IEnumerable<Purchase> Get(DateTime initialDate, DateTime finalDate, String documentType) 
        {
            return Lib_Primavera.PriIntegration.GetPurchases(initialDate, finalDate, documentType);
        }
    }
}