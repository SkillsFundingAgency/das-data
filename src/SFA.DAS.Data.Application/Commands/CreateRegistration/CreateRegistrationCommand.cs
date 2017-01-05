using MediatR;

namespace SFA.DAS.Data.Application.Commands.CreateRegistration
{
    public class CreateRegistrationCommand : IAsyncRequest
    {
        public int OrganisationId { get; set; }
    }
}
