using System;
using System.Collections.Generic;

namespace Dashboard.Models.Primavera.Model
{
    public class Year
    {
        public int year;
        public Double total { get; set; }
        public List<Double> months;

        public Year()
        {
            year = 0;
            total = 0;
            months = new List<Double>();
        }

        public void addMonth(Double value){
            total += value;
            months.Add(value);
        }

        public static Year operator +(Year c1, Year c2)
        {
            Year res = new Year();
            res.year = c1.year;

            for (int i = 0; i < c1.months.Count && i < c2.months.Count; i++)
                res.addMonth(c1.months[i] + c2.months[i]);

            return res;
        }
    }
}