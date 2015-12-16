using System.Web.Mvc;

namespace Dashboard.Controllers
{
    public class DashboardController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }

}