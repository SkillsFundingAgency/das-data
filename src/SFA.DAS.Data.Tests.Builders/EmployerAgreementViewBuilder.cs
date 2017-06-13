using System;
using SFA.DAS.EAS.Account.Api.Types;

namespace SFA.DAS.Data.Tests.Builders
{
    public class EmployerAgreementViewBuilder
    {
        private int _id = 12435;
        private int _legalEntityId = 4536;
        private string _legalEntityCode = "AH2345";
        private string _hashedAccountId = "ABC123";
        private string _legalEntityName = "Test";
        private EmployerAgreementStatus _employerAgreementStatus = EmployerAgreementStatus.Signed;
        private string _signedByName = "Jim";
        private DateTime? _expiredDate = null;
        private string _hashedAgreementId = "321CBA";
        private DateTime _signedDate = DateTime.Now;
        private int _accountId = 453;
        private string _legalEntityAddress = "Address";
        private DateTime _legalEntityInceptionDate = DateTime.Now.AddYears(-10);
        private string _legalEntityStatus = "active";
        private string _sector = "sector";
        private int _templateId = 32;
        private string _templatePartialViewName = "Name";

        public EmployerAgreementViewBuilder WithDasAccountId(string dasAccountId)
        {
            _hashedAccountId = dasAccountId;
            return this;
        }

        public EmployerAgreementViewBuilder WithDasAgreementId(string dasAgreementId)
        {
            _hashedAgreementId = dasAgreementId;
            return this;
        }

        public EmployerAgreementView Build()
        {
            return new EmployerAgreementView
            {
                Id = _id,
                LegalEntityId = _legalEntityId,
                LegalEntityCode = _legalEntityCode,
                HashedAccountId = _hashedAccountId,
                LegalEntityName = _legalEntityName,
                Status = _employerAgreementStatus,
                SignedByName = _signedByName,
                ExpiredDate = _expiredDate,
                HashedAgreementId = _hashedAgreementId,
                SignedDate = _signedDate,
                AccountId = _accountId,
                LegalEntityAddress = _legalEntityAddress,
                LegalEntityInceptionDate = _legalEntityInceptionDate,
                LegalEntityStatus = _legalEntityStatus,
                Sector = _sector,
                TemplateId = _templateId,
                TemplatePartialViewName = _templatePartialViewName
            };
        }
    }
}
