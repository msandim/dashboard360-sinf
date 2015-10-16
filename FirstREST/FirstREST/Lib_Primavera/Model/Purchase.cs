using System;
using System.Web;

namespace FirstREST.Lib_Primavera.Model
{
    public class Purchase
    {
        public String ID { get; set; }
        public DateTime ProductReceivedOn { get; set; }
        public DateTime PayedOn { get; set; }
        public Money Value { get; set; }
    }
}
