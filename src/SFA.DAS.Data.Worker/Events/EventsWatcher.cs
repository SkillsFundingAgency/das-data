using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NLog;

namespace SFA.DAS.Data.Worker.Events
{
    public class EventsWatcher : IEventsWatcher
    {
        private readonly IEnumerable<IEventsProcessor> _processors;
        private readonly ILogger _logger;
        
        public EventsWatcher(IEnumerable<IEventsProcessor> processors, ILogger logger)
        {
            _processors = processors;
            _logger = logger;
        }

        public async Task ProcessEvents()
        {
            foreach (var processor in _processors)
            {
                try
                {
                    await processor.ProcessEvents();
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Error occurred whilst processing events");
                }
            }
        }
    }
}
