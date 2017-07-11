using System;
using System.Threading.Tasks;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Application.Interfaces.Repositories;

namespace SFA.DAS.Data.Application.DataExtractors
{
    public class AccountsDataExtractor : IPerformancePlaformDataExtractor
    {
        private const string DataType = "account registered";

        private IPerformancePlatformRepository _performancePlatformRepository;
        private IAccountRepository _accountRepository;

        public AccountsDataExtractor(IPerformancePlatformRepository performancePlatformRepository, IAccountRepository accountRepository)
        {
            _performancePlatformRepository = performancePlatformRepository;
            _accountRepository = accountRepository;
        }

        public async Task<PerformancePlanData> Extract(DateTime extractDateTime)
        {
            var currentRecordCount = await _accountRepository.GetTotalNumberOfAccounts();
            var newRecords = await GetNumberOfNewRecords(currentRecordCount);

            return new PerformancePlanData(extractDateTime.AddDays(-1).Date, DataType, newRecords, currentRecordCount);
        }

        private async Task<long> GetNumberOfNewRecords(long currentRecordCount)
        {
            var previousRecordCount = await _performancePlatformRepository.GetNumberOfRecordsFromLastRun(DataType);
            
            var newRecords = currentRecordCount - previousRecordCount;
            return newRecords;
        }
    }
}
