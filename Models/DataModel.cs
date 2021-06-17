using System;

namespace CovidApiNigeria.Models {

    /// <summary>
    /// Data Model Class
    /// </summary>
    public class DataModel  {

        /// <summary>
        /// Name of state
        /// </summary>
        public string StateName { get; set; }

        /// <summary>
        /// Number of Confirmed Cases in a State
        /// </summary>
        public int NumberOfConfirmedCases { get; set; }

        /// <summary>
        /// Number of Active Cases in a State
        /// </summary>
        public int NumberOfActiveCases { get; set; }

        /// <summary>
        /// Number of Deaths in a State
        /// </summary>
        public int NumberOfDeaths { get; set; }

        /// <summary>
        /// Number of Discharged Cases in a State
        /// </summary>
        public int NumberOfDischarged { get; set; }

        /// <summary>
        /// Number of Samples Tested at the National Level
        /// </summary>
        public int SamplesTested { get; set; }

        /// <summary>
        /// Total Number of Confirmed Cases at the National Level
        /// </summary>
        public int TotalConfirmedCases { get; set; }

        /// <summary>
        /// Total Number of Active Cases at the National Level
        /// </summary>
        public int TotalActiveCases { get; set; }

        /// <summary>
        /// Total Number of Discharged Cases at the National Level
        /// </summary>
        public int TotalDischarged { get; set; }

        /// <summary>
        /// Total Number of Deaths at the National Level
        /// </summary>
        public int TotalDeaths { get; set; }

        /// <summary>
        /// Reporting Date
        /// </summary>
        public DateTime Date { get; set; }
    }
}
