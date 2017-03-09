using System;
using AutoMapper;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Data.Worker.Mapping
{
    public class EventsMapping : Profile
    {
        public EventsMapping()
        {
            CreateMap<ApprenticeshipEventView, CommitmentsApprenticeshipEvent>()
                .ForMember(target => target.PaymentStatus, 
                    config => config.MapFrom(source => Enum.GetName(typeof(PaymentStatus), source.PaymentStatus)))
                .ForMember(target => target.AgreementStatus,
                    config => config.MapFrom(source => Enum.GetName(typeof(AgreementStatus), source.AgreementStatus)));
        }
    }
}
