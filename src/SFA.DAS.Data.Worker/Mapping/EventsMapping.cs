using System;
using AutoMapper;
using SFA.DAS.Events.Api.Types;
using ApprenticeshipEvent = SFA.DAS.Data.Domain.Models.ApprenticeshipEvent;

namespace SFA.DAS.Data.Worker.Mapping
{
    public class EventsMapping : Profile
    {
        public EventsMapping()
        {
            CreateMap<ApprenticeshipEventView, ApprenticeshipEvent>()
                .ForMember(target => target.PaymentStatus, 
                    config => config.MapFrom(source => Enum.GetName(typeof(PaymentStatus), source.PaymentStatus)))
                .ForMember(target => target.AgreementStatus,
                    config => config.MapFrom(source => Enum.GetName(typeof(AgreementStatus), source.AgreementStatus)))
                .ForMember(target => target.LegalEntityCode, config => config.MapFrom(source => source.LegalEntityId));

            CreateMap<AgreementEventView, AgreementEvent>()
                .ForMember(target => target.Event, config => config.MapFrom(source => source.Event))
                .ForMember(target => target.ProviderId, config => config.MapFrom(source => source.ProviderId))
                .ForMember(target => target.ContractType, config => config.MapFrom(source => source.ContractType));
        }
    }
}
