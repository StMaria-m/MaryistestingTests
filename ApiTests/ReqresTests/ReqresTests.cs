using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using ApiTests.ReqresTests.Models;

namespace ApiTests.ReqresTests
{
    [Category("Api tests")]
    [Author("Maria", "http://maryistesting.com")]
    public class ReqresTests
    {
        private string _userPath = "/api/users";
        private string _unknonwPath = "/api/unknown";

        private RestClient _restClient;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _restClient = new RestClient();
            _restClient.BaseUrl = new Uri("https://reqres.in");
        }

        [Test]
        [Description("Check if api works properly")]
        public void CorrectRequest_apiWorksProperlyTest()
        {
            RestRequest restRequest = new RestRequest(_userPath, Method.GET);

            IRestResponse response = _restClient.Execute(restRequest);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            UsersListResponse responseData = JsonConvert.DeserializeObject<UsersListResponse>(response.Content);

            Assert.IsNotNull(responseData?.Data);

            Assert.IsTrue(responseData.Data.Any());
        }

        [Test]
        [Description("Check if api returns all users list")]
        public void CorrectRequest_apiReturnsAllUsersListTest()
        {
            int total = 12;

            RestRequest restRequest = new RestRequest($"{_userPath}?per_page={total}", Method.GET);

            IRestResponse response = _restClient.Execute(restRequest);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            UsersListResponse responseData = JsonConvert.DeserializeObject<UsersListResponse>(response.Content);

            Assert.IsNotNull(responseData?.Data);

            Assert.AreEqual(total, responseData.Data.Count());
        }

        [Test]
        [Description("Check if api returns user using id")]
        public void CorrectRequest_apiReturnsUserUsingIdTest()
        {
            int userId = 2;

            RestRequest restRequest = new RestRequest($"{_userPath}/{userId}", Method.GET);

            IRestResponse response = _restClient.Execute(restRequest);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            SingleUserResponse responseData = JsonConvert.DeserializeObject<SingleUserResponse>(response.Content);

            Assert.IsNotNull(responseData?.Data);

            Assert.AreEqual(userId, responseData.Data.Id);
        }

        [Test]
        [Description("Check if api returns unknowns list")]
        public void CorrectRequest_apiReturnsUnknownsListTest()
        {
            RestRequest restRequest = new RestRequest(_unknonwPath, Method.GET);

            IRestResponse response = _restClient.Execute(restRequest);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            UnknownsListResponse responseData = JsonConvert.DeserializeObject<UnknownsListResponse>(response.Content);

            Assert.IsNotNull(responseData?.Data);

            Assert.IsTrue(responseData.Data.Any());
        }

        [Test]
        [Description("Check if api returns all unknowns list")]
        public void CorrectRequest_apiReturnsAllUnknownsListTest()
        {
            int total = 12;

            RestRequest restRequest = new RestRequest($"{_unknonwPath}?per_page={total}", Method.GET);

            IRestResponse response = _restClient.Execute(restRequest);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            UnknownsListResponse responseData = JsonConvert.DeserializeObject<UnknownsListResponse>(response.Content);

            Assert.IsNotNull(responseData?.Data);

            Assert.AreEqual(total, responseData.Data.Count());
        }


        [Test]
        [Description("Check if api returns unknown using id")]
        public void CorrectRequest_apiReturnsUnknownUsingIdTest()
        {
            int unknownId = 1;

            RestRequest restRequest = new RestRequest($"{_unknonwPath}/{unknownId}", Method.GET);

            IRestResponse response = _restClient.Execute(restRequest);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            SingleUnknownResponse responseData = JsonConvert.DeserializeObject<SingleUnknownResponse>(response.Content);

            Assert.IsNotNull(responseData?.Data);

            Assert.AreEqual(unknownId, responseData.Data.Id);
        }

    }
}
