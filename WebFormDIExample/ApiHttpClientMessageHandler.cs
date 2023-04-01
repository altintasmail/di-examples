using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace WebApplication2.HttpClientHandlers
{
    public class ApiHttpClientMessageHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //belki headere token ekliyoruzdur.
            request.Headers.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: "token-from-session");
            return base.SendAsync(request, cancellationToken);
        }
    }
}