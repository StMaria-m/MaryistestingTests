using ApiTests.CovidApiTests.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
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

            RestRequest restRequestAllCountries = new RestRequest("/help/countries", Method.GET);

            IRestResponse responseAllCountries = _restClient.Execute(restRequestAllCountries);

            Assert.AreEqual(HttpStatusCode.OK, responseAllCountries.StatusCode);

            IList<CountryModel> responseData2 = JsonConvert.DeserializeObject<IList<CountryModel>>(responseAllCountries.Content);

            Assert.IsNotNull(responseData?.Count);

            Assert.AreEqual(responseData.Count, responseData2.Count);
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
    }
}

