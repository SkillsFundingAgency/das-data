using MediatR;

namespace SFA.DAS.Data.Application.Commands.CreateAccount
{
    public class CreateAccountCommand : IAsyncNotification
    {
        public string AccountHref { get; set; }
    }
}
