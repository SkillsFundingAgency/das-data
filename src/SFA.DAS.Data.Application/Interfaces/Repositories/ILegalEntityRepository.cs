using System.Threading.Tasks;
using SFA.DAS.EAS.Account.Api.Types;

namespace SFA.DAS.Data.Application.Interfaces.Repositories
{
    public interface ILegalEntityRepository
    {
        Task SaveLegalEntity(LegalEntityViewModel legalEntity);
        Task<long> GetTotalNumberOfLegalEntities();
    }
}
