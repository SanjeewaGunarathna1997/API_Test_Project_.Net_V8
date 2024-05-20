using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using System.Net;

namespace API_Test_Project
{
    [TestClass]
    public class MainScript
    {
        private RestClient _client;
        private string _baseUrl = "https://api.restful-api.dev/";
        private String productId;

        [SetUp]
        public void Setup()
        {
            _client = new RestClient(_baseUrl);
        }
        //Add a object
        [Test, Order(1)]
        public void Test_AddObject()
        {

            string createJsonBody = "{\"name\": \"Test Product 001\", \"data\": { \"Generation\": \"35th\", \"Price\": \"678.99\", \"Capacity\": \"512 GB\" }}";
            var request = new RestRequest("objects", Method.Post);
            request.AddJsonBody(createJsonBody);
            var response = _client.Execute(request);
            JObject responseObject = JObject.Parse(response.Content);
            productId = (string)responseObject["id"];
            NUnit.Framework.TestContext.WriteLine("product id is = " + productId);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
        //Get and verify the added object
        [Test, Order(2)]
        public void Test_GetSingleObjectForCreate()
        {

            var request = new RestRequest("objects/" + productId + "", Method.Get);
            var response = _client.Execute(request);
            NUnit.Framework.TestContext.WriteLine("url is " + "objects/" + productId + "");
            JObject responseObject = JObject.Parse(response.Content);

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual("Test Product 001", (string)responseObject["name"]);
        }
        //Get all objects available
        [Test, Order(3)]
        public void Test_GetAllObjects()
        {
            var request = new RestRequest("objects", Method.Get);
            var response = _client.Execute(request);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
        //Ubdate the priviously created object
        [Test, Order(4)]
        public void Test_UpdateObject()
        {
            string updateJsonBody = "{\"name\": \"Test Product 002\", \"data\": { \"Generation\": \"40th\", \"Price\": \"777.99\", \"Capacity\": \"1080 GB\" }}";

            var request = new RestRequest("objects/" + productId + "", Method.Put);
            request.AddJsonBody(updateJsonBody);
            var response = _client.Execute(request);

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        }
        //Get and verify the updated object is succssfully updated or not
        [Test, Order(5)]
        public void Test_GetSingleObjectForUpdate()
        {

            var request = new RestRequest("objects/" + productId + "", Method.Get);
            var response = _client.Execute(request);
            NUnit.Framework.TestContext.WriteLine("url is " + "objects/" + productId + "");
            JObject responseObject = JObject.Parse(response.Content);

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual("Test Product 002", (string)responseObject["name"]);
        }

        //Delete the object
        [Test, Order(6)]
        public void Test_DeleteObject()
        {
            var request = new RestRequest("objects/" + productId + "", Method.Delete);
            var response = _client.Execute(request);

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
        //Verify deleted object using GET check status equal to NOT FOUND
        [Test, Order(7)]
        public void Test_GetSingleObjectForDelete()
        {

            var request = new RestRequest("objects/" + productId + "", Method.Get);
            var response = _client.Execute(request);
            NUnit.Framework.TestContext.WriteLine("url is " + "objects/" + productId + "");
            JObject responseObject = JObject.Parse(response.Content);

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}