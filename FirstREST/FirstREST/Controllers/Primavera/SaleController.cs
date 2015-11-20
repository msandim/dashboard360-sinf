using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Dashboard.Controllers.Primavera
{
    using Models.Primavera;
    using Models.Primavera.Model;

    public class SaleController : ApiController
    {
        //GET api/sale
        public IEnumerable<Sale> Get(DateTime initialDate, DateTime finalDate, String documentType)
        {
            return PriIntegration.GetSales(initialDate, finalDate, documentType);
        }
    }
}
