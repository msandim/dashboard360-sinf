using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboard.Models.Primavera.Model
{
    public class EmployeeMovement
    {
        public String EmployeeId { get; set; }
        public DateTime MovementDate { get; set; }
        public Double EmployeePayment { get; set; }
        public Double EstateCharges { get; set; }
        public Int32 ProcessNo { get; set; }
    }
}