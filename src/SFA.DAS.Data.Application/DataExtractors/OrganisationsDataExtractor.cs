using System;
using System.Threading.Tasks;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Application.DataExtractors
{
    public class OrganisationsDataExtractor : IPerformancePlatformDataExtractor
    {
        private const string DataType = "organisations added";

        private readonly IPerformancePlatformRepository _performancePlatformRepository;
        private readonly ILegalEntityRepository _legalEntityRepository;
        private readonly ILog _logger;

        public OrganisationsDataExtractor(IPerformancePlatformRepository performancePlatformRepository, ILegalEntityRepository legalEntityRepository, ILog logger)
        {
            _performancePlatformRepository = performancePlatformRepository;
            _legalEntityRepository = legalEntityRepository;
            _logger = logger;
        }

        public async Task<PerformancePlatformData> Extract(DateTime extractDateTime)
        {
            _logger.Info($"Getting organisation data for publishing to the performance platform.");

            var currentRecordCount = await _legalEntityRepository.GetTotalNumberOfLegalEntities();
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
