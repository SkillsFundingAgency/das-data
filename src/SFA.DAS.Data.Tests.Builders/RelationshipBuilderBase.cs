using System;
using SFA.DAS.Common.Domain.Types;

namespace SFA.DAS.Data.Tests.Builders
{
    public class RelationshipBuilderBase
    {
        public string LegalEntityAddress = "Legal Entity Address";
        public string LegalEntityName = "Legal Entity Name";
        public OrganisationType LegalOrganisationType = OrganisationType.CompaniesHouse;
        public string ProviderName = "Provider Name";

        protected long Randomid
        {
            get
            {
                var rnd = new Random();
                return rnd.Next(10000, 99999);
            }
        }
    }
}
