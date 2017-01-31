using System.Threading.Tasks;
using SFA.DAS.EAS.Account.Api.Types;

namespace SFA.DAS.Data.Application.Interfaces.Repositories
{
    public interface IPayeSchemeRepository
    {
        Task SavePayeScheme(PayeSchemeViewModel payeScheme);
    }
}
