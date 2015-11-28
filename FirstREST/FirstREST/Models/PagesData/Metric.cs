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

        public static Metric operator +(Metric c1, Metric c2)
        {
            Metric res = new Metric(c1.name, c1.class_code, c1.left_T_side);

            if (c1.years_data.Count == 0)
            {
                res.years_data = c2.years_data;
            }
            else
            {
                for (int i = 0; i < c1.years_data.Count && i < c2.years_data.Count; i++)
                {
                    if (c1.years_data[i].year == c2.years_data[i].year)
                        res.years_data.Add(c1.years_data[i] + c2.years_data[i]);
                }
            }
            return res;
        }
    }
}