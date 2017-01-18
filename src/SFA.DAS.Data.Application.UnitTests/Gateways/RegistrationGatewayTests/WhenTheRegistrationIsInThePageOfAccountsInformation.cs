using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace SFA.DAS.Data.Application.UnitTests.Gateways.RegistrationGatewayTests
{
    [TestFixture]
    public class WhenTheRegistrationIsInThePageOfAccountsInformation : RegistrationGatewayTestsBase
    {
        [Test]
        public async Task ThenTheAccountInformationIsReturned()
        {
            var dasAccountId = "2385";
            var accountInformationList = GenerateAccountList(8);
            var expectedAccountInformation = CreateExpectedAccountInformation(dasAccountId);
                
            accountInformationList.InsertRange(7, expectedAccountInformation);

            var accountInformationResponse = CreateGetPageOfAccountInformationResponse(accountInformationList, 1, 3);
            AccountApiClient.Setup(x => x.GetPageOfAccountInformation(DateTime.Now.AddYears(-1).Date, It.IsAny<DateTime>(), 1, 1000)).ReturnsAsync(accountInformationResponse);

            var result = await RegistrationGateway.GetRegistration(dasAccountId);

            result.ShouldAllBeEquivalentTo(expectedAccountInformation);
            AccountApiClient.Verify(x => x.GetPageOfAccountInformation(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Test]
        public async Task IfTheAccountInformationSpansMultiplePagesAndThereAreMorePagesThenTheAccountInformationIsReturned()
        {
            var dasAccountId = "2385";
            var expectedAccountInformation = CreateExpectedAccountInformation(dasAccountId);

            var accountInformationListPage1 = GenerateAccountList(9);
            accountInformationListPage1.Add(expectedAccountInformation.First());
            var accountInformationResponsePage1 = CreateGetPageOfAccountInformationResponse(accountInformationListPage1, 1, 3);
            AccountApiClient.Setup(x => x.GetPageOfAccountInformation(DateTime.Now.AddYears(-1).Date, It.IsAny<DateTime>(), 1, 1000)).ReturnsAsync(accountInformationResponsePage1);

            var accountInformationListPage2 = GenerateAccountList(9);
            accountInformationListPage2.Insert(0, expectedAccountInformation.Last());
            var accountInformationResponsePage2 = CreateGetPageOfAccountInformationResponse(accountInformationListPage2, 2, 3);
            AccountApiClient.Setup(x => x.GetPageOfAccountInformation(DateTime.Now.AddYears(-1).Date, It.IsAny<DateTime>(), 2, 1000)).ReturnsAsync(accountInformationResponsePage2);

            var result = await RegistrationGateway.GetRegistration(dasAccountId);

            result.ShouldAllBeEquivalentTo(expectedAccountInformation);
            AccountApiClient.Verify(x => x.GetPageOfAccountInformation(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(2));
        }

        [Test]
        public async Task IfTheAccountInformationSpansMultiplePagesAndThereAreNoMorePagesThenTheAccountInformationIsReturned()
        {
            var dasAccountId = "2385";
            var expectedAccountInformation = CreateExpectedAccountInformation(dasAccountId);

            var accountInformationListPage1 = GenerateAccountList(8);
            accountInformationListPage1.AddRange(expectedAccountInformation);
            var accountInformationResponsePage1 = CreateGetPageOfAccountInformationResponse(accountInformationListPage1, 1, 1);
            AccountApiClient.Setup(x => x.GetPageOfAccountInformation(DateTime.Now.AddYears(-1).Date, It.IsAny<DateTime>(), 1, 1000)).ReturnsAsync(accountInformationResponsePage1);

            var result = await RegistrationGateway.GetRegistration(dasAccountId);

            result.ShouldAllBeEquivalentTo(expectedAccountInformation);
            AccountApiClient.Verify(x => x.GetPageOfAccountInformation(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(1));
        }
    }
}
