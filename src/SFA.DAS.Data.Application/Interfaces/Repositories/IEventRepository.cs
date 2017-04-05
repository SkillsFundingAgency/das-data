using System.Threading.Tasks;

namespace SFA.DAS.Data.Application.Interfaces.Repositories
{
    public interface IEventRepository
    {
        Task<T> GetLastProcessedEventId<T>(string eventFeed);

        Task StoreLastProcessedEventId<T>(string eventFeed, T id);

        Task<int> GetEventFailureCount<T>(T eventId);

        Task SetEventFailureCount<T>(T eventId, int failureCount);
    }
}
