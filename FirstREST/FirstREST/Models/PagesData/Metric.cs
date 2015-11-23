using System;
using System.Collections.Generic;

namespace Dashboard.Models.Primavera.Model
{
    public class Metric
    {
        public String name { get; set; }
        public String class_code { get; set; }
        public bool left_T_side { get; set; }
        public List<Year> years_data { get; set; }

        public Metric(String name, String class_code, bool left_T_side)
        {
            this.name = name;
            this.class_code = class_code;
            this.left_T_side = left_T_side;
            years_data = new List<Year>();
        }
    }
}