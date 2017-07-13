using System;
using System.Threading.Tasks;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Application.Interfaces.Repositories;

namespace SFA.DAS.Data.Application.DataExtractors
{
    public class OrganisationsDataExtractor : IPerformancePlatformDataExtractor
    {
        private const string DataType = "organisations added";

        private IPerformancePlatformRepository _performancePlatformRepository;
        private ILegalEntityRepository _legalEntityRepository;

        public OrganisationsDataExtractor(IPerformancePlatformRepository performancePlatformRepository, ILegalEntityRepository legalEntityRepository)
        {
            _performancePlatformRepository = performancePlatformRepository;
            _legalEntityRepository = legalEntityRepository;
        }

        public async Task<PerformancePlatformData> Extract(DateTime extractDateTime)
        {
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
