using MediatR;

namespace SFA.DAS.Data.Application.Commands.CreateLegalEntity
{
    public class CreateLegalEntityCommand : IAsyncNotification
    {
        public string LegalEntityHref { get; set; }
    }
}
