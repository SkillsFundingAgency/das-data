using System;
using SFA.DAS.Provider.Events.Api.Types;

namespace SFA.DAS.Data.Tests.Builders
{
    public class PaymentBuilder
    {
        private decimal _amount = 1234m;
        private int _apprenticeshipId = 123;
        private string _apprenticeshipVersion = "V1";
        private string _period = "PERIOD";
        private int _collectionMonth = 1;
        private int _collectionYear = 2017;
        private string _id = "ID";
        private string _employerAccountId = "ABC123";
        private int _deliveryMonth = 1;
        private int _deliveryYear = 2017;
        private int _ukprn = 12345;
        private FundingSource _fundingSource = FundingSource.CoInvestedEmployer;
        private int _programmeType = 12;
        private int _standardCode = 324;
        private int _frameworkCode = 435;
        private TransactionType _transactionType = TransactionType.Balancing;
        private DateTime _evidenceSubmittedOn = DateTime.Now;
        private int _pathwayCode = 234;
        private int _uln = 346905;
        private string _employerAccountVersion = "V2";
        private ContractType _contractType = ContractType.ContractWithEmployer;

        public PaymentBuilder WithId(string id)
        {
            _id = id;
            return this;
        }

        public PaymentBuilder WithPeriod(string period)
        {
            _period = period;
            return this;
        }

        public Payment Build()
        {
            return new Payment
            {
                Amount = _amount,
                ApprenticeshipId = _apprenticeshipId,
                ApprenticeshipVersion = _apprenticeshipVersion,
                CollectionPeriod = new NamedCalendarPeriod
                {
                    Id = _period,
                    Month = _collectionMonth,
                    Year = _collectionYear
                },
                Id = _id,
                EmployerAccountId = _employerAccountId,
                DeliveryPeriod = new CalendarPeriod
                {
                    Month = _deliveryMonth,
                    Year = _deliveryYear
                },
                Ukprn = _ukprn,
                FundingSource = _fundingSource,
                ProgrammeType = _programmeType,
                StandardCode = _standardCode,
                FrameworkCode = _frameworkCode,
                TransactionType = _transactionType,
                EvidenceSubmittedOn = _evidenceSubmittedOn,
                PathwayCode = _pathwayCode,
                Uln = _uln,
                EmployerAccountVersion = _employerAccountVersion,
                ContractType = _contractType
            };
        }
    }
}
