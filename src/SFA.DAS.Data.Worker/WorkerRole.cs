using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.ServiceRuntime;
using SFA.DAS.Data.Worker.DependencyResolution;
using StructureMap;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Infrastructure.DependencyResolution;
using SFA.DAS.Data.Infrastructure.DependencyResolution.Policies;
using SFA.DAS.Data.Worker.Events;
using SFA.DAS.Messaging.Interfaces;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Worker
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly CancellationTokenSource _cancellationTokenSourceMessageProcessor = new CancellationTokenSource();
        private readonly ManualResetEvent _runCompleteEvent = new ManualResetEvent(false);
        private IContainer _container;
        private IEventsWatcher _eventsWatcher;

        public override void Run()
        {
            //Trace.TraceInformation("SFA.DAS.Data.Worker is running");

            try
            {
                ThreadStart job = RunMessageProcessors;
                var thread = new Thread(job);
                thread.Start();

                this.RunAsync(this._cancellationTokenSource.Token).Wait();
            }
            finally
            {
                this._runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            ServicePointManager.DefaultConnectionLimit = 12;
            TelemetryConfiguration.Active.InstrumentationKey = CloudConfigurationManager.GetSetting("InstrumentationKey", false);
            _container = ConfigureIocContainer();
            _eventsWatcher = _container.GetInstance<IEventsWatcher>();

            var result = base.OnStart();

            //Trace.TraceInformation("SFA.DAS.Data.Worker has been started");

            return result;
        }

        public override void OnStop()
        {
            //Trace.TraceInformation("SFA.DAS.Data.Worker is stopping");

            this._cancellationTokenSource.Cancel();
            this._cancellationTokenSourceMessageProcessor.Cancel();
            this._runCompleteEvent.WaitOne();

            base.OnStop();

            //Trace.TraceInformation("SFA.DAS.Data.Worker has stopped");
        }
 
        private async Task RunAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await _eventsWatcher.ProcessEvents();
                await Task.Delay(60000, cancellationToken);
            }
        }

        private IContainer ConfigureIocContainer()
        {
            var container = new Container(c =>
            {
                c.Policies.Add(new ConfigurationPolicy<ServiceBusConfiguration>("SFA.DAS.Data"));
                c.Policies.Add(new MessageSubscriberPolicy<ServiceBusConfiguration>("SFA.DAS.Data"));
                c.AddRegistry<DefaultRegistry>();
            });
            return container;
        }

        private void RunMessageProcessors()
        {
            var logger = _container.GetInstance<ILog>();

            try
            {
                logger.Debug("Getting message processors");

                var messageProcessors = _container.GetAllInstances<IMessageProcessor>().ToList();

                logger.Debug($"Found {messageProcessors.Count} message processors");

                foreach (var processor in messageProcessors)
                {
                    logger.Debug($"Found message processor of type {processor.GetType().FullName}");
                }

                var tasks = messageProcessors.Select(x => x.RunAsync(_cancellationTokenSourceMessageProcessor)).ToArray();
                Task.WaitAll(tasks);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Task Worker has had an unhandled excdeption and is exiting");
                throw;
            }
            finally
            {
                _runCompleteEvent.Set();
            }
        }
    }
}
