using MediatR;

namespace SFA.DAS.Data.Application.Commands.CreateLevyDeclarations
{
    public class CreateLevyDeclarationsCommand : IAsyncNotification
    {
        public string LevyDeclarationsHref { get; set; }
    }
}
