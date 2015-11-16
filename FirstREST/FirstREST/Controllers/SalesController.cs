using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace FirstREST.Controllers
{
    public class SalesController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
