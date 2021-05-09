using Doorfail.DataConnection;
using Doorfail.Connections.BL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Diagnostics;
using System.Web.Mvc;
using Doorfail.Connections.WebUI.Model;

namespace Doorfail.Connections.WebUI
{
    public struct InternalServerError
    {
        public string Message;
        public string ExceptionMessage;
        public string ExceptionType;//Type but errors sometimes and only used as a string anyways
        public string StackTrace;
    }

    public static class apiHelper
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

        private static Uri apiURL = new Uri("https://doorfail.live/api/");//SET SERVER
        [Conditional("DEBUG")]
        private static void setDebugURL()=> apiURL = new Uri("https://localhost:44368/api/");//SET LOCAL #
        private static HttpClient InitializeClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("cc-api-user", "masterKey");//todo hash these
            client.DefaultRequestHeaders.Add("cc-api-key", "masterpassword");
            setDebugURL();//Will not be hit on running on server
            client.BaseAddress = apiURL;
            //client.Timeout = TimeSpan.FromMinutes(3);
            return client;
        }

        private static TEntity[] getAll<TEntity>(string model, bool all = true) where TEntity : class
        {
            const string LIST_WORD = "collection";//remove collection from class names if present
            if (model.ToLower().Contains(LIST_WORD))
                model = model.Remove(model.ToLower().IndexOf(LIST_WORD), LIST_WORD.Length);

            HttpClient client = httpClient;
            String urlRequest = model + (all ? "/all" : String.Empty);
            String url = client.BaseAddress + urlRequest;//just for error info
            HttpResponseMessage message=null;
            try
            {
                message = client.GetAsync(urlRequest).Result;//GetAsync("LinkName")  HttpResponseMessage
                dynamic res = message
                        .Content.ReadAsStringAsync().Result;//Json as string
                //Json converted to object
                if (message.StatusCode == HttpStatusCode.NotFound)
                    throw new HttpUnhandledException(res);
                if (all)
                    return JsonConvert.DeserializeObject<TEntity[]>(res);// Array
                else
                    return new TEntity[]{ JsonConvert.DeserializeObject<TEntity>(res)};// Single

            }
            catch (JsonSerializationException e)
            {
                InternalServerError err = JsonConvert.DeserializeObject<InternalServerError>(//Json to Object of the
                                    message
                                    .Content.ReadAsStringAsync().Result);//to string result
                string mes = String.Empty;

                if (err.Message != null)
                    throw new HttpException((int)message.StatusCode,
                        err.ExceptionType + ": " + err.Message + "\r\n\r\n" + err.ExceptionMessage + "\r\n\r\nStack Trace: " + err.StackTrace);//"API had an Exception, check exception details");
                else
                    throw new Exception("Could not parse: "+message
                                    .Content.ReadAsStringAsync().Result, e);
                
                throw new HttpException((int)message.StatusCode,mes);//"API had an Exception, check exception details");
                throw new Exception(message.StatusCode + ": \"" + JsonConvert.DeserializeObject(message.Content.ReadAsStringAsync().Result) + "\" was not expected");
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
                    if (e.GetBaseException().InnerException != null)
                        throw e.GetBaseException().InnerException;
                    else
                        throw new Exception("\"" + url + "\" threw an exception: " + e.GetBaseException().Message + "\nIt does not want to share what happened");
                    //throw new Exception("\""+url+"\" threw an exception: " + e.GetBaseException().Message +"\nCheck if url exists");
                }
                else if (e.InnerException != null)
                    throw e.InnerException;
                else
                    throw;
            }



        }

        //TODO CHECK PASSWORD
        public static TEntity fromPassword<TEntity>(Password password) where TEntity : class
            => getEmail<TEntity>(ToSafeUrl(password.email));

        public static TEntity[] getAll<TEntity>() where TEntity : class
            => getAll<TEntity>(       typeof(TEntity).Name);

        public static TEntity getOne<TEntity>(Guid id)                    where TEntity : class
            => getAll<TEntity>(       typeof(TEntity).Name + "/get/" +      id, false)[0];
        public static TEntity getEmail<TEntity>(String id)                where TEntity : class
            => getAll<TEntity>(       typeof(TEntity).Name + "/getEmail/" + ToSafeUrl(id), false)[0];

        //TODO FIX THROWS ERROR An invalid request URI was provided. The request URI must either be an absolute URI or BaseAddress must be set.
        public static HttpResponseMessage create<TEntity>(TEntity entity) where TEntity : class
            => httpClient.PutAsync(   typeof(TEntity).Name + "/put",        setContent(entity)).Result;
        public static HttpResponseMessage update<TEntity>(TEntity entity) where TEntity : class
            => httpClient.PutAsync(   typeof(TEntity).Name + "/post",       setContent(entity)).Result;

        public static HttpResponseMessage delete<TEntity, Type>(Type id)  where TEntity : class where Type : class
            => httpClient.DeleteAsync(typeof(TEntity).Name + "/delete/" +   id).Result;

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

        public static bool Ping()
        {
            HttpClient client = httpClient;
            
            var res = Task.Run(async () => await client.GetStringAsync("values").ConfigureAwait(false)).Result;

            return (JsonConvert.DeserializeObject<String>(//Json to Object of the
                res
                ) == PL.Test.t//to string result
            );
        }

        public static string ToUnsafe(string id)
        {
            return string.IsNullOrEmpty(id) ?
                throw new ArgumentNullException(nameof(id)) :
                id.Replace('-', '.');
        }
        public static string ToSafeUrl(string id)
        {
            return string.IsNullOrEmpty(id) ?
                 throw new ArgumentNullException(nameof(id)) :
                 id.Replace('.', '-');
        }
    }
}