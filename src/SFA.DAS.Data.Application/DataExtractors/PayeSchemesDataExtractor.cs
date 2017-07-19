using System;
using System.Threading.Tasks;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Application.DataExtractors
{
    public class PayeSchemesDataExtractor : IPerformancePlatformDataExtractor
    {
        private const string DataType = "paye schemes added";

        private readonly IPerformancePlatformRepository _performancePlatformRepository;
        private readonly IPayeSchemeRepository _payeSchemeRepository;
        private readonly ILog _logger;

        public PayeSchemesDataExtractor(IPerformancePlatformRepository performancePlatformRepository, IPayeSchemeRepository payeSchemeRepository, ILog logger)
        {
            _performancePlatformRepository = performancePlatformRepository;
            _payeSchemeRepository = payeSchemeRepository;
            _logger = logger;
        }

        public async Task<PerformancePlatformData> Extract(DateTime extractDateTime)
        {
            _logger.Info($"Getting paye scheme data for publishing to the performance platform.");

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
