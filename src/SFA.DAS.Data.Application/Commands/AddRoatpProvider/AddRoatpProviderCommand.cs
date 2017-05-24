using MediatR;

namespace SFA.DAS.Data.Application.Commands.AddRoatpProvider
{
    public class AddRoatpProviderCommand : IAsyncNotification
    {
        public long ProviderId { get; set; }
    }
}
