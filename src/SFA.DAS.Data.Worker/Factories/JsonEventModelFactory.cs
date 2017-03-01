using System;
using Newtonsoft.Json;
using NLog;

namespace SFA.DAS.Data.Worker.Factories
{
    public class JsonEventModelFactory : IEventModelFactory
    {
        private readonly ILogger _logger;

        public JsonEventModelFactory(ILogger logger)
        {
            _logger = logger;
        }

        public T Create<T>(string data)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(data);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Could not create model of type");
                return default(T);
            }
        }
    }
}
