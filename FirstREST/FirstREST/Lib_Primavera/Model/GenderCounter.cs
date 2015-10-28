using System;
using System.Collections.Generic;
using System.Linq;

namespace FirstREST.Lib_Primavera.Model
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
