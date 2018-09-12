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
            if (notification.Event != null)
            {
                await SaveDataLock(notification.Event);
            }
        }

        private async Task SaveDataLock(DataLockEvent dataLock)
        {
            try
            {
                await _dataLockRepository.SaveDataLock(dataLock);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Exception thrown saving data lock");
                throw;
            }
        }
    }
}