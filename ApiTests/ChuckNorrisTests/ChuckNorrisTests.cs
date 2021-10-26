using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using ApiTests.ChuckNorrisTests.Models;

namespace ApiTests.ChuckNorrisTests
{
    [Category("Api tests")]
    [Author("Maria", "http://maryistesting.com")]

    public class ChuckNorrisTests
    {
        private RestClient _restClient;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _restClient = new RestClient();
            _restClient.BaseUrl = new Uri("https://api.chucknorris.io/");
        }

        [Test]
        [Description("Check if api works properly")]
        public void CorrectRequest_apiWorksProperlyTest()
        {
            RestRequest restRequest = new RestRequest("jokes/random", Method.GET);

            IRestResponse response = _restClient.Execute(restRequest);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        [Description("Check if api returns categories")]
        public void CorrectRequest_apiReturnsCategoriesTest()
        {
            RestRequest restRequest = new RestRequest("jokes/categories", Method.GET);

            IRestResponse response = _restClient.Execute(restRequest);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var responseJokes = JsonConvert.DeserializeObject<List<string>>(response.Content);

            Assert.AreEqual(16, responseJokes.Count);
        }

        [Test]
        [Description("Check if api returns jokes contain demanded string")]
        public void CorrectRequest_return_jokesContainsDemandedStringTest()
        {

            RestRequest restRequest = new RestRequest("jokes/search?query=Twisted", Method.GET);

            IRestResponse response = _restClient.Execute(restRequest);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var responseJokes = JsonConvert.DeserializeObject<JokesResponse>(response.Content);

            Assert.IsTrue(responseJokes.Total > 0);
            Assert.AreEqual(responseJokes.Total, responseJokes.Result.Count);

            var singleJoke = responseJokes.Result.FirstOrDefault(x => x.Id == "y6KjkzAjSdGLwrHU9bzDqQ");
            Assert.IsNotNull(singleJoke);
        }

        [Test]
        [Description("Check if api returns joke")]
        public void CorrectRequest_return_jokeContainsIDTest()
        {
            string jokeId = "OuMP_rpkSGKomnwps_9myQ";

            RestRequest restRequest = new RestRequest($"jokes/{jokeId}", Method.GET);

            IRestResponse response = _restClient.Execute(restRequest);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var responseJoke = JsonConvert.DeserializeObject<SingleJokeResponse>(response.Content);

            Assert.IsTrue(responseJoke.Id == jokeId);
        }
    }
}
