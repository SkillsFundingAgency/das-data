using MediatR;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Data.Application.Commands.CreateRoatpProvider
{
    public class CreateProviderCommand : IAsyncRequest<CreateProviderResponse>
    {
        public AgreementEvent Event { get; set; }
    }
}
