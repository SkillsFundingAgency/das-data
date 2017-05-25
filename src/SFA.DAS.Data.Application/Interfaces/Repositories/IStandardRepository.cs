using System.Threading.Tasks;
using SFA.DAS.Apprenticeships.Api.Types;

namespace SFA.DAS.Data.Application.Interfaces.Repositories
{
    public interface IStandardRepository
    {
        Task SaveStandard(Standard standard);
    }
}
