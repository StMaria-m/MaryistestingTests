using ApiTests.CovidApiTests.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace ApiTests.CovidApiTests
{
    [Category("Api tests")]
    [Author("Maria", "http://maryistesting.com")]
    public class CovidApiTests
    {
        private RestClient _restClient;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _restClient = new RestClient();
            _restClient.BaseUrl = new Uri("https://covid19-api.com/");
        }

        [Test]
        [Description("Check if api works properly")]
        [TestCase("Italy")]
        [TestCase("Norway")]
        [TestCase("Germany")]
        public void CorrectRequest_apiWorksProperlyTest(string countryName)
        {
            RestRequest restRequest = new RestRequest($"country?name={countryName}", Method.GET);

            IRestResponse response = _restClient.Execute(restRequest);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            IList<CountryReportModel> responseData = JsonConvert.DeserializeObject<IList<CountryReportModel>>(response.Content);

            Assert.IsNotNull(responseData?.Count);
            Assert.IsTrue(responseData.All(x => x.Country.Contains(countryName, StringComparison.CurrentCultureIgnoreCase)));
        }

        [Test]
        [Description("Check if api returns all countries with latest data")]
        public void CorrectRequest_apiReturnsAllCountryWithLatestDataTest()
        {
            RestRequest restRequest = new RestRequest("/country/all", Method.GET);

            IRestResponse response = _restClient.Execute(restRequest);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            IList<CountryReportModel> responseData = JsonConvert.DeserializeObject<IList<CountryReportModel>>(response.Content);

            Assert.IsNotNull(responseData?.Count);

            var countriesData = GetCountriesData();

            Assert.AreEqual(responseData.Count, countriesData.Count);
            
            foreach ( var countryReport in responseData)
            {
                if (!countriesData.Any(x => x.Name == countryReport.Country))
                {
                    Assert.Fail($"{countryReport.Country} do not return data");
                }
            }

            Assert.Pass();
        }

        [Test]
        [Description("Check if api returns country by country code")]
        [TestCase("It")]
        [TestCase("US")]
        [TestCase("gb")]
        public void CorrectRequest_apiReturnsCountryByCountryCodeTest(string countryCode)
        {
            RestRequest restRequest = new RestRequest($"/country/code?code={countryCode}", Method.GET);

            IRestResponse response = _restClient.Execute(restRequest);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            IList<CountryReportModel> responseData = JsonConvert.DeserializeObject<IList<CountryReportModel>>(response.Content);

            Assert.IsNotNull(responseData?.Count);
            Assert.IsTrue(responseData.All(x => x.Code.Contains(countryCode, StringComparison.CurrentCultureIgnoreCase)));
        }

        [Test]
        [Description("Check if api returns daily report by country code")]
        [TestCase("Pl", "2021-10-25")]
        public void CorrectRequest_apiReturnsDailyReportByCountryCodeTest(string countryCode, string date)
        {
            RestRequest restRequest = new RestRequest($"/report/country/code?code={countryCode}&date={date}", Method.GET);

            IRestResponse response = _restClient.Execute(restRequest);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            IList<DailyReportModel> responseData = JsonConvert.DeserializeObject<IList<DailyReportModel>>(response.Content);

            Assert.IsNotNull(responseData?.Count);
            Assert.IsTrue(responseData.All(x => x.Date.Contains(date, StringComparison.CurrentCultureIgnoreCase)));
        }

        [Test]
        [Description("Check if api response countries data is the same as json file")]
        public void CorrectRequest_checkIfCountriesDataAreValid()
        {
            RestRequest restRequest = new RestRequest("/help/countries", Method.GET);

            IRestResponse response = _restClient.Execute(restRequest);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            IList<CountryModel> responseData = JsonConvert.DeserializeObject<IList<CountryModel>>(response.Content);

            Assert.IsNotNull(responseData?.Count);

            var countriesData = GetCountriesData();

            Assert.AreEqual(responseData.Count, countriesData.Count);
            
            var countryNamesJson = countriesData.Select(x => x.Name).ToList();

            List<string> countryNamesAPI = new List<string>();
            foreach (var countryModel in responseData)
            {
                countryNamesAPI.Add(countryModel.Name);
            }

            CollectionAssert.AreEqual(countryNamesJson, countryNamesAPI);
        }
        public IList<CountryModel> GetCountriesData()
        {
            var countriesDataString = File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}//CovidApiTests\\Data//CountriesData.json");
            return JsonConvert.DeserializeObject<IList<CountryModel>>(countriesDataString);
        }
    }
}

