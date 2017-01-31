using MediatR;

namespace SFA.DAS.Data.Application.Commands.CreatePayeScheme
{
    public class CreatePayeSchemeCommand : IAsyncNotification
    {
        public string PayeSchemeHref { get; set; }
    }
}
