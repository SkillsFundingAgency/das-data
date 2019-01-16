using System.Threading.Tasks;
using SFA.DAS.Provider.Events.Api.Types;

namespace SFA.DAS.Data.Application.Interfaces.Repositories
{
    public interface IDataLockRepository
    {
        Task SaveDataLock(DataLockEvent dataLock);
    }
}
