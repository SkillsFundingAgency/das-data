using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Provider.Events.Api.Types;

namespace SFA.DAS.Data.Application.Interfaces.Repositories
{
    public interface IPaymentRepository
    {
        Task SavePayments(IEnumerable<Payment> payment);
    }
    public interface ITransferRepository
    {
        Task SaveTransfers(IEnumerable<AccountTransfer> transfers);
    }
}
