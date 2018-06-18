using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using SFA.DAS.Data.Application.Commands.CreateAccount;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Functions.Ioc;
using SFA.DAS.EAS.Account.Api.Types.Events.Account;
using SFA.DAS.EmployerAccounts.Events.Messages;
using SFA.DAS.Events.Api.Types;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Functions.EmployerAccount
{
    public class ProcessAccountCreatedEvents
    {
        [FunctionName("ProcessAccountCreatedEvents")]
        public static void Run([TimerTrigger("*/15 * * * *")] ExecutionContext executionContext, TraceWriter log, [Inject] IEventService _eventService, [Inject] ILog logger,[Inject] IMediator _mediator)
        {
            var typeName = typeof(AccountCreatedEvent).Name;

            var events = _eventService.GetUnprocessedGenericEvents(typeName).Result;

            var eventModels = events?.Select(x => JsonConvert.DeserializeObject<AccountCreatedEvent>(x.Payload)).ToList();
            

            Parallel.ForEach(eventModels, (currentEvent) =>
            {
                _mediator.PublishAsync(new CreateAccountCommand { AccountHref = currentEvent.ResourceUri });
            });
        }
    }
}
