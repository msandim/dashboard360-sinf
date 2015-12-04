using System;

namespace Dashboard.Models.Primavera.Model
{
    public class GenderCounter
    {
        public GenderCounter(int male, int female, DateTime initialDate, DateTime finalDate)
        {
            Male = male;
            Female = female;
            InitialDate = initialDate;
            FinalDate = finalDate;
        }

        public int Male { get; set; }
        public int Female { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime FinalDate { get; set; }
    }
}
