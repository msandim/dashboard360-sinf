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
    }
}