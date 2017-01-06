using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Gateways;
using SFA.DAS.Data.Application.UnitTests.Builders;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.EAS.Account.Api.Client.Dtos;

namespace SFA.DAS.Data.Application.UnitTests.Gateways.RegistrationGatewayTests
{
    [TestFixture]
    public class WhenTheRegistrationIsInThePageOfAccountsInformation
    {
        private RegistrationGateway _registrationGateway;
        private Mock<IAccountApiClient> _accountApiClient;

        [SetUp]
        public void Arrange()
        {
            _accountApiClient = new Mock<IAccountApiClient>();

            _registrationGateway = new RegistrationGateway(_accountApiClient.Object);
        }

        [Test]
        public async Task ThenTheAccountInformationIsReturned()
        {
            var dasAccountId = "2385";
            var accountInformationList = GenerateAccountList(10);
            var expectedAccountInformation = CreateExpectedAccountInformation(dasAccountId);
                
            accountInformationList.InsertRange(7, expectedAccountInformation);

            var accountInformationResponse = CreateGetPageOfAccountInformationResponse(accountInformationList, 1, 3);
            _accountApiClient.Setup(x => x.GetPageOfAccountInformation(DateTime.MinValue, DateTime.MaxValue, 1, 1000)).ReturnsAsync(accountInformationResponse);

            var result = await _registrationGateway.GetRegistration(dasAccountId);

            result.ShouldAllBeEquivalentTo(expectedAccountInformation);
            _accountApiClient.Verify(x => x.GetPageOfAccountInformation(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Test]
        public async Task IfTheAccountInformationSpansMultiplePagesAndThereAreMorePages()
        {
            var dasAccountId = "2385";
            var expectedAccountInformation = CreateExpectedAccountInformation(dasAccountId);

            var accountInformationListPage1 = GenerateAccountList(9);
            accountInformationListPage1.Add(expectedAccountInformation.First());
            var accountInformationResponsePage1 = CreateGetPageOfAccountInformationResponse(accountInformationListPage1, 1, 3);
            _accountApiClient.Setup(x => x.GetPageOfAccountInformation(DateTime.MinValue, DateTime.MaxValue, 1, 1000)).ReturnsAsync(accountInformationResponsePage1);

            var accountInformationListPage2 = GenerateAccountList(9);
            accountInformationListPage2.Insert(0, expectedAccountInformation.Last());
            var accountInformationResponsePage2 = CreateGetPageOfAccountInformationResponse(accountInformationListPage2, 2, 3);
            _accountApiClient.Setup(x => x.GetPageOfAccountInformation(DateTime.MinValue, DateTime.MaxValue, 2, 1000)).ReturnsAsync(accountInformationResponsePage2);

            var result = await _registrationGateway.GetRegistration(dasAccountId);

            result.ShouldAllBeEquivalentTo(expectedAccountInformation);
            _accountApiClient.Verify(x => x.GetPageOfAccountInformation(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(2));
        }

        [Test]
        public async Task IfTheAccountInformationSpansMultiplePagesAndThereAreNoMorePages()
        {
            var dasAccountId = "2385";
            var expectedAccountInformation = CreateExpectedAccountInformation(dasAccountId);

            var accountInformationListPage1 = GenerateAccountList(8);
            accountInformationListPage1.AddRange(expectedAccountInformation);
            var accountInformationResponsePage1 = CreateGetPageOfAccountInformationResponse(accountInformationListPage1, 1, 1);
            _accountApiClient.Setup(x => x.GetPageOfAccountInformation(DateTime.MinValue, DateTime.MaxValue, 1, 1000)).ReturnsAsync(accountInformationResponsePage1);

            var result = await _registrationGateway.GetRegistration(dasAccountId);

            result.ShouldAllBeEquivalentTo(expectedAccountInformation);
            _accountApiClient.Verify(x => x.GetPageOfAccountInformation(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(1));
        }

        private static PagedApiResponseViewModel<AccountInformationViewModel> CreateGetPageOfAccountInformationResponse(List<AccountInformationViewModel> accountInformationList, int page, int totalPages)
        {
            var accountInformationResponse = new PagedApiResponseViewModel<AccountInformationViewModel>
            {
                Data = accountInformationList,
                Page = page,
                TotalPages = totalPages
            };
            return accountInformationResponse;
        }

        private static List<AccountInformationViewModel> CreateExpectedAccountInformation(string dasAccountId)
        {
            var expectedAccountInformation = new List<AccountInformationViewModel>
            {
                new AccountInformationViewModelBuilder().WithDasAccountId(dasAccountId).Build(),
                new AccountInformationViewModelBuilder().WithDasAccountId(dasAccountId).Build()
            };
            return expectedAccountInformation;
        }

        private List<AccountInformationViewModel> GenerateAccountList(int numberOfItems)
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
