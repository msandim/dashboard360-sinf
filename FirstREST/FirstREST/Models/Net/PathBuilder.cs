using System;

namespace Dashboard.Models.Net
{
    public class PathBuilder : IPathBuilder
    {
        public Path BasePath { get; set; }
        public String Action { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime FinalDate { get; set; }
        public String DocumentType { get; set; }

        public PathBuilder()
        {
            InitialDate = DateTime.MinValue;
            FinalDate = DateTime.MinValue;
            DocumentType = null;
        }
        
        public Path Build()
        {
            Path path = BasePath != null ? new Path(BasePath.ToString()) : PathConstants.BasePath;

            if (Action != null)
                path.BasePath += "/" + Action;

            if (InitialDate == DateTime.MinValue && FinalDate == DateTime.MinValue && DocumentType == null)
                return path;

            if (InitialDate != DateTime.MinValue)
                AddParameter(path, "initialDate", InitialDate);

            if (FinalDate != DateTime.MinValue)
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

        public static Path Build(Path basePath, String action)
        {
            PathBuilder builder = new PathBuilder
            {
                BasePath = basePath,
                Action = action,
            };

            return builder.Build();
        }
        public static Path Build(Path basePath, String action, DateTime initialDate, DateTime finalDate, String documentType = null)
        {
            PathBuilder builder = new PathBuilder
            {
                BasePath = basePath,
                Action = action,
                InitialDate = initialDate,
                FinalDate = finalDate,
                DocumentType = documentType
            };

            return builder.Build();
        }
    }
}