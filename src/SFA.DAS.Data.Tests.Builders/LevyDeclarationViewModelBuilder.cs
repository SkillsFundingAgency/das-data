using System;
using SFA.DAS.EAS.Account.Api.Types;

namespace SFA.DAS.Data.Tests.Builders
{
    public class LevyDeclarationViewModelBuilder
    {
        private string _dasAccountId = "AHC12343";
        private DateTime _inactiveFrom = DateTime.Now.AddDays(1);
        private short _payrollMonth = 2;
        private bool _endOfYearAdjustment = true;
        private int _topupPercentage = 1;
        private string _payrollYear = "2017";
        private decimal _endOfYearAdjustmentAmount = 123.45m;
        private int _submissionId = 123;
        private decimal _englishFraction = 0.2m;
        private DateTime _createdDate = DateTime.Now;
        private DateTime _inactiveTo = DateTime.Now.AddYears(1);
        private DateTime _dateCeased = DateTime.Now.AddDays(-1);
        private string _payeSchemeReference = "ABC123";
        private DateTime _submissionDate = DateTime.Now.AddDays(-3);
        private int _hmrcSubmissionId = 4576;
        private decimal _levyDueYearToDate = 45366.32m;
        private int _levyDeclarationId = 43256;
        private decimal _levyAllowanceForYear = 32478.1m;
        private decimal _topupAmount = 435.4m;

        public LevyDeclarationViewModelBuilder WithDasAccountId(string dasAccountId)
        {
            _dasAccountId = dasAccountId;
            return this;
        }

        public LevyDeclarationViewModel Build()
        {
            return new LevyDeclarationViewModel
            {
                HashedAccountId = _dasAccountId,
                InactiveFrom = _inactiveFrom,
                PayrollMonth = _payrollMonth,
                EndOfYearAdjustment = _endOfYearAdjustment,
                TopUpPercentage = _topupPercentage,
                PayrollYear = _payrollYear,
                EndOfYearAdjustmentAmount = _endOfYearAdjustmentAmount,
                SubmissionId = _submissionId,
                EnglishFraction = _englishFraction,
                CreatedDate = _createdDate,
                InactiveTo = _inactiveTo,
                DateCeased = _dateCeased,
                PayeSchemeReference = _payeSchemeReference,
                SubmissionDate = _submissionDate,
                HmrcSubmissionId = _hmrcSubmissionId,
                LevyDueYtd = _levyDueYearToDate,
                Id = _levyDeclarationId,
                LevyAllowanceForYear = _levyAllowanceForYear,
                TopUp = _topupAmount
            };
        }
    }
}
