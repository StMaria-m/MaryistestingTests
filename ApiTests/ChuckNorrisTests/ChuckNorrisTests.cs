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
        [TestCase("money")]
        [TestCase("1111111111111111")]
        [TestCase("smoke")]
        [TestCase("fish")]
        public void CorrectRequest_return_jokesContainsDemandedStringTest_testCase_version(string keyWord)
        {
            JokesContainsDemandedStringTest(keyWord);
        }

        [Test]
        [Description("Check if api returns jokes contain demanded string")]
        public void CorrectRequest_return_jokesContainsDemandedStringTest_loop_version()
        {
            List<string> keyWords = new List<string>()
            {
                "money",
                "tree",
                "smoke",
                "fish"
            };

            foreach (var keyWord in keyWords)
            {
                JokesContainsDemandedStringTest(keyWord);
            }
        }

        private void JokesContainsDemandedStringTest(string keyWord)
        {
            RestRequest restRequest = new RestRequest("jokes/search", Method.GET);
            restRequest.AddParameter("query", keyWord);

            IRestResponse response = _restClient.Execute(restRequest);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var responseJokes = JsonConvert.DeserializeObject<JokesResponse>(response.Content);

            Assert.Greater(responseJokes.Total, 0);
            Assert.AreEqual(responseJokes.Total, responseJokes.Result.Count);
            Assert.IsTrue(responseJokes.Result.All(x => x.Value.Contains(keyWord, StringComparison.CurrentCultureIgnoreCase)));
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

        [Test]
        [Description("Check if api returns jokes from different categories")]
        public void CorrectRequest_apiReturnsJokesFromDifferentCategoriesTest()
        {
            var categories = new List<string>
            {
                "animal",
                "career",
                "celebrity",
                "dev",
                "explicit",
                "fashion",
                "food",
                "history",
                "money",
                "movie",
                "music",
                "political",
                "religion",
                "science",
                "sport",
                "travel"
            };

            for (int index = 0; index < categories.Count; index++)
            {
                string category = categories[index];
                SendCategory(category);
            }

            int[] colectionOfNumber = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            foreach (int currentNumber in colectionOfNumber)
            {
                string category = categories[currentNumber];
                SendCategory(category);
            }

            foreach (string category in categories)
            {
                SendCategory(category);
            }
        }

        private void SendCategory(string category)
        {
            RestRequest restRequest = new RestRequest($"/jokes/random?category={category}", Method.GET);

            IRestResponse response = _restClient.Execute(restRequest);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            SingleJokeResponse responseData = JsonConvert.DeserializeObject<SingleJokeResponse>(response.Content);

            Assert.IsNotNull(responseData);
            Assert.IsTrue(responseData.Categories.Any(x => x == category));
        }
    }
}
