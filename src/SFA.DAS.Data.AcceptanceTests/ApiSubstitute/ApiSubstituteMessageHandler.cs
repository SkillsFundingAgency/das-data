using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Data.AcceptanceTests.ApiSubstitute
{
    public class ApiSubstituteMessageHandler : DelegatingHandler
    {
        private Dictionary<string, object> _configuredGets = new Dictionary<string, object>();

        public void SetupGet(string url, object response)
        {
            _configuredGets.Add(url, response);
        }

        public void ClearSetup()
        {
            _configuredGets.Clear();
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var requestUri = request.RequestUri.ToString();
            Trace.WriteLine("Capturing request " + requestUri);
            HttpResponseMessage response;
            if (!_configuredGets.ContainsKey(requestUri))
            {
                response = request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                response = request.CreateResponse(HttpStatusCode.OK, _configuredGets[requestUri]);
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            Trace.WriteLine("Responding to request " + requestUri);
            return tsc.Task;
        }
    }
}
