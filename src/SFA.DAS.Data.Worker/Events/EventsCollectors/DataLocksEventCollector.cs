using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Provider.Events.Api.Types;

namespace SFA.DAS.Data.Worker.Events.EventsCollectors
{
    public class DataLockEventCollector : IEventsCollector<DataLockEvent>
    {
        private readonly IDataConfiguration _config;
        private readonly IProviderEventService _eventService;
        private readonly ILog _logger;
        private readonly bool _isEnabled;

        public DataLockEventCollector(IProviderEventService eventService, ILog logger, IDataConfiguration config)
        {
            _eventService = eventService;
            _logger = logger;
            _config = config;
            _isEnabled = _config.DataLocksEnabled;
        }

        public async Task<ICollection<DataLockEvent>> GetEvents()
        {
            _logger.Info("Getting unprocessed data locks");

            if (!_isEnabled)
            {
                return new List<DataLockEvent>();
            }

            var apiEvents = await _eventService.GetDataLocks(1);

            if (apiEvents?.Items == null)
            {
                return new List<DataLockEvent>();
            }

            _logger.Info($"{apiEvents?.Items?.Length} data locks retrieved from provider events service");

            return apiEvents.Items.Select(p => p).ToList();
        }
    }
}
