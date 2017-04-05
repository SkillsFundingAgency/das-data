using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Commands.CreatePaymentsForPeriodEnd;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Provider.Events.Api.Types;

namespace SFA.DAS.Data.Application.UnitTests.Commands.CreatePaymentsForPeriodEndCommandTests
{
    [TestFixture]
    public class WhenAPeriodEndIsProcessed
    {
        private CreatePaymentsForPeriodEndCommandHandler _handler;
        private CreatePaymentsForPeriodEndCommand _command;
        private Mock<IPaymentRepository> _paymentRepository;
        private Mock<IProviderEventService> _providerEventService;
        private Mock<ILog> _logger;

        [SetUp]
        public void Arrange()
        {
            _command = new CreatePaymentsForPeriodEndCommand { PeriodEndId = "ABC123" };

            _paymentRepository = new Mock<IPaymentRepository>();
            _providerEventService = new Mock<IProviderEventService>();
            _logger = new Mock<ILog>();
            _handler = new CreatePaymentsForPeriodEndCommandHandler(_paymentRepository.Object, _providerEventService.Object, _logger.Object);
        }

        [Test]
        public async Task AndThereIsASinglePageOfInformationThenThePaymentsAreCreated()
        {
            var payments = new PageOfResults<Payment>
            {
                PageNumber = 1,
                TotalNumberOfPages = 1,
                Items = new[] { new Payment(), new Payment(), new Payment() }
            };
            _providerEventService.Setup(x => x.GetPayments(_command.PeriodEndId, 1)).ReturnsAsync(payments);

            await _handler.Handle(_command);

            _paymentRepository.Verify(x => x.SavePayment(It.IsAny<Payment>()), Times.Exactly(3));

            foreach (var payment in payments.Items)
            {
                _paymentRepository.Verify(x => x.SavePayment(payment), Times.Once);
            }
        }

        [Test]
        public async Task AndThereAreMultiplePagesOfInformationThenThePaymentsAreCreated()
        {
            var paymentsPage1 = new PageOfResults<Payment>
            {
                PageNumber = 1,
                TotalNumberOfPages = 3,
                Items = new[] { new Payment(), new Payment(), new Payment() }
            };
            var paymentsPage2 = new PageOfResults<Payment>
            {
                PageNumber = 2,
                TotalNumberOfPages = 3,
                Items = new[] { new Payment(), new Payment() }
            };
            var paymentsPage3 = new PageOfResults<Payment>
            {
                PageNumber = 3,
                TotalNumberOfPages = 3,
                Items = new[] { new Payment(), new Payment(), new Payment() }
            };
            _providerEventService.Setup(x => x.GetPayments(_command.PeriodEndId, 1)).ReturnsAsync(paymentsPage1);
            _providerEventService.Setup(x => x.GetPayments(_command.PeriodEndId, 2)).ReturnsAsync(paymentsPage2);
            _providerEventService.Setup(x => x.GetPayments(_command.PeriodEndId, 3)).ReturnsAsync(paymentsPage3);

            await _handler.Handle(_command);

            _paymentRepository.Verify(x => x.SavePayment(It.IsAny<Payment>()), Times.Exactly(8));

            foreach (var payment in paymentsPage1.Items)
            {
                _paymentRepository.Verify(x => x.SavePayment(payment), Times.Once);
            }

            foreach (var payment in paymentsPage2.Items)
            {
                _paymentRepository.Verify(x => x.SavePayment(payment), Times.Once);
            }

            foreach (var payment in paymentsPage3.Items)
            {
                _paymentRepository.Verify(x => x.SavePayment(payment), Times.Once);
            }
        }

        [Test]
        public async Task AndGettingPaymentsFailsThenTheExceptionIsLogged()
        {
            var expectedException = new Exception();
            _providerEventService.Setup(x => x.GetPayments(_command.PeriodEndId, 1)).ReturnsAsync(new PageOfResults<Payment> { PageNumber = 1, TotalNumberOfPages = 3, Items = new Payment[0] });
            _providerEventService.Setup(x => x.GetPayments(_command.PeriodEndId, 2)).ThrowsAsync(expectedException);

            Assert.ThrowsAsync<Exception>(() => _handler.Handle(_command));
            
            _logger.Verify(x => x.Error(expectedException, $"Exception thrown getting period end {_command.PeriodEndId} page 2."));
        }

        [Test]
        public async Task AndSavingAPaymentFailsThenTheExceptionIsLogged()
        {
            var expectedException = new Exception();
            var failingPayment = new Payment { Id = "PayID" };
            var payments = new PageOfResults<Payment>
            {
                PageNumber = 1,
                TotalNumberOfPages = 1,
                Items = new[] { new Payment(), failingPayment, new Payment() }
            };
            _providerEventService.Setup(x => x.GetPayments(_command.PeriodEndId, 1)).ReturnsAsync(payments);
            _paymentRepository.Setup(x => x.SavePayment(failingPayment)).Throws(expectedException);

            Assert.ThrowsAsync<Exception>(() => _handler.Handle(_command));

            _logger.Verify(x => x.Error(expectedException, $"Exception thrown saving payment {failingPayment.Id} for period end {_command.PeriodEndId}"));
        }
    }
}
