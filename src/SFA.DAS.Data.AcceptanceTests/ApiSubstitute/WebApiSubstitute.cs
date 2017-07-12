using System;
using System.Diagnostics;
using Microsoft.Owin.Hosting;

namespace SFA.DAS.Data.AcceptanceTests.ApiSubstitute
{
    public class WebApiSubstitute : IDisposable
    {
        private readonly string[] _baseAddresses;
        private ApiSubstituteMessageHandler _messageHandler;
        private IDisposable _webApp;

        public WebApiSubstitute(params string[] baseAddresses)
        {
            _baseAddresses = baseAddresses;
            _messageHandler = new ApiSubstituteMessageHandler();
        }

        public void Start()
        {
            var startOptions = new StartOptions();
            foreach (var address in _baseAddresses)
            {
                startOptions.Urls.Add(address);
            }
            var apiStartup = new ApiStartup();
            _webApp = WebApp.Start(startOptions, builder => apiStartup.Configuration(builder, _messageHandler));
        }

        public void SetupGet(string apiPath, object returnValue)
        {
            foreach (var address in _baseAddresses)
            {
                var fullUrl = address + apiPath;
                _messageHandler.SetupGet(fullUrl, returnValue);
            }
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
