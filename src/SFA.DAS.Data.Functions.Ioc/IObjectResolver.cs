using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Data.Functions.Ioc
{
    public interface IObjectResolver
    {
        object Resolve(Type type);
        T Resolve<T>();
    }
}
