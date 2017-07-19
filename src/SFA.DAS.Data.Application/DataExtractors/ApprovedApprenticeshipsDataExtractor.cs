using System;
using System.Threading.Tasks;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Application.DataExtractors
{
    public class ApprovedApprenticeshipsDataExtractor : IPerformancePlatformDataExtractor
    {
        private const string DataType = "apprentices approved";

        private readonly IPerformancePlatformRepository _performancePlatformRepository;
        private readonly IApprenticeshipRepository _apprenticeshipRepository;
        private readonly ILog _logger;

        public ApprovedApprenticeshipsDataExtractor(IPerformancePlatformRepository performancePlatformRepository, IApprenticeshipRepository apprenticeshipRepository, ILog logger)
        {
            _performancePlatformRepository = performancePlatformRepository;
            _apprenticeshipRepository = apprenticeshipRepository;
            _logger = logger;
        }

        public async Task<PerformancePlatformData> Extract(DateTime extractDateTime)
        {
            _logger.Info($"Getting approved apprenticeships data for publishing to the performance platform.");
            var currentRecordCount = await _apprenticeshipRepository.GetTotalNumberOfAgreedApprenticeships();
            var newRecords = await GetNumberOfNewRecords(currentRecordCount);

            return new PerformancePlatformData(extractDateTime.AddDays(-1).Date, DataType, newRecords, currentRecordCount);
        }

        private async Task<long> GetNumberOfNewRecords(long currentRecordCount)
        {
            var previousRecordCount = await _performancePlatformRepository.GetNumberOfRecordsFromLastRun(DataType);
            
            var newRecords = currentRecordCount - previousRecordCount;
            return newRecords;
        }
    }
}
