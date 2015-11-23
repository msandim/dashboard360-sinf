using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboard.Models.Net
{
    public class PathConstants
    {
        private const String BASE_PATH = "http://localhost:49822";
        public static Path BasePath
        {
            get
            {
                return new Path(BASE_PATH);
            }
        }

        private const String BASE_PATH_API_PRIMAVERA = BASE_PATH + "/api/primavera";
        public static Path BasePathAPIPrimavera
        {
            get
            {
                return new Path(BASE_PATH_API_PRIMAVERA);
            }
        }
    }
}