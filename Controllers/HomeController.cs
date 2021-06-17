using CovidApiNigeria.Models;
using CovidApiNigeria.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidApiNigeria.Controllers {
    /// <summary>
    /// HomeController Class
    /// 
    /// </summary>
    [Route("api/data")]
    [ApiController]
    public class HomeController : Controller {
        private readonly IDataService _dataService;

        /// <summary>
        /// HomeController()
        /// </summary>
        /// <param name="dataService"></param>
        /// <returns>
        /// IEnumerable DataModel
        /// </returns>
        public HomeController(IDataService dataService) {
            _dataService = dataService;
        }

        public IDataService IDataService {
            get => default;
            set {
            }
        }

        /// <summary>
        /// Get latest from all states
        /// </summary>
        /// <returns>
        /// IEnumerable DataModel
        /// </returns>
        [HttpGet]
        public async Task<IEnumerable<DataModel>> GetLatestData() {
            var result = await _dataService.LatestDataAsync();
            return result;
        }

        /// <summary>
        /// Get all data for a particular state
        /// </summary>
        /// <param name="state">State</param>
        /// <returns>
        /// IEnumerable DataModel
        /// </returns>
        [HttpGet("s/{state}", Name = "GetStateData")]
        public async Task<IEnumerable<DataModel>> GetStateData(string state) {
            var result = await _dataService.GetStateData(state);
            return result;
        }

        /// <summary>
        /// Get the data for a state from a period
        /// </summary>
        /// <param name="state">State Name</param>
        /// <param name="start">Start Date</param>
        /// <param name="end">End Date</param>
        /// <returns>
        /// IEnumerable DataModel
        /// </returns>
        [HttpGet("s/", Name = "GetStateDatas")]
        public async Task<IEnumerable<DataModel>> GetStateDatas(string state, string start, string end) {
            var startDateTime = GetDateTime(start);
            var endDateTime = GetDateTime(end);

            var result = await _dataService.GetStateData(state, startDateTime, endDateTime);
            return result;
        }

        /// <summary>
        /// Get Data for a particular date
        /// </summary>
        /// <param name="date">Date of Incident</param>
        /// <returns>
        /// IEnumerable DataModel
        /// </returns>
        [HttpGet("d/{date}", Name = "GetDataByDate")]
        public async Task<IEnumerable<DataModel>> GetDataByDate(string date) {
            var dateTime = GetDateTime(date);
            var results = await _dataService.GetDataByDate(dateTime);
            return results;
        }

        /// <summary>
        /// Get National data across a date range
        /// </summary>
        /// <param name="start">Start Date</param>
        /// <param name="end">End </param>
        /// <returns>
        /// IEnumerable DataModel
        /// </returns>
        [HttpGet("d/", Name = "GetDataByDates")]
        public async Task<IEnumerable<DataModel>> GetDataByDates(string start, string end) {
            var startDateTime = GetDateTime(start);
            var endDateTime = GetDateTime(end);

            var results = await _dataService.GetDataByDate(startDateTime, endDateTime);
            return results;
        }

        private DateTime GetDateTime(string date) {
            var dateComponent = date.Split("-"); //split the date into day, month and year 

            int day = int.Parse(dateComponent[0]);
            int month = int.Parse(dateComponent[1]);
            int year = int.Parse(dateComponent[2]);

            return new DateTime(year, month, day);
        }


    }
}
