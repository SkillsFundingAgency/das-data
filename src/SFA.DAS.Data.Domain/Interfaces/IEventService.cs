using System.Collections.Generic;
using SFA.DAS.Data.Domain.Models;

namespace SFA.DAS.Data.Domain.Interfaces
{
    public interface IEventService
    {
        ICollection<T> GetEvents<T>();

        ICollection<CommitmentsApprenticeshipEvent> GetApprenticeshipEvents();
    }
}
