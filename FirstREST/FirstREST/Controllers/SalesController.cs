using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Dashboard.Controllers
{
    using Models;

    public class SalesController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
