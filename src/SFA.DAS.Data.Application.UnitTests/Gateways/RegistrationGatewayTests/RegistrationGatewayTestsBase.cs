using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Gateways;
using SFA.DAS.Data.Tests.Builders;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.EAS.Account.Api.Client.Dtos;

namespace SFA.DAS.Data.Application.UnitTests.Gateways.RegistrationGatewayTests
{
    public abstract class RegistrationGatewayTestsBase
    {
        protected RegistrationGateway RegistrationGateway;
        protected Mock<IAccountApiClient> AccountApiClient;

        [SetUp]
        public void Arrange()
        {
            AccountApiClient = new Mock<IAccountApiClient>();

            RegistrationGateway = new RegistrationGateway(AccountApiClient.Object);
        }

        protected static PagedApiResponseViewModel<AccountInformationViewModel> CreateGetPageOfAccountInformationResponse(List<AccountInformationViewModel> accountInformationList, int page, int totalPages)
        {
            var accountInformationResponse = new PagedApiResponseViewModel<AccountInformationViewModel>
            {
                Data = accountInformationList,
                Page = page,
                TotalPages = totalPages
            };
            return accountInformationResponse;
        }

        protected static List<AccountInformationViewModel> CreateExpectedAccountInformation(string dasAccountId)
        {
            var expectedAccountInformation = new List<AccountInformationViewModel>
            {
                new AccountInformationViewModelBuilder().WithDasAccountId(dasAccountId).Build(),
                new AccountInformationViewModelBuilder().WithDasAccountId(dasAccountId).Build()
            };
            return expectedAccountInformation;
        }

        protected List<AccountInformationViewModel> GenerateAccountList(int numberOfItems)
        {
            var accountInformationList = new List<AccountInformationViewModel>();
            for (var i = 0; i < numberOfItems; i++)
            {
                accountInformationList.Add(new AccountInformationViewModelBuilder().Build());
            }
            return accountInformationList;
        }
    }
}
