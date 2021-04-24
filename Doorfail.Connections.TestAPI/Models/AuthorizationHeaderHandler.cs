using Doorfail.Connections.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Doorfail.Connections.API.Models
{
    public class AuthorizationHeaderhandler
        : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //TODO handle api keys
            //IEnumerable<string> apiKeyHeaderValues = null;
            //if (request.Headers.TryGetValues("cc-api-user", out apiKeyHeaderValues))//get user's name
            //{
            //    apiPassword apiUser = new apiPassword(new Password(apiKeyHeaderValues.FirstOrDefault()));//create Password instance
            //    if (!(apiUser.Key is null))//has a key
            //        if (request.Headers.TryGetValues("cc-api-key", out apiKeyHeaderValues))//get user's key
            //            if(apiUser.Key == apiKeyHeaderValues.FirstOrDefault())//check if key is correct
            return base.SendAsync(request, cancellationToken);
            //}

            var response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }
    }
}