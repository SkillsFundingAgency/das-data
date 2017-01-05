using System.Diagnostics;
using System.Net;
using System.Threading;
using Microsoft.Azure;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.ServiceRuntime;
using StructureMap;

namespace SFA.DAS.Data.Bus
{
    public class WorkerRole : RoleEntryPoint
    {
        private const string QueueName = "DataUpdatesQueue";

        private QueueClient _client;
        private readonly ManualResetEvent _completedEvent = new ManualResetEvent(false);
        private IContainer _container;

        public override void Run()
        {
            Trace.WriteLine("Starting processing of messages");

            _client.OnMessage((receivedMessage) =>
            {
                Trace.WriteLine("Processing Service Bus message: " + receivedMessage.SequenceNumber.ToString());
            });

            _completedEvent.WaitOne();
        }

        public override bool OnStart()
        {
            ServicePointManager.DefaultConnectionLimit = 12;

            _client = ConfigureServiceBusQueueClient();
            _container = ConfigureIocContainer();

            return base.OnStart();
        }

        private QueueClient ConfigureServiceBusQueueClient()
        {
            string connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
            if (!namespaceManager.QueueExists(QueueName))
            {
                namespaceManager.CreateQueue(QueueName);
            }

            return QueueClient.CreateFromConnectionString(connectionString, QueueName);
        }

        private IContainer ConfigureIocContainer()
        {
            var container = new Container(c =>
            {
            });
            return container;
        }

        public override void OnStop()
        {
            _client.Close();
            _completedEvent.Set();
            base.OnStop();
        }
    }
}
