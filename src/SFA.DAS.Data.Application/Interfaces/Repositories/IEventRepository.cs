using System.Threading.Tasks;

namespace SFA.DAS.Data.Application.Interfaces.Repositories
{
    public interface IEventRepository
    {
        Task<long> GetLastProcessedEventId(string eventFeed);

        Task StoreLastProcessedEventId(string eventFeed, long id);
    }
}
