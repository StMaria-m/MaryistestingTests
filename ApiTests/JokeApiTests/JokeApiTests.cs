using ApiTests.JokeApiTests.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
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
            int startIdRange = 35;
            int endIdRange = 78;
            int amount = 3;

            RestRequest restRequest = new RestRequest("joke/Any", Method.GET);

            List<Parameter> parameters = new List<Parameter> 
            {
                new Parameter ("idRange", $"{startIdRange}-{endIdRange}", ParameterType.QueryString),
                new Parameter ("amount", amount, ParameterType.QueryString),
            };
            restRequest.AddOrUpdateParameters(parameters);

            IRestResponse response = _restClient.Execute(restRequest);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            JokesResponse responseJokes = JsonConvert.DeserializeObject<JokesResponse>(response.Content);

            Assert.AreEqual(amount, responseJokes.Jokes.Count);

            Assert.IsTrue(responseJokes.Jokes.All(x => x.Id >= startIdRange && x.Id <= endIdRange));
        }

        [Test]
        [Description("Check if api returns right type of joke")]
        public void CorrectRequest_return_rightTypeOfJokeTest()
        {
            var jokeType = "twopart";
            int startIdRange = 0;
            int endIdRange = 10;
            int amount = 10;

            RestRequest restRequest = new RestRequest($"joke/Programming", Method.GET);
            
            List<Parameter> parameters = new List<Parameter>
            {
                new Parameter ("idRange", $"{startIdRange}-{endIdRange}", ParameterType.QueryString),
                new Parameter ("type", jokeType, ParameterType.QueryString),
                new Parameter ("amount", amount, ParameterType.QueryString),
            };
            restRequest.AddOrUpdateParameters(parameters);

            IRestResponse response = _restClient.Execute(restRequest);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var responseJokes = JsonConvert.DeserializeObject<JokesResponse>(response.Content);

            Assert.AreEqual(5, responseJokes.Jokes.Count);

            Assert.IsTrue(responseJokes.Jokes.All(x => x.Id >= startIdRange && x.Id <= endIdRange));

            Assert.IsTrue(responseJokes.Jokes.All(x => x.Type == jokeType));
        }

        [Test]
        [Description("Check if api returns right language")]
        public void CorrectRequest_return_rightLanguageJokeTest()
        {

            string language = "de";
            string jokeType = "twopart";
            int startIdRange = 0;
            int endIdRange = 10;

            RestRequest restRequest = new RestRequest("joke/Any", Method.GET);
           
            List<Parameter> parameters = new List<Parameter>
            {
                new Parameter ("lang", language, ParameterType.QueryString),
                new Parameter ("type", jokeType, ParameterType.QueryString),
                new Parameter ("idRange", $"{startIdRange}-{endIdRange}", ParameterType.QueryString),
            };
            restRequest.AddOrUpdateParameters(parameters);

            IRestResponse response = _restClient.Execute(restRequest);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            SingleJoke responseJoke = JsonConvert.DeserializeObject<SingleJoke>(response.Content);

            StringAssert.AreEqualIgnoringCase("DE", responseJoke.Lang);

            Assert.IsTrue(responseJoke.Id >= startIdRange && responseJoke.Id <= endIdRange);

            StringAssert.Contains(jokeType, responseJoke.Type);
        }

        [Test]
        [Description("Check if api eliminates jokes signed as blacklist: nsfw, religious, racist, sexist, explicit")]
        public void CorrectRequest_return_jokeWithoutBlacklistTest()
        {
            RestRequest restRequest = new RestRequest("joke/Any", Method.GET);
            restRequest.AddParameter("blacklistFlags", "nsfw,religious,racist,sexist,explicit");

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
            string jokeType = "single";
            string searchingKey = "man";

            RestRequest restRequest = new RestRequest("joke/Any", Method.GET);

            List<Parameter> parameters = new List<Parameter>
            {
                new Parameter ("type", jokeType, ParameterType.QueryString),
                new Parameter ("contains", searchingKey, ParameterType.QueryString),
            };
            restRequest.AddOrUpdateParameters(parameters);

            IRestResponse response = _restClient.Execute(restRequest);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            SingleJokeResponse responseJoke = JsonConvert.DeserializeObject<SingleJokeResponse>(response.Content);

            StringAssert.Contains(searchingKey, responseJoke.Joke);

            StringAssert.Contains(jokeType, responseJoke.Type);
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

            for (int i = 1; i <= 121; i++)
            {
                IRestResponse response = _restClient.Execute(restRequest);
                if (i == 120)
                {
                    //sprawdź czy zapytanie nr 120 zakończy się powodzeniem
                    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                }
                else if (i == 121)
                {
                    //sprawdź czy wyświetlono kod błędu 429 Too many requests
                    Assert.AreEqual(HttpStatusCode.TooManyRequests, response.StatusCode);
                }
            }
        }
    }
}
