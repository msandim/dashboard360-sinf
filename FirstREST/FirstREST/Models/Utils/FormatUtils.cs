using System;
using System.Globalization;

namespace Dashboard.Models.Utils
{
    public class FormatUtils
    {
        public static String FormatCurrency(Double currency)
        {
            return currency.ToString("N", CultureInfo.CurrentCulture.NumberFormat);
        }
    }
}