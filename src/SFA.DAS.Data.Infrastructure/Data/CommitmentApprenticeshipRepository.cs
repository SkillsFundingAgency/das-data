using System;
using SFA.DAS.Data.Domain.Models;

namespace SFA.DAS.Data.Infrastructure.Data
{
    public class CommitmentApprenticeshipRepository : BaseRepository
    {
        public CommitmentApprenticeshipRepository(string connectionString) : base(connectionString)
        {

        }

        public void Create(CommitmentsApprenticeshipEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}
