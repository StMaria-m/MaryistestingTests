using ApiTests.MyStoreApiTests.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System;
using System.Net;
using System.Linq;
using System.IO;
using ApiTests.MyStoreApiTests.Data;

namespace ApiTests.MyStoreApiTests
{
    [Category("Api tests")]
    [Author("Maria", "http://maryistesting.com")]
    public class MyStoreTests
    {
        private RestClient _restClient;
        private AppsettingsModel _appSettings;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _restClient = new RestClient();
            _restClient.BaseUrl = new Uri("https://my-store2.p.rapidapi.com/");

            var appSettingsString = File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}//appsettings.json");
            _appSettings = JsonConvert.DeserializeObject<AppsettingsModel>(appSettingsString);
        }

        [Test]
        [Description("Check if api works well")]
        public void CorrectRequest_apiWorksWellTest()
        {
            RestRequest restRequest = new RestRequest("catalog/products", Method.GET);
            restRequest.AddHeader("x-rapidapi-key", _appSettings.RapidapiKey);

            IRestResponse response = _restClient.Execute(restRequest);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        [Description("Check if api returns product by product Id")]
        public void CorrectRequest_apiReturnsProductTest()
        {
            int id = 19485;
            RestRequest restRequest = new RestRequest($"catalog/product/{id}", Method.GET);
            restRequest.AddHeader("x-rapidapi-key", _appSettings.RapidapiKey);

            IRestResponse response = _restClient.Execute(restRequest);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            SingleProductFull responseProduct = JsonConvert.DeserializeObject<SingleProductFull>(response.Content);

            Assert.IsNotNull(responseProduct);

            Assert.AreEqual(responseProduct.Id, id);
        }

        [Test]
        [Description("Check if api returns products list ")]
        public void CorrectRequest_apiReturnsProductsListTest()
        {

            RestRequest restRequest = new RestRequest("catalog/products", Method.GET);

            restRequest.AddHeader("x-rapidapi-key", _appSettings.RapidapiKey);

            IRestResponse response = _restClient.Execute(restRequest);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            ProductsGridResponse responseProductsList = JsonConvert.DeserializeObject<ProductsGridResponse>(response.Content);

            Assert.IsNotNull(responseProductsList);
            Assert.GreaterOrEqual(responseProductsList.Summary.Count, 0);
            Assert.AreEqual(10, responseProductsList.Products.Count);
        }

        [Test]
        [Description("Check if api returns categories list ")]
        public void CorrectRequest_apiReturnsCatgoriesListTest()
        {
            RestRequest restRequest = new RestRequest("catalog/categories", Method.GET);

            restRequest.AddHeader("x-rapidapi-key", "");

            IRestResponse response = _restClient.Execute(restRequest);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            CategoriesGridResponse responseCategoriesList = JsonConvert.DeserializeObject<CategoriesGridResponse>(response.Content);

            Assert.IsTrue(responseCategoriesList.Categories.Any());
        }

        [Test]
        [Description("Check if api returns products in category")]
        [TestCase("furniture")]
        public void CorrectRequest_apiReturnsProductsInCategoryTest(string category)
        {
            RestRequest restRequest = new RestRequest($"catalog/category/{category}/products", Method.GET);

            restRequest.AddHeader("x-rapidapi-key", "");
            restRequest.AddParameter("skip", 0);
            restRequest.AddParameter("limit", 10);

            IRestResponse response = _restClient.Execute(restRequest);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            ProductsGridResponse responseCategoriesList = JsonConvert.DeserializeObject<ProductsGridResponse>(response.Content);

            Assert.IsNotNull(responseCategoriesList);

            Assert.IsTrue(responseCategoriesList.Products.All(x => x.Category == category));
            Assert.AreEqual(7, responseCategoriesList.Summary.Count);
        }

        [Test]
        [Description("Delete existing product")]
        public void CorrectRequest_deleteExistingProductTest()
        {
            NewProductRequest newProduct = new NewProductRequest
            {
                Name = "table",
                Price = 10,
                Manufacturer = "abc",
                Category = "furniture",
                Description = "new product description",
                Tags = "furniture"
            };

            var productId = AddNewProduct(newProduct).Id;

            RestRequest deleteRequest = new RestRequest($"/catalog/product/{productId}", Method.DELETE);
            deleteRequest.AddHeader("x-rapidapi-key", _appSettings.RapidapiKey);

            IRestResponse deleteResponse = _restClient.Execute(deleteRequest);

            Assert.AreEqual(HttpStatusCode.OK, deleteResponse.StatusCode);

            DeleteProductResponse deleteProduct = JsonConvert.DeserializeObject<DeleteProductResponse>(deleteResponse.Content);
            Assert.AreEqual("deleted", deleteProduct.Status);
        }        

        [Test]
        [Description("Add new product")]
        public void CorrectRequest_addNewProductTest()
        {
            NewProductRequest payload = new NewProductRequest()
            {
                Name = "aaa",
                Price = 10,
                Manufacturer = "abc",
                Category = "  meble",
                Description = "ccc",
                Tags = "111"
            };

            var newProduct = AddNewProduct(payload);
            Assert.Greater(newProduct.Id, 0);
        }

        private SingleProductFull AddNewProduct(NewProductRequest newProduct)
        {
            var payload = JsonConvert.SerializeObject(newProduct);

            RestRequest restRequest = new RestRequest("/catalog/product", Method.POST);
            restRequest.AddHeader("x-rapidapi-key", _appSettings.RapidapiKey);
            restRequest.AddParameter("application/json", payload, ParameterType.RequestBody);

            IRestResponse response = _restClient.Execute(restRequest);
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            return JsonConvert.DeserializeObject<SingleProductFull>(response.Content);
        }        
    }
}
