using MediatR;

namespace SFA.DAS.Data.Application.Commands.RemovePayeScheme
{
    public class RemovePayeSchemeCommand : IAsyncNotification
    {
        public string PayeSchemeHref { get; set; }
    }
}
