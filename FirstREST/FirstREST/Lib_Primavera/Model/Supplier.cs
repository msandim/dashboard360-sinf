using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstREST.Lib_Primavera.Model
{
    public class Supplier
    {
        public String ID { get; set; }
        public String Name { get; set; }
        public Double PendingOrdersValue { get; set; }
    }
}