using AutoMapper;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Data.Worker.Mapping
{
    public class EventsMapping : Profile
    {
        public EventsMapping()
        {
            CreateMap<ApprenticeshipEventView, CommitmentsApprenticeshipEvent>();
        }
    }
}
