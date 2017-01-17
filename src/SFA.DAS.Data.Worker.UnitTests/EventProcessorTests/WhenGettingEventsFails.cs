using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;

namespace SFA.DAS.Data.Worker.UnitTests.EventProcessorTests
{
    [TestFixture]
    public class WhenGettingEventsFails : EventProcessorTests
    {
        [Test]
        public async Task ThenTheExceptionIsLogged()
        {
            var expectedException = new Exception();
            EventsApi.Setup(x => x.GetAccountEventsById(CurrentEventId + 1, 1000, 1)).ThrowsAsync(expectedException);

            await EventProcessor.ProcessEvents();

            Logger.Verify(x => x.Error(expectedException, "Unexcepted exception when processing events."));
        }
    }
}
