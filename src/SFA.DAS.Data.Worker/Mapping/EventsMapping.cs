﻿using System;
using AutoMapper;
using SFA.DAS.Data.Domain.Models;
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
                    config => config.MapFrom(source => Enum.GetName(typeof(AgreementStatus), source.AgreementStatus)));
        }
    }
}
