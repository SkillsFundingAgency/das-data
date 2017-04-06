using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Provider.Events.Api.Types;

namespace SFA.DAS.Data.Application.Commands.CreatePaymentsForPeriodEnd
{
    public class CreatePaymentsForPeriodEndCommandHandler : IAsyncNotificationHandler<CreatePaymentsForPeriodEndCommand>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IProviderEventService _providerEventService;
        private readonly ILog _logger;

        public CreatePaymentsForPeriodEndCommandHandler(IPaymentRepository paymentRepository, IProviderEventService providerEventService, ILog logger)
        {
            _paymentRepository = paymentRepository;
            _providerEventService = providerEventService;
            _logger = logger;
        }

        public async Task Handle(CreatePaymentsForPeriodEndCommand notification)
        {
            await ProcessPageOfPayments(notification.PeriodEndId, 1);
        }

        private async Task ProcessPageOfPayments(string periodEndId, int pageNumber)
        {
            var payments = await GetPayments(periodEndId, pageNumber);

            await SavePayments(payments, periodEndId);

            if (HasMorePagesToProcess(pageNumber, payments.TotalNumberOfPages))
            {
                await ProcessPageOfPayments(periodEndId, ++pageNumber);
            }
        }

        private async Task<PageOfResults<Payment>> GetPayments(string periodEndId, int pageNumber)
        {
            try
            {
                var payments = await _providerEventService.GetPayments(periodEndId, pageNumber);
                return payments;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Exception thrown getting period end {periodEndId} page {pageNumber}.");
                throw;
            }
        }

        private static bool HasMorePagesToProcess(int currentPageNumber, int totalNumberOfPages)
        {
            return totalNumberOfPages > currentPageNumber;
        }

        private async Task SavePayments(PageOfResults<Payment> payments, string periodEndId)
        {
            var tasks = payments.Items.Select(x => SavePayment(x, periodEndId));
            await Task.WhenAll(tasks);
        }

        private Task SavePayment(Payment payment, string periodEndId)
        {
            try
            {
                return _paymentRepository.SavePayment(payment);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Exception thrown saving payment {payment.Id} for period end {periodEndId}");
                throw;
            }
        }
    }
}
