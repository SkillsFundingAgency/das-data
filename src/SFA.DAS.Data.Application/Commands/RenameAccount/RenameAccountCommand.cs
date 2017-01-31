using MediatR;

namespace SFA.DAS.Data.Application.Commands.RenameAccount
{
    public class RenameAccountCommand : IAsyncNotification
    {
        public string AccountHref { get; set; }
    }
}
