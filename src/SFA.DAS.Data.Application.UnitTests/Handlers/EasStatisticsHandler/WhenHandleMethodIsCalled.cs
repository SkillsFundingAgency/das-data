using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Domain;
using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Application.UnitTests.Handlers.EasStatisticsHandler
{
    [TestFixture]
    public class WhenHandleMethodIsCalled : StatisticsHandlerBase<EasStatisticsModel>
    {
        private Application.Handlers.EasStatisticsHandler _handler;
        
        [SetUp]
        public void Setup()
        {
            base.SetUp(o => o.EasStatisticsEndPoint);

            _handler = new Application.Handlers.EasStatisticsHandler(HttpClientWrapper.Object, Configuration.Object, Logger.Object);
        }

        protected override async Task<dynamic> CallHandleMethodOnHandler()
        {
            return await _handler.Handle();
        }
    }
}
