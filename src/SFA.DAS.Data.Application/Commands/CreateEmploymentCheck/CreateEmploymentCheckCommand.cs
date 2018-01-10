using MediatR;
using SFA.DAS.EmploymentCheck.Events;

namespace SFA.DAS.Data.Application.Commands.CreateEmploymentCheck
{
    public class CreateEmploymentCheckCommand : IAsyncNotification
    {
        public EmploymentCheckCompleteEvent Event { get; set; }
    }
}
