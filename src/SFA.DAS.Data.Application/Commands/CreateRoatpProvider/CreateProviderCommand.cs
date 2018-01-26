using MediatR;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Data.Application.Commands.CreateRoatpProvider
{
    public class CreateProviderCommand : IAsyncNotification
    {
        public AgreementEventView Event { get; set; }
    }
}
