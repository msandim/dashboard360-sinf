using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstREST.Lib_Primavera.Model
{
    public class Product
    {
        public String Brand { get; set; }
        public String Model { get; set; }
        public String Description { get; set; }
        public String FamilyId { get; set; }
        public String FamilyDescription { get; set; }
    }
}