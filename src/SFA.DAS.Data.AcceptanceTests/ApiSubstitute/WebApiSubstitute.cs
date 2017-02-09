using System;
using Microsoft.Owin.Hosting;

namespace SFA.DAS.Data.AcceptanceTests.ApiSubstitute
{
    public class WebApiSubstitute : IDisposable
    {
        private readonly string _baseAddress;
        private ApiSubstituteMessageHandler _messageHandler;
        private IDisposable _webApp;

        public WebApiSubstitute(string baseAddress)
        {
            _baseAddress = baseAddress;
            _messageHandler = new ApiSubstituteMessageHandler();
        }

        public void Start()
        {
            var startOptions = new StartOptions(_baseAddress);
            var apiStartup = new ApiStartup();
            _webApp = WebApp.Start(startOptions, builder => apiStartup.Configuration(builder, _messageHandler));
        }

        public void SetupGet(string apiPath, object returnValue)
        {
            var fullUrl = _baseAddress + apiPath;
            _messageHandler.SetupGet(fullUrl, returnValue);
        }

        public void ClearSetup()
        {
            _messageHandler.ClearSetup();
        }

        public void Dispose()
        {
            _webApp.Dispose();
        }
    }
}
