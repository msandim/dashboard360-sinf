using System;
using System.Text;

namespace Dashboard.Models.Net
{
    public class PathBuilder : IPathBuilder
    {
        public DateTime InitialDate { get; set; }
        public DateTime FinalDate { get; set; }
        public String DocumentType { get; set; }
        private Boolean firstParameter;
        
        public Path Build()
        {
            Path path = new Path(PathConstants.BASE_PATH_API_PRIMAVERA);

            if (InitialDate == null && FinalDate == null && DocumentType == null)
                return path;

            if (InitialDate != null)
                AddParameter(path, "initialDate", InitialDate);

            if (FinalDate != null)
                AddParameter(path, "finalDate", FinalDate);

            if (DocumentType != null)
                AddParameter(path, "documentType", DocumentType);

            return path;
        }

        private void AddParameter(Path path, String key, String value)
        {
            path.AddParameter(key, value);
        }
        private void AddParameter(Path path, String key, DateTime date)
        {
            AddParameter(path, key, FormatDate(date));
        }

        private String FormatDate(DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }
    }
}