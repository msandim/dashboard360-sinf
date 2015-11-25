using System;

namespace Dashboard.Models.Net
{
    public class PathConstants
    {
        private const String _BasePath = "http://localhost:49822";
        public static Path BasePath => new Path(_BasePath);

        private const String _BasePathApiPrimavera = _BasePath + "/api/primavera";
        public static Path BasePathApiPrimavera => new Path(_BasePathApiPrimavera);
    }
}