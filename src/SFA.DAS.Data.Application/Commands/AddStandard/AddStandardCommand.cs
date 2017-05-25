using MediatR;

namespace SFA.DAS.Data.Application.Commands.AddStandard
{
    public class AddStandardCommand : IAsyncNotification
    {
        public string StandardId { get; set; }
    }
}
