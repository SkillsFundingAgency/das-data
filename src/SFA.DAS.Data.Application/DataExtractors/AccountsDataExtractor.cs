using System;
using System.Threading.Tasks;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Application.DataExtractors
{
    public class AccountsDataExtractor : IPerformancePlatformDataExtractor
    {
        private const string DataType = "account registered";

        private readonly IPerformancePlatformRepository _performancePlatformRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ILog _logger;

        public AccountsDataExtractor(IPerformancePlatformRepository performancePlatformRepository, IAccountRepository accountRepository, ILog logger)
        {
            _performancePlatformRepository = performancePlatformRepository;
            _accountRepository = accountRepository;
            _logger = logger;
        }

        public async Task<PerformancePlatformData> Extract(DateTime extractDateTime)
        {
            _logger.Info($"Getting accounts data for publishing to the performance platform.");

            var currentRecordCount = await _accountRepository.GetTotalNumberOfAccounts();
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
