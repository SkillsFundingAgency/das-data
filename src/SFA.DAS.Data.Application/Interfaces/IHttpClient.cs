using System.Threading.Tasks;

namespace SFA.DAS.Data.Application.Interfaces
{
    public interface IHttpClient
    {
        Task PostAsync(string url, string data, string token);
    }
}
