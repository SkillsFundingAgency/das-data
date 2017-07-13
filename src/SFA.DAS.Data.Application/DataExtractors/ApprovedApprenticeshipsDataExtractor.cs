﻿using System;
using System.Threading.Tasks;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Application.Interfaces.Repositories;

namespace SFA.DAS.Data.Application.DataExtractors
{
    public class ApprovedApprenticeshipsDataExtractor : IPerformancePlatformDataExtractor
    {
        private const string DataType = "apprentices approved";

        private IPerformancePlatformRepository _performancePlatformRepository;
        private IApprenticeshipRepository _apprenticeshipRepository;

        public ApprovedApprenticeshipsDataExtractor(IPerformancePlatformRepository performancePlatformRepository, IApprenticeshipRepository apprenticeshipRepository)
        {
            _performancePlatformRepository = performancePlatformRepository;
            _apprenticeshipRepository = apprenticeshipRepository;
        }

        public async Task<PerformancePlatformData> Extract(DateTime extractDateTime)
        {
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
