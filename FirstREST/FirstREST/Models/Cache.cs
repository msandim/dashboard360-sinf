using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dashboard.Models.Net;
using Dashboard.Models.Primavera.Model;

namespace Dashboard.Models
{
    public class Cache<T> where T : IDocument
    {
        private Boolean _firstRun = true;
        private Path BasePath { get; }
        private String Action { get; }
        private DateTime InitialDate { get; set; }
        private DateTime FinalDate { get; set; }
        private LinkedList<T> CachedData { get; }

        public Cache(Path basePath, String action)
        {
            BasePath = basePath;
            Action = action;
            InitialDate = DateTime.MaxValue;
            FinalDate = DateTime.MinValue;
            CachedData = new LinkedList<T>();
        }

        public IEnumerable<T> FetchData(DateTime initialDate, DateTime finalDate)
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
                        return Query(initialDate, finalDate);

                    // If we need to update the initial date:
                    if (updateInitialDate)
                    {
                        // Then we only have to make a request from the initialDate to InitialDate:
                        MakeRequest(BasePath, Action, initialDate, InitialDate);
                        InitialDate = initialDate;
                    }
                    // If we need to update the final date:
                    if (updateFinalDate)
                    {
                        // Then we only have to make a request from the FinalDate to finalDate:
                        MakeRequest(BasePath, Action, FinalDate, finalDate);
                        FinalDate = finalDate;
                    }
                }

                return Query(initialDate, finalDate);
            }
        }

        public IEnumerable<T> Query(DateTime initialDate, DateTime finalDate)
        {
            return from data in CachedData
                   where initialDate <= data.DocumentDate && data.DocumentDate <= finalDate
                   select data;
        }

        private void MakeRequest(Path basePath, String action, DateTime initialDate, DateTime finalDate)
        {
            // Build path and make request:
            var path = PathBuilder.Build(basePath, action, initialDate, finalDate);
            var task = Task.Run(() => NetHelper.MakeRequest<T>(path));
            var enumerable = task.Result;

            // Join new data to the cached data:
            foreach (var item in enumerable)
                CachedData.AddLast(item);
        }
    }
}