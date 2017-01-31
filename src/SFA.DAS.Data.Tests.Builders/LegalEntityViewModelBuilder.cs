using System;
using SFA.DAS.EAS.Account.Api.Types;

namespace SFA.DAS.Data.Tests.Builders
{
    public class LegalEntityViewModelBuilder
    {
        private string _dasAccountId = "AHC12343";
        private string _address = "Some address";
        private string _code = "Code";
        private DateTime _dateOfInception = DateTime.Now.AddYears(-3);
        private long _legalEntityId = 123;
        private string _name = "Name";
        private string _source = "source";
        private string _status = "status";

        public LegalEntityViewModelBuilder WithDasAccountId(string dasAccountId)
        {
            _dasAccountId = dasAccountId;
            return this;
        }

        public LegalEntityViewModelBuilder WithLegalEntityId(long legalEntityId)
        {
            _legalEntityId = legalEntityId;
            return this;
        }

        public LegalEntityViewModel Build()
        {
            return new LegalEntityViewModel
            {
                DasAccountId = _dasAccountId,
                Address = _address,
                Code = _code,
                DateOfInception = _dateOfInception,
                LegalEntityId = _legalEntityId,
                Name = _name,
                Source = _source,
                Status = _status
            };
        }
    }
}
