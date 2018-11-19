using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.Data.Domain.Models.Statistics.Commitments;

namespace SFA.DAS.Data.Domain.Interfaces
{
    public interface ICommitmentsStatisticsHandler
    {
        Task<CommitmentsExternalModel> Handle();
    }
}
