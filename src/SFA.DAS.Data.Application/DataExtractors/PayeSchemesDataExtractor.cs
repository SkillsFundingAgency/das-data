using System;
using System.Threading.Tasks;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Application.Interfaces.Repositories;

namespace SFA.DAS.Data.Application.DataExtractors
{
    public class PayeSchemesDataExtractor : IPerformancePlatformDataExtractor
    {
        private const string DataType = "paye schemes added";

        private IPerformancePlatformRepository _performancePlatformRepository;
        private IPayeSchemeRepository _payeSchemeRepository;

        public PayeSchemesDataExtractor(IPerformancePlatformRepository performancePlatformRepository, IPayeSchemeRepository payeSchemeRepository)
        {
            _performancePlatformRepository = performancePlatformRepository;
            _payeSchemeRepository = payeSchemeRepository;
        }

        public async Task<PerformancePlatformData> Extract(DateTime extractDateTime)
        {
            var currentRecordCount = await _payeSchemeRepository.GetTotalNumberOfPayeSchemes();
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
