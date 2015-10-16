using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstREST.Lib_Primavera.Model
{
    public class Money
    {
        public Double Value { get; set; }
        public String Currency { get; set; }

        public Money()
        {
            Value = 0.0;
            Currency = "";
        }
        public Money(Double value, String currency)
        {
            Value = value;
            Currency = currency;
        }
    }
}