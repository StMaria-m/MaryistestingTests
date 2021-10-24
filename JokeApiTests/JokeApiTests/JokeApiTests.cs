using ApiTests.JokeApiTests.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System;
using System.Linq;
using System.Net;

namespace ApiTests.JokeApiTests
{
    [Category("Api tests")]
    [Author("Maria", "http://maryistesting.com")]
    public class JokeApiTests
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

        [Test]
        [Description("Check if api eliminates jokes signed as blacklist: nsfw, religious, racist, sexist, explicit")]
        public void CorrectRequest_return_jokeWithoutBlacklistTest()
        {
            RestRequest restRequest = new RestRequest("joke/Any?blacklistFlags=nsfw,religious,racist,sexist,explicit", Method.GET);

            IRestResponse response = _restClient.Execute(restRequest);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            SingleJoke responseJoke = JsonConvert.DeserializeObject<SingleJoke>(response.Content);

            Assert.IsTrue(responseJoke.Flags.Nsfw == false
                && responseJoke.Flags.Religious == false
                && responseJoke.Flags.Racist == false
                && responseJoke.Flags.Sexist == false
                && responseJoke.Flags.Explicit == false);
        }

        [Test]
        [Description("Check if api returns jokes contains demanded string")]
        public void CorrectRequest_return_jokeContainsDemandedStringTest()
        {

            RestRequest restRequest = new RestRequest("joke/Any?type=single&contains=man", Method.GET);

                IRestResponse response = _restClient.Execute(restRequest);

                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

                SingleJoke responseJoke = JsonConvert.DeserializeObject<SingleJoke>(response.Content);

                StringAssert.Contains("man", responseJoke.Joke);

                StringAssert.Contains("single", responseJoke.Type);
        }

        [Test]
        [Description("Add new joke")]
        public void CorrectRequest_add_newJokeTest()
        {
            NewJokeRequest newJoke = new NewJokeRequest
            {
                Category = "Dark",
                FormatVersion = 3,
                Joke = "ioijioio",
                Lang = "en",
                Type = "single",
                Flags = new Flags
                {
                    Explicit = false,
                    Nsfw = false,
                    Political = true,
                    Racist = false,
                    Religious = false,
                    Sexist = false
                }
            };

            var payload = JsonConvert.SerializeObject(newJoke);

            RestRequest restRequest = new RestRequest("submit", Method.PUT);
            restRequest.AddParameter("application/json", payload, ParameterType.RequestBody);

            IRestResponse response = _restClient.Execute(restRequest);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        [Test]
        [Description("Check if api returns status code 429")]
        public void ToManyRequest_Test()
        {
            RestRequest restRequest = new RestRequest("joke/Any", Method.GET);

            for (int i = 1; i <= 101; i++)
            {
                IRestResponse response = _restClient.Execute(restRequest);
                if (i == 100)
                {
                    //sprawdź czy zapytanie nr 100 zakończy się powodzeniem
                    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                } 
                else if (i == 101)
                {
                    //sprawdź czy wyświetlono kod błędu 429 Too many requests
                    Assert.AreEqual(HttpStatusCode.TooManyRequests, response.StatusCode);
                }
            }
        }
    }
}
