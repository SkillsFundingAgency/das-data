using System;
using SFA.DAS.EAS.Account.Api.Types;

namespace SFA.DAS.Data.Tests.Builders
{
    public class TransactionViewModelBuilder
    {
        private string _dasAccountId = "AHC12343";
        
        public TransactionViewModelBuilder WithDasAccountId(string dasAccountId)
        {
            _dasAccountId = dasAccountId;
            return this;
        }

        public TransactionViewModel Build()
        {
            return new TransactionViewModel
            {
                HashedAccountId = _dasAccountId,
                PeriodEnd = "201702",
                TransactionDate = DateTime.Now.AddDays(-2),
                Amount = 123.43m,
                UkPrn = 23454,
                TransactionType = "Levy",
                DateCreated = DateTime.Now,
                SubmissionId = 23144,
                LevyDeclared = 345.54m,
                PayeSchemeRef = "ABC2345"
            };
        }
    }
}
