using System;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Provider.Events.Api.Types;

namespace SFA.DAS.Data.Application.Commands.CreateDataLock
{
    public class CreateDataLockCommandHandler : IAsyncNotificationHandler<CreateDataLockCommand>
    {
        private readonly IDataLockRepository _dataLockRepository;
        private readonly IProviderEventService _providerEventService;
        private readonly ILog _logger;

        public CreateDataLockCommandHandler(IDataLockRepository dataLockRepository,
            IProviderEventService providerEventService, ILog logger)
        {
            _dataLockRepository = dataLockRepository;
            _providerEventService = providerEventService;
            _logger = logger;
        }

        public async Task Handle(CreateDataLockCommand notification)
        {
            await ProcessPageOfDataLocks(1);
        }

        private async Task ProcessPageOfDataLocks(int pageNumber)
        {
            while (true)
            {
                var dataLocks = await GetDataLocks(pageNumber);

                if (dataLocks.Items != null && dataLocks.Items.Length > 0)
                    await SavePayments(dataLocks);

                if (HasMorePagesToProcess(pageNumber, dataLocks.TotalNumberOfPages))
                {
                    pageNumber = ++pageNumber;
                    continue;
                }

                break;
            }
        }

        private async Task<PageOfResults<DataLockEvent>> GetDataLocks(int pageNumber)
        {
            try
            {
                var dataLocks = await _providerEventService.GetDataLocks(pageNumber);
                return dataLocks;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Exception thrown getting data locks page {pageNumber}.");
                throw;
            }
        }

        private static bool HasMorePagesToProcess(int currentPageNumber, int totalNumberOfPages)
        {
            return totalNumberOfPages > currentPageNumber;
        }

        private async Task SavePayments(PageOfResults<DataLockEvent> dataLocks)
        {
            try
            {
                await _dataLockRepository.SaveDataLocks(dataLocks.Items);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Exception thrown saving data locks");
                throw;
            }
        }
    }
}