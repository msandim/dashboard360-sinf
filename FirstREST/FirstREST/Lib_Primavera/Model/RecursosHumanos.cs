using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FirstREST.Lib_Primavera.Model
{
    public class RecursosHumanos
    {
        public String Hired { get; set; }
        public String Fired { get; set; }
        public String LaborCost { get; set; }
        public String Absences { get; set; }
        public String Overtime { get; set; }
        public String MaleToFemaleRatio { get; set; }
    }
}
