using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FirstREST.Lib_Primavera.Model;

namespace FirstREST.Controllers
{
    public class SupplierController : ApiController
    {
        // GET api/Supplier
        public IEnumerable<Supplier> Get()
        {
            return Lib_Primavera.PriIntegration.GetSuppliers();
        }
    }
}