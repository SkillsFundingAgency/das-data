using System;
using Newtonsoft.Json;
using NLog;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Worker.Factories
{
    public class JsonEventModelFactory : IEventModelFactory
    {
        private readonly ILog _logger;

        public JsonEventModelFactory(ILog logger)
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
