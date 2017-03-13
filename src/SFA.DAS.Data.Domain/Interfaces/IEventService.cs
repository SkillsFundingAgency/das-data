﻿using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Data.Domain.Interfaces
{
    public interface IEventService
    {
        Task<ICollection<GenericEvent>> GetUnprocessedGenericEvents(string eventType);

        Task<ICollection<ApprenticeshipEventView>> GetUnprocessedApprenticeshipEvents();

        Task<ICollection<AccountEventView>> GetUnprocessedAccountEvents();
    }
}