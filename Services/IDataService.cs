using CovidApiNigeria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidApiNigeria.Services {
    public interface IDataService {
        public void ScrapeData();
        public Task<IEnumerable<DataModel>> LatestDataAsync();
        public Task<IEnumerable<DataModel>> GetStateData(string stateName);
        public Task<IEnumerable<DataModel>> GetDataByDate(DateTime date);
        public Task<IEnumerable<DataModel>> GetDataByDate(DateTime startDate, DateTime endDate);
        public Task<IEnumerable<DataModel>> GetStateData(string stateName, DateTime startDate, DateTime endDate);
    }
}
