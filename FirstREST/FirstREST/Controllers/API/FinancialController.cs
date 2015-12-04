using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Dashboard.Controllers.API
{
    using Models;
    using Models.Utils;
    using Models.PagesData;

    public class FinancialController : ApiController
    {
        [ActionName("/")]
        public BalanceSheet GetBalanceSheet()
        {
            return MetricsManager.GetBalanceSheet();
        }
    }
}
