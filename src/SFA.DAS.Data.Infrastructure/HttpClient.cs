using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SFA.DAS.Data.Application.Interfaces;

namespace SFA.DAS.Data.Infrastructure
{
    public class HttpClient : IHttpClient
    {
        public async Task PostAsync(string url, string data, string token)
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await client.PostAsync(url, new StringContent(data, Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();
            }
        }
    }
}
