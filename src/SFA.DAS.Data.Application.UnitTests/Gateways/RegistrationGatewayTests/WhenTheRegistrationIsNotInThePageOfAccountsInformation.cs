using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace SFA.DAS.Data.Application.UnitTests.Gateways.RegistrationGatewayTests
{
    [TestFixture]
    public class WhenTheRegistrationIsNotInThePageOfAccountsInformation : RegistrationGatewayTestsBase
    {
        [Test]
        public async Task ThenItIsRetrievedFromTheNextPage()
        {
            var dasAccountId = "2385";
            var expectedAccountInformation = CreateExpectedAccountInformation(dasAccountId);

            var accountInformationListPage1 = GenerateAccountList(10);
            var accountInformationResponsePage1 = CreateGetPageOfAccountInformationResponse(accountInformationListPage1, 1, 3);
            AccountApiClient.Setup(x => x.GetPageOfAccountInformation(DateTime.Now.AddYears(-1).Date, It.IsAny<DateTime>(), 1, 1000)).ReturnsAsync(accountInformationResponsePage1);

            var accountInformationListPage2 = GenerateAccountList(8);
            accountInformationListPage2.InsertRange(4, expectedAccountInformation);
            var accountInformationResponsePage2 = CreateGetPageOfAccountInformationResponse(accountInformationListPage2, 2, 3);
            AccountApiClient.Setup(x => x.GetPageOfAccountInformation(DateTime.Now.AddYears(-1).Date, It.IsAny<DateTime>(), 2, 1000)).ReturnsAsync(accountInformationResponsePage2);

            var result = await RegistrationGateway.GetRegistration(dasAccountId);

            result.ShouldAllBeEquivalentTo(expectedAccountInformation);
            AccountApiClient.Verify(x => x.GetPageOfAccountInformation(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(2));
        }

        [Test]
        public async Task IfItDoesNotExistThenNoAccountInformationIsReturned()
        {
            var dasAccountId = "2385";

            var accountInformationListPage1 = GenerateAccountList(10);
            var accountInformationResponsePage1 = CreateGetPageOfAccountInformationResponse(accountInformationListPage1, 1, 3);
            AccountApiClient.Setup(x => x.GetPageOfAccountInformation(DateTime.Now.AddYears(-1).Date, It.IsAny<DateTime>(), 1, 1000)).ReturnsAsync(accountInformationResponsePage1);

            var accountInformationListPage2 = GenerateAccountList(10);
            var accountInformationResponsePage2 = CreateGetPageOfAccountInformationResponse(accountInformationListPage2, 2, 3);
            AccountApiClient.Setup(x => x.GetPageOfAccountInformation(DateTime.Now.AddYears(-1).Date, It.IsAny<DateTime>(), 2, 1000)).ReturnsAsync(accountInformationResponsePage2);

            var accountInformationListPage3 = GenerateAccountList(10);
            var accountInformationResponsePage3 = CreateGetPageOfAccountInformationResponse(accountInformationListPage3, 3, 3);
            AccountApiClient.Setup(x => x.GetPageOfAccountInformation(DateTime.Now.AddYears(-1).Date, It.IsAny<DateTime>(), 3, 1000)).ReturnsAsync(accountInformationResponsePage3);

            var result = await RegistrationGateway.GetRegistration(dasAccountId);

            result.Count().Should().Be(0);
            AccountApiClient.Verify(x => x.GetPageOfAccountInformation(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(3));
        }
    }
}
