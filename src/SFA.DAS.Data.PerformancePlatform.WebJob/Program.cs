using SFA.DAS.Data.PerformancePlatform.WebJob.DependencyResolution;
using StructureMap;

namespace SFA.DAS.Data.PerformancePlatform.WebJob
{
    public class Program
    {
        static void Main()
        {
            var container = ConfigureIocContainer();
            var paymentUpdater = container.GetInstance<IPerformancePlatformProcessor>();
            paymentUpdater.ExportData().Wait();
        }

        private static IContainer ConfigureIocContainer()
        {
            var container = new Container(c =>
            {
                c.AddRegistry<DefaultRegistry>();
            });
            return container;
        }
    }
}
