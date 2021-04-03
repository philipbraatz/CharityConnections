﻿using CC.Abstract;
using CC.Connections.BL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace CC.Connections.WebUI
{
    public class apiHelper
    {
        private static HttpClient httpClient = InitializeClient();//TODO enable secure version
        //private static HttpClient InitializeClient(apiPassword apiPassword)
        //{
        //    using (HttpClient client = new HttpClient())
        //    {
        //        client.DefaultRequestHeaders.Add("cc-api-user", apiPassword.email);
        //        client.DefaultRequestHeaders.Add("cc-api-key", apiPassword.Key);
        //        //return new HttpClient { BaseAddress = new Uri("http://pebsurveymaker.azurewebsites.net/api/") };
        //        return new HttpClient { BaseAddress = new Uri("http://localhost:44363/api/") };
        //    }
        //}
        private static HttpClient InitializeClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("cc-api-user", "masterKey");//todo hash these
            client.DefaultRequestHeaders.Add("cc-api-key", "masterpassword");
            //client.Timeout = TimeSpan.FromMinutes(3);
            if (false)
                return new HttpClient { BaseAddress = new Uri("http://pebdvdcentraapi.azurewebsites.net/api/") };//Requires server database
            else
                return new HttpClient { BaseAddress = new Uri("http://localhost:44363/api/") };//SET LOCAL #
        }

        private static TEntityList getAll<TEntityList>(string model, bool all = true) where TEntityList : class
        {
            const string LIST_WORD = "collection";//remove collection from class names if present
            if (model.ToLower().Contains(LIST_WORD))
                model = model.Remove(model.ToLower().IndexOf(LIST_WORD), LIST_WORD.Length);

            HttpClient client = httpClient;
            String urlRequest = model + (all ? "/all" : String.Empty);
            String url = client.BaseAddress + urlRequest;//just for error info
            try
            {
                HttpResponseMessage message = client.GetAsync(urlRequest).Result;//GetAsync("LinkName")  HttpResponseMessage
                return JsonConvert.DeserializeObject<TEntityList>(//Json to Object of the
                                message
                                .Content.ReadAsStringAsync().Result);//to string result
            }
            catch (AggregateException e)
            {
                if (e.GetBaseException().GetType() == typeof( TaskCanceledException))
                {
                    TaskCanceledException ex = (TaskCanceledException)e.GetBaseException();
                    if (ex.CancellationToken.IsCancellationRequested)
                        throw;
                    else
                        throw new TimeoutException("Loading all for " + model + " took too long", ex);
                }
                else if(e.GetBaseException().GetType() == typeof(HttpRequestException))
                {
                    //if (e.GetBaseException().InnerException != null)
                        throw new Exception("\"" + url + "\" threw an exception: " + e.GetBaseException().Message + "\nIt does not want to share what happened");
                    //throw new Exception("\""+url+"\" threw an exception: " + e.GetBaseException().Message +"\nCheck if url exists");
                }
                else if (e.InnerException != null)
                    throw e.InnerException;
                else
                    throw;
            }



        }
        public static TEntityList getAll<TEntityList>() where TEntityList : class
            => getAll<TEntityList>(typeof(TEntityList).Name);

        public static TEntity getOne<TEntity>(Guid id) where TEntity : class
            => getAll<TEntity>(typeof(TEntity).Name + "/get/" + id, false);
        public static TEntity getEmail<TEntity>(String id) where TEntity : class
            => getAll<TEntity>(typeof(TEntity).Name + "/getEmail/" + id, false);

        public static HttpResponseMessage create<TEntity>(TEntity entity) where TEntity : class
            => httpClient.PutAsync(typeof(TEntity).Name + "/put", setContent(entity)).Result;
        public static HttpResponseMessage update<TEntity>(TEntity entity) where TEntity : class
            => httpClient.PutAsync(typeof(TEntity).Name + "/post", setContent(entity)).Result;

        public static HttpResponseMessage delete<TEntity, Type>(Type id) where TEntity : class where Type : class
            => httpClient.DeleteAsync(typeof(TEntity).Name + "/delete/" + id).Result;

        private static StringContent setContent<TEntity>(TEntity entity)
        {
            var content = new StringContent(        //to string
                JsonConvert.SerializeObject(entity)); //to json
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            return content;
        }

        public static TEntity getAction<TEntity>(string action, object value) where TEntity : class
        {
            HttpClient client = httpClient;
            return JsonConvert.DeserializeObject<TEntity>(//Json to Object of the
                client.GetAsync(nameof(TEntity) + "/" + (action + value != null ? value : String.Empty)).Result//GetAsync("LinkName")  HttpResponseMessage
                .Content.ReadAsStringAsync().Result//to string result
            );
        }
    }
}