using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.ServiceRuntime;
using SFA.DAS.Data.Worker.DependencyResolution;
using StructureMap;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure;
using SFA.DAS.Data.Worker.Events;

namespace SFA.DAS.Data.Worker
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent _runCompleteEvent = new ManualResetEvent(false);
        private IContainer _container;
        private IEventsWatcher _eventsWatcher;

        public override void Run()
        {
            //Trace.TraceInformation("SFA.DAS.Data.Worker is running");

            try
            {
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
                c.AddRegistry<DefaultRegistry>();
            });
            return container;
        }
    }
}
