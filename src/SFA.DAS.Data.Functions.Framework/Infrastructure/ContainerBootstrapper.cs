//using System.IO;
//using System.Reflection;
//using StructureMap;

//namespace SFA.DAS.Data.Functions.Framework.Infrastructure
//{
//    public class ContainerBootstrapper : Registry
//    {
//        private static readonly object LockObject = new object();
//        private static IContainer _container;
//        public static IContainer Bootstrap()
//        {
//            lock (LockObject)
//            {
//                return _container ?? (_container = new Container(c =>
//                {
//                    //c.AddRegistry<ConfigurationRegistry>();
//                    c.AddRegistry<DefaultRegistry>();
//                    //c.AddRegistry<MediatrRegistry>();
//                    var binPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
//                    c.Scan(assScanner =>
//                    {
//                        //assScanner.LookForRegistries();
//                        assScanner.TheCallingAssembly();
//                        assScanner.AssembliesFromPath(binPath, a => a.GetName().Name.StartsWith("SFA.DAS.Data"));
//                        assScanner.RegisterConcreteTypesAgainstTheFirstInterface();
//                    });
//                }));
//            }
//        }
//    }
//}
