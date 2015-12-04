using System;
using System.Collections.Generic;
using Dashboard.Models.Net;

namespace Dashboard.Models.Caching
{
    public class Cache<T>
    {
        private Boolean _firstRun = true;
        private Path BasePath { get; set; }
        private String Action { get; set; }
        private DateTime InitialDate { get; set; }
        private DateTime FinalDate { get; set; }
        public LinkedList<T> CachedData { get; set; }

        public Cache(Path basePath, String action)
        {
            BasePath = basePath;
            Action = action;
            InitialDate = DateTime.MaxValue;
            FinalDate = DateTime.MinValue;
            CachedData = new LinkedList<T>();
        }

        public void UpdateData(DateTime initialDate, DateTime finalDate)
        {
            lock (CachedData)
            {
                if (_firstRun)
                {
                    _firstRun = false;
                    Initialize(initialDate, finalDate);
                }
                else
                {
                    UpdateNewData(initialDate, finalDate);
                }
            } 
        }

        private void Initialize(DateTime initialDate, DateTime finalDate)
        {
            InitialDate = initialDate;
            FinalDate = finalDate;
            MakeRequest(BasePath, Action, initialDate, finalDate);
        }
        private void UpdateNewData(DateTime initialDate, DateTime finalDate)
        {
            // ]FinalDate, finalDate], FinalDate := finalDate:
            if (initialDate >= InitialDate && finalDate > FinalDate)
                UpdateFinalDateData(finalDate);

            // [initialDate, InitialDate[, InitialDate := initialDate:
            else if (initialDate < InitialDate && finalDate <= FinalDate)
                UpdateInitialDateData(initialDate);

            // [initialDate, InitialDate[, ]FinalDate, finalDate], InitialDate := initialDate, FinalDate := finalDate:
            else if (initialDate < InitialDate && finalDate > FinalDate)
            {
                UpdateInitialDateData(initialDate);
                UpdateFinalDateData(finalDate);
            }
        }
        private void UpdateInitialDateData(DateTime initialDate)
        {
            // Make a request from [initialDate, InitialDate[:
            MakeRequest(BasePath, Action, initialDate, InitialDate.AddDays(-1));

            // Updating InitialDate:
            InitialDate = initialDate;
        }
        private void UpdateFinalDateData(DateTime finalDate)
        {
            // Make a request from ]FinalDate, finalDate]:
            MakeRequest(BasePath, Action, FinalDate.AddDays(1), finalDate);

            // Updating FinalDate:
            FinalDate = finalDate;
        }

        private void MakeRequest(Path basePath, String action, DateTime initialDate, DateTime finalDate)
        {
            // Build path and make request:
            var path = PathBuilder.Build(basePath, action, initialDate, finalDate);
            var enumerable = NetHelper.MakeRequest<T>(path);

            // Join new data to the cached data:
            foreach (var item in enumerable)
                CachedData.AddLast(item);
        }
    }
}