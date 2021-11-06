using System;
using System.Collections.Generic;
using System.Text;

namespace ApiTests.CovidApiTests.Models
{
    public class DailyReportModel
    {
        public string Country { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Date { get; set; }
    }
}
