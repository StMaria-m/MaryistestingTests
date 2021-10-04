using JokeApiTests.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System;
using System.Linq;
using System.Net;

namespace JokeApiTests
{
    [Category("Api tests")]
    [Author("Maria", "http://maryistesting.com")]
    public partial class JokeApiTests
    {
        private RestClient _restClient;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _restClient = new RestClient();
            _restClient.BaseUrl = new Uri("https://v2.jokeapi.dev/");           
        }

        [Test]
        [Description("Check if api works well")]
        public void CorrectRequest_apiWorksWellTest()
        {
            RestRequest restRequest = new RestRequest("joke/Any", Method.GET);

            IRestResponse response = _restClient.Execute(restRequest);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        [Description("Check if api returns right id range and amount")]
        public void CorrectRequest_return_rightIdRangeAndAmountJokesTest()
        {
            RestRequest restRequest = new RestRequest("joke/Any?idRange=35-78&amount=3", Method.GET);

            IRestResponse response = _restClient.Execute(restRequest);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            JokesResponse responseJokes = JsonConvert.DeserializeObject<JokesResponse>(response.Content);

            Assert.AreEqual(3, responseJokes.Jokes.Count);

            Assert.IsTrue(responseJokes.Jokes.All(x => x.Id >= 35 && x.Id <= 78));
        }

        [Test]
        [Description("Check if api returns right type of joke")]
        public void CorrectRequest_return_rightTypeOfJokeTest()
        {
            var type = "twopart";

            RestRequest restRequest = new RestRequest($"joke/Programming?type={type}&idRange=0-10&amount=10", Method.GET);

            IRestResponse response = _restClient.Execute(restRequest);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var responseJokes = JsonConvert.DeserializeObject<JokesResponse>(response.Content);

            Assert.AreEqual(5, responseJokes.Jokes.Count);

            Assert.IsTrue(responseJokes.Jokes.All(x => x.Id >= 0 && x.Id <= 10));

            Assert.IsTrue(responseJokes.Jokes.All(x => x.Type == type));
        }

        [Test]
        [Description("Check if api returns right language")]
        public void CorrectRequest_return_rightLanguageJokeTest()
        {
            RestRequest restRequest = new RestRequest("joke/Any?lang=de&type=twopart&idRange=0-10", Method.GET);

            IRestResponse response = _restClient.Execute(restRequest);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            SingleJoke responseJoke = JsonConvert.DeserializeObject<SingleJoke>(response.Content);
                        
            StringAssert.AreEqualIgnoringCase("DE", responseJoke.Lang);

            Assert.IsTrue(responseJoke.Id >= 0 && responseJoke.Id <= 10);

            StringAssert.Contains("twopart", responseJoke.Type);
        }
    }
}
