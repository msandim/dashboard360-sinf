using System;
using System.Collections.Generic;
using Dashboard.Models.Net;

namespace Dashboard.Models
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
                    InitialDate = initialDate;
                    FinalDate = finalDate;
                    MakeRequest(BasePath, Action, initialDate, finalDate);
                }
                else
                {
                    Boolean updateInitialDate = initialDate < InitialDate;
                    Boolean updateFinalDate = finalDate > FinalDate;

                    // If there is no need for update, then return the cached data:
                    if (!updateInitialDate && !updateFinalDate)
                        return;

                    // If we need to update the initial date:
                    if (updateInitialDate)
                    {
                        // Then we only have to make a request from [initialDate, InitialDate[:
                        MakeRequest(BasePath, Action, initialDate, InitialDate.AddDays(-1));
                        InitialDate = initialDate;
                    }
                    // If we need to update the final date:
                    if (updateFinalDate)
                    {
                        // Then we only have to make a request from ]FinalDate, finalDate]:
                        MakeRequest(BasePath, Action, FinalDate.AddDays(1), finalDate);
                        FinalDate = finalDate;
                    }
                }
            }
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