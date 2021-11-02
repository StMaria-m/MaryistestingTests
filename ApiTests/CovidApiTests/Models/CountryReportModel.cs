namespace ApiTests.CovidApiTests.Models
{
    public class CountryReportModel
    {
        public string Country { get; set; }
        public string Code { get; set; }
        public int Confirmed { get; set; }
        public int Recovered { get; set; }
        public int Critical { get; set; }
        public int Deaths { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string LastChange { get; set; }
        public string LastUpdate { get; set; }
    }
}
