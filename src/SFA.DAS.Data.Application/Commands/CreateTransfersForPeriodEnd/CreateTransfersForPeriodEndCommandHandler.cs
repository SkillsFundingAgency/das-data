using System;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Provider.Events.Api.Types;

namespace SFA.DAS.Data.Application.Commands.CreateTransfersForPeriodEnd
{
    public class CreateTransfersForPeriodEndCommandHandler : IAsyncNotificationHandler<CreateTransfersForPeriodEndCommand>
    {
        private readonly ITransferRepository _transferRepository;
        private readonly IProviderEventService _providerEventService;
        private readonly ILog _logger;

        public CreateTransfersForPeriodEndCommandHandler(ITransferRepository transferRepository, IProviderEventService providerEventService, ILog logger)
        {
            _transferRepository = transferRepository;
            _providerEventService = providerEventService;
            _logger = logger;
        }

        public async Task Handle(CreateTransfersForPeriodEndCommand notification)
        {
            await ProcessPageOfTransfers(notification.PeriodEndId, 1);
        }

        private async Task ProcessPageOfTransfers(string periodEndId, int pageNumber)
        {
            while (true)
            {
                var transfers = await GetTransfers(periodEndId, pageNumber);

                if (transfers.Items != null && transfers.Items.Length > 0)
                    await SaveTransfers(transfers, periodEndId);

                if (HasMorePagesToProcess(pageNumber, transfers.TotalNumberOfPages))
                {
                    pageNumber = ++pageNumber;
                    continue;
                }

                break;
            }
        }

        private async Task<PageOfResults<AccountTransfer>> GetTransfers(string periodEndId, int pageNumber)
        {
            try
            {
                return await _providerEventService.GetTransfers(periodEndId, pageNumber);
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

        private async Task SaveTransfers(PageOfResults<AccountTransfer> transfers, string periodEndId)
        {
            try
            {
                await _transferRepository.SaveTransfers(transfers.Items);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Exception thrown saving Transfers for period end {periodEndId}");
                throw;
            }
        }
    }
}
