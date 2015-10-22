using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace FirstREST.Controllers
{
    public class ItemsController : ApiController
    {
        //GET api/Items
        public IEnumerable<Lib_Primavera.Model.Sale> Get()
        {
            return Lib_Primavera.PriIntegration.GetItems();
        }
    }
}