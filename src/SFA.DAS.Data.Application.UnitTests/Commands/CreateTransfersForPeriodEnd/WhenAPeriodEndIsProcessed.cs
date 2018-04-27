using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Commands.CreateTransfersForPeriodEnd;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Provider.Events.Api.Types;

namespace SFA.DAS.Data.Application.UnitTests.Commands.CreateTransfersForPeriodEnd
{
    [TestFixture]
    public class WhenAPeriodEndIsProcessed
    {
        private CreateTransfersForPeriodEndCommandHandler _handler;
        private CreateTransfersForPeriodEndCommand _command;
        private Mock<ITransferRepository> _transferRepository;
        private Mock<IProviderEventService> _providerEventService;
        private Mock<ILog> _logger;

        [SetUp]
        public void Arrange()
        {
            _command = new CreateTransfersForPeriodEndCommand { PeriodEndId = "ABC123" };

            _transferRepository = new Mock<ITransferRepository>();
            _providerEventService = new Mock<IProviderEventService>();
            _logger = new Mock<ILog>();
            _handler = new CreateTransfersForPeriodEndCommandHandler(_transferRepository.Object, _providerEventService.Object, _logger.Object);
        }

        [Test]
        public async Task AndThereIsASinglePageOfInformationThenTheTransfersAreCreated()
        {
            var transfers = new PageOfResults<AccountTransfer>
            {
                PageNumber = 1,
                TotalNumberOfPages = 1,
                Items = new[] { new AccountTransfer(), new AccountTransfer(), new AccountTransfer() }
            };
            _providerEventService.Setup(x => x.GetTransfers(_command.PeriodEndId, 1)).ReturnsAsync(transfers);

            await _handler.Handle(_command);

            _transferRepository.Verify(x => x.SaveTransfers(transfers.Items), Times.Once());
        }

        [Test]
        public async Task AndThereAreMultiplePagesOfInformationThenTheTransfersAreCreated()
        {
            var transfersPage1 = new PageOfResults<AccountTransfer>
            {
                PageNumber = 1,
                TotalNumberOfPages = 3,
                Items = new[] { new AccountTransfer(), new AccountTransfer(), new AccountTransfer() }
            };
            var transfersPage2 = new PageOfResults<AccountTransfer>
            {
                PageNumber = 2,
                TotalNumberOfPages = 3,
                Items = new[] { new AccountTransfer(), new AccountTransfer() }
            };
            var transfersPage3 = new PageOfResults<AccountTransfer>
            {
                PageNumber = 3,
                TotalNumberOfPages = 3,
                Items = new[] { new AccountTransfer(), new AccountTransfer(), new AccountTransfer() }
            };
            _providerEventService.Setup(x => x.GetTransfers(_command.PeriodEndId, 1)).ReturnsAsync(transfersPage1);
            _providerEventService.Setup(x => x.GetTransfers(_command.PeriodEndId, 2)).ReturnsAsync(transfersPage2);
            _providerEventService.Setup(x => x.GetTransfers(_command.PeriodEndId, 3)).ReturnsAsync(transfersPage3);

            await _handler.Handle(_command);

            _transferRepository.Verify(x => x.SaveTransfers(It.IsAny<IEnumerable<AccountTransfer>>()), Times.Exactly(3));

            _transferRepository.Verify(x => x.SaveTransfers(transfersPage1.Items), Times.Once);
            _transferRepository.Verify(x => x.SaveTransfers(transfersPage2.Items), Times.Once);
            _transferRepository.Verify(x => x.SaveTransfers(transfersPage3.Items), Times.Once);
        }

        [Test]
        public void AndGettingTransfersFailsThenTheExceptionIsLogged()
        {
            var expectedException = new Exception();
            _providerEventService.Setup(x => x.GetTransfers(_command.PeriodEndId, 1)).ReturnsAsync(new PageOfResults<AccountTransfer> { PageNumber = 1, TotalNumberOfPages = 3, Items = new AccountTransfer[0] });
            _providerEventService.Setup(x => x.GetTransfers(_command.PeriodEndId, 2)).ThrowsAsync(expectedException);

            Assert.ThrowsAsync<Exception>(() => _handler.Handle(_command));
            
            _logger.Verify(x => x.Error(expectedException, $"Exception thrown getting period end {_command.PeriodEndId} page 2."));
        }

        [Test]
        public void AndSavingATransferFailsThenTheExceptionIsLogged()
        {
            var expectedException = new Exception();
            var failingTransfer = new AccountTransfer {TransferId = 1};
            var transfers = new PageOfResults<AccountTransfer>
            {
                PageNumber = 1,
                TotalNumberOfPages = 1,
                Items = new[] {new AccountTransfer(), failingTransfer, new AccountTransfer()}
            };
            _providerEventService.Setup(x => x.GetTransfers(_command.PeriodEndId, 1)).ReturnsAsync(transfers);
            _transferRepository.Setup(x => x.SaveTransfers(transfers.Items)).Throws(expectedException);

            Assert.ThrowsAsync<Exception>(() => _handler.Handle(_command));

            _logger.Verify(x => x.Error(expectedException, $"Exception thrown saving Transfers for period end {_command.PeriodEndId}"));
        }
    }
}
