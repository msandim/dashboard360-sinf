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

    public class PurchaseController : ApiController
    {
        //GET api/purchase
        public IEnumerable<Purchase> Get(DateTime initialDate, DateTime finalDate, String documentType) 
        {
            return PriIntegration.GetPurchases(initialDate, finalDate, documentType);
        }
    }
}