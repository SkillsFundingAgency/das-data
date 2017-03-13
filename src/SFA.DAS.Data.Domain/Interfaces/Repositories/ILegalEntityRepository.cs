using System.Threading.Tasks;
using SFA.DAS.EAS.Account.Api.Types;

namespace SFA.DAS.Data.Domain.Interfaces.Repositories
{
    public interface ILegalEntityRepository
    {
        Task SaveLegalEntity(LegalEntityViewModel legalEntity);
    }
}
