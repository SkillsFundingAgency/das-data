using System.Threading.Tasks;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.EAS.Account.Api.Types;

namespace SFA.DAS.Data.Application.Interfaces.Repositories
{
    public interface ITransferRelationshipRepository
    {
        Task SaveTransferRelationship(TransferRelationship transferRelationship);
       
    }
}
