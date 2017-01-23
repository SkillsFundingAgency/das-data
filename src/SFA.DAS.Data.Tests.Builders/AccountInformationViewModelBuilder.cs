using System;
using SFA.DAS.EAS.Account.Api.Client.Dtos;

namespace SFA.DAS.Data.Tests.Builders
{
    public class AccountInformationViewModelBuilder
    {
        private string _dasAccountId = "AHC12343";
        private string _ownerEmail = "test@test.com";
        private string _dasAccountName = "Account Name";
        private DateTime _dateRegistered = DateTime.Now.AddMinutes(-1);
        private string _organisationStatus = "Status";
        private string _organisationName = "Organisation Name";
        private DateTime _organsiationCreatedDate = DateTime.Now.AddYears(-1);
        private string _organisationSource = "Companies House";
        private string _organisationRegisteredAddress = "Some Street, Some Town";
        private string _organisationNumber = "ORGNO";
        private long _organisationId = 83475;
        private string _payeSchemeName = "PAYESCHEME";

        public AccountInformationViewModelBuilder WithDasAccountId(string dasAccountId)
        {
            _dasAccountId = dasAccountId;
            return this;
        }

        public AccountInformationViewModel Build()
        {
            return new AccountInformationViewModel
            {
                DasAccountId = _dasAccountId,
                OwnerEmail = _ownerEmail,
                DasAccountName = _dasAccountName,
                DateRegistered = _dateRegistered,
                OrganisationStatus = _organisationStatus,
                OrganisationName = _organisationName,
                OrgansiationCreatedDate = _organsiationCreatedDate,
                OrganisationSource = _organisationSource,
                OrganisationRegisteredAddress = _organisationRegisteredAddress,
                OrganisationNumber = _organisationNumber,
                OrganisationId = _organisationId,
                PayeSchemeName = _payeSchemeName,
            };
        }
    }
}
