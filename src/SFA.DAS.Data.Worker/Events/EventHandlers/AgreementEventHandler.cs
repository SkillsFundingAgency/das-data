using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SFA.DAS.Data.Application.Commands.CreateRoatpProvider;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Events.Api.Types;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Worker.Events.EventHandlers
{
    public class AgreementEventHandler : EventApiHandler<AgreementEventView>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AgreementEventHandler(IMediator mediator,
            IMapper mapper, IEventRepository eventRepository, 
            IDataConfiguration configuration, ILog logger) 
            : base(eventRepository, configuration, logger)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        protected override async Task ProcessEvent(AgreementEventView @event)
        {
            await _mediator.PublishAsync(new CreateProviderCommand() {Event = @event});
        }
    }
}
