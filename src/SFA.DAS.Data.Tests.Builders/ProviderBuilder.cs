using System;
using SFA.Roatp.Api.Types;

namespace SFA.DAS.Data.Tests.Builders
{
    public class ProviderBuilder
    {
        private long _ukPrn = 12345678;
        private string _uri = "https://localhost:8005/api/providers/12345678";
        private ProviderType _providerType = ProviderType.MainProvider;
        private bool _parentCompanyGuarantee = false;
        private bool _newOrganisationWithoutFinancialTrackRecord = false;
        private readonly DateTime? _startDate = DateTime.Now.AddDays(-1);

        public Roatp.Api.Types.Provider Build()
        {
            return new Roatp.Api.Types.Provider()
            {
                Ukprn = _ukPrn,
                Uri = _uri,
                ProviderType = _providerType,
                ParentCompanyGuarantee = _parentCompanyGuarantee,
                NewOrganisationWithoutFinancialTrackRecord = _newOrganisationWithoutFinancialTrackRecord,
                StartDate = _startDate
            };
        }
    }
}
