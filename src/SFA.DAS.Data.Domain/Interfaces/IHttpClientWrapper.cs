using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.Data.Domain.Interfaces
{
    public interface IHttpClientWrapper
    {
        Task<HttpResponseMessage> GetAsync(Uri requestUri, string contentType = Constants.ContentTypeValue);

        Task<T> ReadResponse<T>(HttpResponseMessage responseMessage);

        Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent postContent, IEnumerable<KeyValuePair<string, string>> headers);
    }
}
