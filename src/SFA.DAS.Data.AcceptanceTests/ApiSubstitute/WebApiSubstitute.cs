﻿using System;
using System.Diagnostics;
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
            Trace.WriteLine("Starting substitute service " + _baseAddress);
            var startOptions = new StartOptions(_baseAddress);
            var apiStartup = new ApiStartup();
            _webApp = WebApp.Start(startOptions, builder => apiStartup.Configuration(builder, _messageHandler));
            Trace.WriteLine("Substitute service started " + _baseAddress);
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
