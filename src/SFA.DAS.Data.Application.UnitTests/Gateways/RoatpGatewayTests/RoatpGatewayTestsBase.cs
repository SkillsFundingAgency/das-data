using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Gateways;
using SFA.Roatp.Api.Client;
using SFA.Roatp.Api.Types;

namespace SFA.DAS.Data.Application.UnitTests.Gateways.RoatpGatewayTests
{
    public abstract class RoatpGatewayTestsBase
    {
        protected RoatpGateway RoatpGateway;
        protected Mock<IRoatpClient> RoatpClient;
        protected IEnumerable<Roatp.Api.Types.Provider> AllProviders;

        protected readonly string ValidUkPrn = "10007315";

        [SetUp]
        public void Arrange()
        {
            RoatpClient = new Mock<IRoatpClient>();
            RoatpGateway = new RoatpGateway(RoatpClient.Object);

            RoatpClient.Setup(y => y.Get(ValidUkPrn)).Returns(new Roatp.Api.Types.Provider());
            RoatpClient.Setup(y => y.Exists(ValidUkPrn)).Returns(true);

            SetupListOfAllProviders();

            RoatpClient.Setup(x => x.FindAll()).Returns(AllProviders);
        }

        private void SetupListOfAllProviders()
        {
            AllProviders = new List<Roatp.Api.Types.Provider>
            {
                new Roatp.Api.Types.Provider()
                {
                    NewOrganisationWithoutFinancialTrackRecord = true,
                    ParentCompanyGuarantee = true,
                    ProviderType = ProviderType.EmployerProvider,
                    StartDate = DateTime.Now,
                    Ukprn = 10007315,
                    Uri = "ABC"
                },
                new Roatp.Api.Types.Provider()
                {
                    NewOrganisationWithoutFinancialTrackRecord = true,
                    ParentCompanyGuarantee = true,
                    ProviderType = ProviderType.MainProvider,
                    StartDate = DateTime.Now,
                    Ukprn = 10007316,
                    Uri = "XYZ"
                },
                new Roatp.Api.Types.Provider()
                {
                    NewOrganisationWithoutFinancialTrackRecord = true,
                    ParentCompanyGuarantee = true,
                    ProviderType = ProviderType.SupportingProvider,
                    StartDate = DateTime.Now,
                    Ukprn = 10007317,
                    Uri = "123"
                }
            };
        }
    }
}
