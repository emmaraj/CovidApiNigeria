using CovidApiNigeria.DbContexts;
using CovidApiNigeria.Models;
using Hangfire;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CovidApiNigeria.Services {
    /// <summary>
    /// 
    /// </summary>
    public class DataService : IDataService {
        private const string pageURL = "http://covid19.ncdc.gov.ng/";

        private readonly DatabaseContext databaseContext;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseContext"></param>
        public DataService(DatabaseContext databaseContext) {
            this.databaseContext = databaseContext;
        }

        public IDataService IDataService {
            get => default;
            set {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<DataModel>> LatestDataAsync() {

            return await databaseContext.CovidNigeriaData
                            .Where(data => data.Date == databaseContext.CovidNigeriaData.Max(data => data.Date)) //Get the latest data
                            .ToListAsync();
            //return null;    
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stateName"></param>
        /// <returns></returns>
        public async Task<IEnumerable<DataModel>> GetStateData(string stateName) {
            var result = await databaseContext.CovidNigeriaData
                        .Where(state => ((state.StateName.ToLower()).Equals(stateName.ToLower())))
                        .OrderByDescending(data => data.Date)
                        .ToListAsync();
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public async Task<IEnumerable<DataModel>> GetDataByDate(DateTime dateTime) {
            var result = await databaseContext.CovidNigeriaData
                            .Where(data => data.Date.Equals(dateTime))
                            .ToListAsync();
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<IEnumerable<DataModel>> GetDataByDate(DateTime startDate, DateTime endDate) {
            var result = await databaseContext.CovidNigeriaData
                            .Where(data => (data.Date >= startDate) && (data.Date <= endDate))
                            .OrderByDescending(data => data.Date)
                            .ToListAsync();
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stateName"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<IEnumerable<DataModel>> GetStateData(string stateName, DateTime startDate, DateTime endDate) {
            var result = await databaseContext.CovidNigeriaData
                            .Where(data => (data.Date >= startDate) && (data.Date <= endDate) && (data.StateName.ToLower() == stateName.ToLower()))
                            .OrderByDescending(data => data.Date)
                            .ToListAsync();
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ScrapeData() {
            var htmlWeb = new HtmlWeb();
            var document = htmlWeb.Load(pageURL);

            //Extract the date 
            var dateNode = document.DocumentNode.SelectSingleNode("//div/div/div/div/div/div/div/div/ul/li");
            DateTime dataDate = ExtractDate(dateNode.InnerText.Trim());

            //the previous day's date
            DateTime yesterdaysDate = DateTime.Today.AddDays(-1);

            if (dataDate.Equals(yesterdaysDate)) {
                //select 
                var headers = document.DocumentNode.SelectNodes("div[div[@class='card-body']] | //h2[@class = 'text-right text-white']");

                List<int> data = new();

                foreach (HtmlNode header in headers) {
                    //remove commas and convert to int
                    data.Add(int.Parse(header.InnerText.ToString().Trim().Replace(",", "")));
                }

                //Extract the appropriate data and assign to the correct variable
                var SamplesTested = data[0];
                var TotalConfirmedCases = data[1];
                var TotalActiveCases = data[2];
                var TotalDischarged = data[3];
                var TotalDeaths = data[4];

                // Get the items in the table body
                var tableBody = document.DocumentNode
                                    .Descendants("tbody")
                                    .FirstOrDefault();

                //get all the rows
                List<HtmlNode> tableRows = tableBody.SelectNodes("tr").ToList();


                //loop through each row to fetch the needed data for every state
                foreach (var row in tableRows) {
                    List<HtmlNode> tableCells = row.SelectNodes("td").ToList();

                    var entry = new DataModel {
                        StateName = tableCells[0].InnerText.ToString().Trim(),
                        NumberOfConfirmedCases = int.Parse(tableCells[1].InnerText.ToString().Trim().Replace(",", "")),
                        NumberOfActiveCases = int.Parse(tableCells[2].InnerText.ToString().Trim().Replace(",", "")),
                        NumberOfDischarged = int.Parse(tableCells[3].InnerText.ToString().Trim().Replace(",", "")),
                        NumberOfDeaths = int.Parse(tableCells[4].InnerText.ToString().Trim().Replace(",", "")),
                        SamplesTested = SamplesTested,
                        TotalConfirmedCases = TotalConfirmedCases,
                        TotalActiveCases = TotalActiveCases,
                        TotalDischarged = TotalDischarged,
                        TotalDeaths = TotalDeaths,
                        Date = dataDate
                    };
                    databaseContext.CovidNigeriaData.Add(entry);

                }
                databaseContext.SaveChanges();
            }
            else {
                BackgroundJob.Schedule(
                    () => ScrapeData(),
                    TimeSpan.FromMinutes(120)       //if data fetched is not the most recent, it will reschedule to run again in two hours time
                    );
            }


        }

        //extract Date from string gotten from the web
        private static DateTime ExtractDate(string text) {
            var splittedText = text.Split(",");

            var extradctedText = splittedText[0];
            var textLength = extradctedText.Length;
            //extract year
            var year = int.Parse(extradctedText.Substring(textLength - 4, 4));

            //extract month
            IDictionary<int, string> months = new Dictionary<int, string> {
                { 1, "Jan" },
                { 2, "Feb" },
                { 3, "Mar" },
                { 4, "Apr" },
                { 5, "May" },
                { 6, "Jun" },
                { 7, "Jul" },
                { 8, "Aug" },
                { 9, "Sep" },
                { 10, "Oct" },
                { 11, "Nov" },
                { 12, "Dec" }
            };

            int selectedMonth = 0;
            foreach (var month in months) {
                if (extradctedText.Contains(month.Value)) {
                    selectedMonth = month.Key;
                }
            }

            //extract day
            var day = int.Parse(Regex.Match(extradctedText, @"\d+").Value);

            DateTime dateTime = new(year, selectedMonth, day);
            return dateTime;
        }
    }
}
