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
        public float Get()
        {
            return Lib_Primavera.PriIntegration.GetMaleToFemaleRatio();
        }
    }
}
