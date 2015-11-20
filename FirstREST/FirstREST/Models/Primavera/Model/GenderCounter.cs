using System;

namespace Dashboard.Models.Primavera.Model
{
    public class GenderCounter
    {
        public GenderCounter(int male, int female)
        {
            Male = male;
            Female = female;
        }

        public int Male { get; set; }
        public int Female { get; set; }
    }
}
