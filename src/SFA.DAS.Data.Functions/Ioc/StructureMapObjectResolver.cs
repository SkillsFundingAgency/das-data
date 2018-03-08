using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureMap;

namespace SFA.DAS.Data.Functions.Ioc
{
    public class StructureMapObjectResolver : IObjectResolver
    {
        private static IContainer _container;
        private static readonly object LockObject = new object();

        private static IContainer Container
        {
            get
            {
                lock (LockObject)
                {
                    return _container ?? (_container = new Container(c =>
                    {
                        //c.For<ITestClass>().Use<TestClass>();
                        c.AddRegistry<DefaultRegistry>();
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
