using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http;

namespace Doorfail.Connections.API.Test
{

    public struct InternalServerError
    {
        public string Message;
        public string ExceptionMessage;
        public string ExceptionType;//Type but errors sometimes and only used as a string anyways
        public string StackTrace;
    }

    [TestClass]
    public class APIControllerUT
    {

        private static HttpClient httpClient = InitializeClient();
        private static Uri apiURL = new Uri("https://localhost:44368/api/");
        private static HttpClient InitializeClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("cc-api-user", "masterKey");//todo hash these
            client.DefaultRequestHeaders.Add("cc-api-key", "masterpassword");
            client.BaseAddress = apiURL;
            //client.Timeout = TimeSpan.FromMinutes(3);
            return client;
        }


        [TestMethod]
        public void GetAll()
        {
        }
        [TestMethod]
        public void GetOne()
        {
        }
        [TestMethod]
        public void GetEmail()
        {
        }
        [TestMethod]
        public void Post()
        {
        }
        [TestMethod]
        public void Put()
        {
        }
        [TestMethod]
        public void Delete()
        {
        }

    }
}
