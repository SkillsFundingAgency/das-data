using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.EAS.Account.Api.Types;

namespace SFA.DAS.Data.Tests.Builders
{
    public class AccountDetailViewModelBuilder
    {
        private string _dasAccountId = "AHC12343";
        private string _ownerEmail = "test@test.com";
        private string _dasAccountName = "Account Name";
        private DateTime _dateRegistered = DateTime.Now.AddMinutes(-1);
        private List<ResourceViewModelBuilder> _legalEntities = new List<ResourceViewModelBuilder>();
        private List<ResourceViewModelBuilder> _payeSchemes = new List<ResourceViewModelBuilder>();

        public AccountDetailViewModelBuilder WithDasAccountId(string dasAccountId)
        {
            _dasAccountId = dasAccountId;
            return this;
        }

        public AccountDetailViewModelBuilder WithLegalEntity(ResourceViewModelBuilder legalEntity)
        {
            _legalEntities.Add(legalEntity);
            return this;
        }

        public AccountDetailViewModel Build()
        {
            if (!_legalEntities.Any())
            {
                _legalEntities.Add(new ResourceViewModelBuilder());
            }

            if (!_payeSchemes.Any())
            {
                _payeSchemes.Add(new ResourceViewModelBuilder());
            }

            return new AccountDetailViewModel
            {
                DasAccountId = _dasAccountId,
                OwnerEmail = _ownerEmail,
                DasAccountName = _dasAccountName,
                DateRegistered = _dateRegistered,
                LegalEntities = new ResourceList(_legalEntities.Select(x => x.Build())),
                PayeSchemes = new ResourceList(_payeSchemes.Select(x => x.Build())),
            };
        }
    }
}
