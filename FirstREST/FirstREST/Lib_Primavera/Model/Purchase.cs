using System;
using System.Web;

namespace FirstREST.Lib_Primavera.Model
{
    public class Purchase
    {
        public String ID { get; set; }
        public DateTime DocumentDate { get; set; }
        public String DocumentType { get; set; }
        public String Product { get; set; }
        public Money Value { get; set; }
    }
}
