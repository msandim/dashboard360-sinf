using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Dashboard.Controllers.API
{
    using Models;

    public class SalesController : ApiController
    {
        public IEnumerable<Double> GetSales(DateTime initialDate, DateTime finalDate)
        {
            return SalesManager.GetNetSales(initialDate, finalDate);
        }
    }
}
