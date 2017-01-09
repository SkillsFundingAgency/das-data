using System.Net;
using System.Threading;
using MediatR;
using Microsoft.Azure;
using Microsoft.WindowsAzure.ServiceRuntime;
using SFA.DAS.Data.Application.Commands.CreateRegistration;
using StructureMap;

namespace SFA.DAS.Data.Bus
{
    public class WorkerRole : RoleEntryPoint
    {
        private AzureServiceBus _bus;
        private readonly ManualResetEvent _completedEvent = new ManualResetEvent(false);
        private IContainer _container;
        private IMediator _mediator;

        public override void Run()
        {
            _bus.OnMessage<string>(receivedMessage =>
            {
                var dasAccountId = receivedMessage;
                _mediator.SendAsync(new CreateRegistrationCommand {DasAccountId = dasAccountId});
            }, _completedEvent);

            _completedEvent.WaitOne();
        }

        public override bool OnStart()
        {
            ServicePointManager.DefaultConnectionLimit = 12;

            _bus = ConfigureServiceBus();
            _container = ConfigureIocContainer();
            _mediator = _container.GetInstance<IMediator>();

            return base.OnStart();
        }

        public override void OnStop()
        {
            _completedEvent.Set();
            base.OnStop();
        }

        private AzureServiceBus ConfigureServiceBus()
        {
            string connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
            return new AzureServiceBus(connectionString);
        }

        private IContainer ConfigureIocContainer()
        {
            var container = new Container(c =>
            {
            });
            return container;
        }
    }
}
