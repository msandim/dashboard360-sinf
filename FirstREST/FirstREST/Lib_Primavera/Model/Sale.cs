using System;
using System.Web;

namespace FirstREST.Lib_Primavera.Model
{
    public class Sale
    {
        public String ID { get; set; }
        public String Item { get; set; }
        public String Category { get; set; }
        public double Comission { get; set; }
        public DateTime PayedOn { get; set; }
        public DateTime ProductDeliveredOn { get; set; }
        public String EmployeeId { get; set; }
        public String Costumer { get; set; }
        public Money Value { get; set; }
    }
}
