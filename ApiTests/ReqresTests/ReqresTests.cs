using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System;
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
        private string _unknownPath = "/api/unknown";

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

            UsersGridResponse responseData = JsonConvert.DeserializeObject<UsersGridResponse>(response.Content);

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

            UsersGridResponse responseData = JsonConvert.DeserializeObject<UsersGridResponse>(response.Content);

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
            RestRequest restRequest = new RestRequest(_unknownPath, Method.GET);

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

            RestRequest restRequest = new RestRequest($"{_unknownPath}?per_page={total}", Method.GET);

            IRestResponse response = _restClient.Execute(restRequest);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            UnknownsListResponse responseData = JsonConvert.DeserializeObject<UnknownsListResponse>(response.Content);

            Assert.IsNotNull(responseData?.Data);

            Assert.AreEqual(total, responseData.Data.Count());
        }


        [Test]
        [Description("Check if api returns unknown using id")]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(5)]
        [TestCase(12)]
        public void CorrectRequest_apiReturnsUnknownUsingIdTest(int unknownId)
        {

            RestRequest restRequest = new RestRequest($"{_unknownPath}/{unknownId}", Method.GET);

            IRestResponse response = _restClient.Execute(restRequest);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            SingleUnknownResponse responseData = JsonConvert.DeserializeObject<SingleUnknownResponse>(response.Content);

            Assert.IsNotNull(responseData?.Data);

            Assert.AreEqual(unknownId, responseData.Data.Id);
        }

        [Test]
        [Description("Add new user")]
        public void CorrectRequest_add_newUserTest()
        {
            NewUserRequest newUser = new NewUserRequest
            {
                Name = "Jan",
                Job = "tester"
            };

            var payload = JsonConvert.SerializeObject(newUser);

            RestRequest restRequest = new RestRequest(_userPath, Method.POST);
            restRequest.AddParameter("application/json", payload, ParameterType.RequestBody);

            IRestResponse response = _restClient.Execute(restRequest);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        [Test]
        [Description("Update user data")]
        public void CorrectRequest_update_userDataTest()
        {
            UpdateUserDataRequest updateUserData = new UpdateUserDataRequest
            {
                Name = "Janina",
                Job = "testerka"
            };

            var payload = JsonConvert.SerializeObject(updateUserData);

            RestRequest restRequest = new RestRequest($"{_userPath}/11", Method.PUT);
            restRequest.AddParameter("application/json", payload, ParameterType.RequestBody);

            IRestResponse response = _restClient.Execute(restRequest);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        [Description("Update user data")]
        public void CorrectRequest_updatePATCH_userDataTest()
        {
            NewUserRequest updateUserData = new NewUserRequest
            {
                Name = "Joanna"
            };

            var payload = JsonConvert.SerializeObject(updateUserData);

            RestRequest restRequest = new RestRequest($"{_userPath}/12", Method.PATCH);
            restRequest.AddParameter("application/json", payload, ParameterType.RequestBody);

            IRestResponse response = _restClient.Execute(restRequest);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        [Description("Registration new user")]
        public void CorrectRequest_register_newAccountTest()
        {
            RegistrationUserRequest newAccount = new RegistrationUserRequest
            {
                Email = "michael.lawson@reqres.in",
                Password = "111222"
            };

            var payload = JsonConvert.SerializeObject(newAccount);

            RestRequest restRequest = new RestRequest("/api/register", Method.POST);
            restRequest.AddParameter("application/json", payload, ParameterType.RequestBody);

            IRestResponse response = _restClient.Execute(restRequest);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
