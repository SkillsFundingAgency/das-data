using MediatR;

namespace SFA.DAS.Data.Application.Commands.CreateEmployerAgreement
{
    public class CreateEmployerAgreementCommand : IAsyncNotification
    {
        public string AgreementHref { get; set; }
    }
}
