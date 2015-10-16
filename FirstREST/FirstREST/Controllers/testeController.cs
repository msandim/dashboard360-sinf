using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Web.Http;


namespace FirstREST.Controllers
{
    public class testeController : ApiController
    {
        //
        // GET: /teste/

        public ActionResult Index()
        {
            return View();
        }

    }
}
