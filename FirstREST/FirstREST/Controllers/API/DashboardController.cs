using System;
using System.Web.Http;
using Dashboard.Models;

namespace Dashboard.Controllers.API
{
    public class DashboardController : ApiController
    {
        [ActionName("button_values")]
        public DashboardManager.ButtonValues GetButtonValues(DateTime initialDate, DateTime finalDate)
        {
            return DashboardManager.GetButtonValues(initialDate, finalDate);
        }
    }
}