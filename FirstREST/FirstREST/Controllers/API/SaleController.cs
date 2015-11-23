using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Dashboard.Controllers.API
{
    public class SaleController : ApiController
    {
        [ActionName("top_products")]
        public IEnumerable<string> GetTopProducts(DateTime initialDate, DateTime finalDate)
        {

        }
    }
}