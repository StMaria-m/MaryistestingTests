using ApiTests.LoveCalculatorApiTests.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System;
using System.Net;

namespace ApiTests.LoveCalculatorApiTests
{
    [Category("Api tests")]
    [Author("Maria", "http://maryistesting.com")]
    public class LoveCalculatorApiTests
    {
        private RestClient _restClient;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _restClient = new RestClient();
            _restClient.BaseUrl = new Uri("https://love-calculator.p.rapidapi.com/");
        }

        [Test]
        [Description("Check if api works well")]
        public void CorrectRequest_apiWorksWellTest()
        {
            RestRequest restRequest = new RestRequest("/getPercentage?sname=Alice&fname=John", Method.GET);

            restRequest.AddHeader("x-rapidapi-host", "love-calculator.p.rapidapi.com");
            restRequest.AddHeader("x-rapidapi-key", "my_key");

            IRestResponse response = _restClient.Execute(restRequest);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        [Description("Check if api returns error without sname parameter")]
        public void IncorrectRequest_withoutSnameParameterTest()
        {
            RestRequest restRequest = new RestRequest("/getPercentage?fname=john", Method.GET);

            restRequest.AddHeader("x-rapidapi-host", "love-calculator.p.rapidapi.com");
            restRequest.AddHeader("x-rapidapi-key", "my_key");

            IRestResponse response = _restClient.Execute(restRequest);

            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Test]
        [Description("Check if api returns error without fname parameter")]
        public void IncorrectRequest_withoutFnameParameterTest()
        {
            RestRequest restRequest = new RestRequest("/getPercentage?sname=alice", Method.GET);

            restRequest.AddHeader("x-rapidapi-host", "love-calculator.p.rapidapi.com");
            restRequest.AddHeader("x-rapidapi-key", "my_key");

            IRestResponse response = _restClient.Execute(restRequest);

            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Test]
        [Description("Check if api returns error without any parameters")]
        public void IncorrectRequest_withoutAnyParametersTest()
        {
            RestRequest restRequest = new RestRequest("/getPercentage", Method.GET);

            restRequest.AddHeader("x-rapidapi-host", "love-calculator.p.rapidapi.com");
            restRequest.AddHeader("x-rapidapi-key", "my_key");

            IRestResponse response = _restClient.Execute(restRequest);

            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Test]
        [Description("Check if api returns error without Headers parameter 'x-rapidapi-key'")]
        public void IncorrectRequest_withoutHeadersParameterKeyAuthorizationTest()
        {
            RestRequest restRequest = new RestRequest("/getPercentage?sname=Alice&fname=John", Method.GET);

            restRequest.AddHeader("x-rapidapi-host", "love-calculator.p.rapidapi.com");

            IRestResponse response = _restClient.Execute(restRequest);

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Test]
        [Description("Check if api returns error with incorrect value Headers parameter 'x-rapidapi-key'")]
        public void IncorrectRequest_withIncorrectHeadersParameter_rapidapi_hostTest()
        {
            RestRequest restRequest = new RestRequest("/getPercentage?sname=Alice&fname=John", Method.GET);

            restRequest.AddHeader("x-rapidapi-host", "love-calculator.p.rapidapi.com");
            restRequest.AddHeader("x-rapidapi-key", "test");

            IRestResponse response = _restClient.Execute(restRequest);

            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Test]
        [Description("Check if api returns various results")]
        [TestCase("John", "Aneta", "56")]
        [TestCase("John", "Aneto", "46")]
        [TestCase("John", "Anetk", "36")]
        public void CorrectRequest_apiReturnsVariousResultsTests_testCase_version(string sname, string fname, string result)
        {
            SendAndCheckResult(sname, fname, result);
        }

        [Test]
        [Description("Check if api returns various results")]
        public void CorrectRequest_apiReturnsVariousResultsTest_loop_version()
        {
            string[] snameExamples = new string[3]
            {
                "John",
                "Karol",
                "Fryderyk"
            };

            string[] fnameExamples = new string[3]
            {
                "Aneta",
                "Beata",
                "Jadwiga"
            };

            string[] results = new string[3]
            {
                "56",
                "65",
                "53"
            };

            for (int index = 0; index <= 2; index++)
            {
                string sname = snameExamples[index];
                string fname = fnameExamples[index];
                string result = results[index];

                SendAndCheckResult(sname, fname, result);
            }

            int[] colectionOfNumber = new int[] { 0, 1, 2 };
            foreach (int curentNumber in colectionOfNumber)
            {
                string sname = snameExamples[curentNumber];
                string fname = fnameExamples[curentNumber];
                string result = results[curentNumber];

                SendAndCheckResult(sname, fname, result);
            }

            int index2 = 0;
            foreach (string sname in snameExamples)
            {
                string fname = fnameExamples[index2];
                string result = results[index2];

                SendAndCheckResult(sname, fname, result);

                index2++;
            }
        }       

        private void SendAndCheckResult(string sname, string fname, string result)
        {
            RestRequest restRequest = new RestRequest($"/getPercentage?sname={sname}&fname={fname}", Method.GET);

            restRequest.AddHeader("x-rapidapi-host", "love-calculator.p.rapidapi.com");
            restRequest.AddHeader("x-rapidapi-key", "my_key");

            IRestResponse response = _restClient.Execute(restRequest);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            SingleLoveResponse responseData = JsonConvert.DeserializeObject<SingleLoveResponse>(response.Content);

            Assert.AreEqual(result, responseData.Percentage);
        }
    }
}
