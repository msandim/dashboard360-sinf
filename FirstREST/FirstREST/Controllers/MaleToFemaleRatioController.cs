using FirstREST.Lib_Primavera.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FirstREST.Controllers
{
    public class MaleToFemaleRatioController : ApiController
    {
        //GET api/FemaleToMaleRatio
        public GenderCounter Get(String initialDate, String finalDate)
        {
            return Lib_Primavera.PriIntegration.GetGenderCounting(DateTime.Parse(initialDate), DateTime.Parse(finalDate));
        }
    }
}
