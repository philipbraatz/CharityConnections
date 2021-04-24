using Doorfail.Connections.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace Doorfail.Connections.WebUI.Model
{
    public class TempApiTest
    {
        static HttpClient client = new HttpClient();
        public static async Task RunAsync()
        {
            client.DefaultRequestHeaders.Add("cc-api-user", "masterKey");//todo hash these
            client.DefaultRequestHeaders.Add("cc-api-key", "masterpassword");

            // Update port # in the following line.
            client.BaseAddress = new Uri("https://tiblar.com/api/v2/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            
        }

        public static async Task<String> GetProductAsync()
        {
            String product = null;
            HttpResponseMessage response = await client.GetAsync("/ping");
            if (response.IsSuccessStatusCode)
            {
                product = await response.Content.ReadAsStringAsync();
            }
            return product;
        }
    }
}