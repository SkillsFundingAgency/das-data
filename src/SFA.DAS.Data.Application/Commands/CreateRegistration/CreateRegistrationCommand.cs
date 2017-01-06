using MediatR;

namespace SFA.DAS.Data.Application.Commands.CreateRegistration
{
    public class CreateRegistrationCommand : IAsyncRequest
    {
        public string DasAccountId { get; set; }
    }
}
