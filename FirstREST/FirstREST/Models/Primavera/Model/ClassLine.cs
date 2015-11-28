using System;
using System.Collections.Generic;

namespace Dashboard.Models.Primavera.Model
{
    public class ClassLine
    {
        public List<String> db_names;
        public List<String> cr_names;

        public int ano { get; set; }
        public String conta { get; set; }
        public String moeda { get; set; }
        public List<double> values;

            /*
        public Money mes00CR { get; set; }
        public Money mes01CR { get; set; }
        public Money mes02CR { get; set; }
        public Money mes03CR { get; set; }
        public Money mes04CR { get; set; }
        public Money mes05CR { get; set; }
        public Money mes06CR { get; set; }
        public Money mes07CR { get; set; }
        public Money mes08CR { get; set; }
        public Money mes09CR { get; set; }
        public Money mes10CR { get; set; }
        public Money mes11CR { get; set; }
        public Money mes12CR { get; set; }
        public Money mes13CR { get; set; }
        public Money mes14CR { get; set; }
        public Money mes15CR { get; set; }

        public Money mes00DB { get; set; }
        public Money mes01DB { get; set; }
        public Money mes02DB { get; set; }
        public Money mes03DB { get; set; }
        public Money mes04DB { get; set; }
        public Money mes05DB { get; set; }
        public Money mes06DB { get; set; }
        public Money mes07DB { get; set; }
        public Money mes08DB { get; set; }
        public Money mes09DB { get; set; }
        public Money mes10DB { get; set; }
        public Money mes11DB { get; set; }
        public Money mes12DB { get; set; }
        public Money mes13DB { get; set; }
        public Money mes14DB { get; set; }
        public Money mes15DB { get; set; }

        public Money mes01OR { get; set; }
        public Money mes02OR { get; set; }
        public Money mes03OR { get; set; }
        public Money mes04OR { get; set; }
        public Money mes05OR { get; set; }
        public Money mes06OR { get; set; }
        public Money mes07OR { get; set; }
        public Money mes08OR { get; set; }
        public Money mes09OR { get; set; }
        public Money mes10OR { get; set; }
        public Money mes11OR { get; set; }
        public Money mes12OR { get; set; }*/

        public String tipoLancamento { get; set; }
        public String naturezaOR { get; set; }

        public ClassLine()
        {
            values = new List<double>();
            db_names = new List<string>(new String[] 
            {
                "Mes00DB",
                "Mes01DB",
                "Mes02DB",
                "Mes03DB",
                "Mes04DB",
                "Mes05DB",
                "Mes06DB",
                "Mes07DB",
                "Mes08DB",
                "Mes09DB",
                "Mes10DB",
                "Mes11DB",
                "Mes12DB"
            });

            cr_names = new List<string>(new String[] 
            {
                "Mes00CR",
                "Mes01CR",
                "Mes02CR",
                "Mes03CR",
                "Mes04CR",
                "Mes05CR",
                "Mes06CR",
                "Mes07CR",
                "Mes08CR",
                "Mes09CR",
                "Mes10CR",
                "Mes11CR",
                "Mes12CR"
            });

        }
    }
}