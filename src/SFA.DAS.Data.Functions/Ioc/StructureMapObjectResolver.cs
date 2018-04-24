using System;
using System.Reflection;
using StructureMap;
using StructureMap.TypeRules;

namespace SFA.DAS.Data.Functions.Ioc
{
    public class StructureMapObjectResolver : IObjectResolver
    {
        private static IContainer _container;
        private static readonly object LockObject = new object();

        public Registry DefaultRegistryType { get; set; }

        public StructureMapObjectResolver(Registry T)
        {
            DefaultRegistryType = T;
        }

        protected IContainer Container
        {
            get
            {
                lock (LockObject)
                {
                    return _container ?? (_container = new Container(c =>
                    {
                        c.AddRegistry<DefaultRegistry>();
                        c.AddRegistry(DefaultRegistryType);
                    }));
                }
            }
        }

       

        public object Resolve(Type type)
        {
            return Container.GetInstance(type);
        }

        public T Resolve<T>()
        {
            return Container.GetInstance<T>();
        }
    }
}
