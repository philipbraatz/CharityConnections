using CC.Abstract;
using CC.Connections.BL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace CC.Connections.WebUI
{
    public class apiHelper
    {
        private static HttpClient httpClient = InitializeClient();
        //private static HttpClient InitializeClient(apiPassword apiPassword)
        //{
        //    using (HttpClient client = new HttpClient())
        //    {
        //        client.DefaultRequestHeaders.Add("cc-api-user", apiPassword.email);
        //        client.DefaultRequestHeaders.Add("cc-api-key", apiPassword.Key);
        //        //return new HttpClient { BaseAddress = new Uri("http://pebsurveymaker.azurewebsites.net/api/") };
        //        return new HttpClient { BaseAddress = new Uri("http://localhost:59445/api/") };
        //    }
        //}
        private static HttpClient InitializeClient()
        {
            HttpClient client = httpClient;
            client.DefaultRequestHeaders.Add("cc-api-user", "masterKey");//todo hash these
            client.DefaultRequestHeaders.Add("cc-api-key", "masterpassword");
            if (false)
                return new HttpClient { BaseAddress = new Uri("http://pebdvdcentraapi.azurewebsites.net/api/") };
            else
                return new HttpClient { BaseAddress = new Uri("http://localhost:62096/api/") };//SET LOCAL #
        }

        private static TEntityList getAll<TEntityList>(string model) where TEntityList : class
        {
            const string LIST_WORD = "collection";//remove collection from class names if present
            if (model.ToLower().Contains(LIST_WORD))
                model = model.Remove(model.ToLower().IndexOf(LIST_WORD), LIST_WORD.Length);

            HttpClient client = httpClient;
            return JsonConvert.DeserializeObject<TEntityList>(//Json to Object of the
                client.GetAsync(model).Result//GetAsync("LinkName")  HttpResponseMessage
                .Content.ReadAsStringAsync().Result//to string result
            );

        }
        public static TEntityList getAll<TEntityList>() where TEntityList : class
            => getAll<TEntityList>(nameof(TEntityList));

        public static TEntity  getOne<TEntity,TType>(TType id) where TEntity : class where TType :class
            => getAll<TEntity>(nameof(TEntity) +"/"+id);

        public static HttpResponseMessage create<TEntity>( TEntity entity) where TEntity : class
            => httpClient.PostAsync(nameof(TEntity), setContent(entity)).Result;
        public static HttpResponseMessage update<TEntity>(TEntity entity) where TEntity : class
            => httpClient.PutAsync(nameof(TEntity), setContent(entity)).Result;

        public static HttpResponseMessage delete<TEntity, Type>(Type id) where TEntity : class where Type : class
            => httpClient.DeleteAsync(nameof(TEntity) + "/"+id).Result;

        private static StringContent setContent<TEntity>(TEntity entity)
        {
            var content = new StringContent(        //to string
                JsonConvert.SerializeObject(entity)); //to json
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            return content;
        }
    }
}