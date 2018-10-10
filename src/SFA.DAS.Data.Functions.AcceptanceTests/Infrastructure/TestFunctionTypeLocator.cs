using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace SFA.DAS.Data.Functions.AcceptanceTests.Infrastructure
{
    //Based on Microsoft.Azure.WebJobs.Host.Indexers.DefaultTypeLocator
    internal class TestFunctionTypeLocator : ITypeLocator
    {
        private static readonly string WebJobsAssemblyName = AssemblyNameCache.GetName(typeof(TableAttribute).Assembly).Name;

        public IReadOnlyList<Type> GetTypes()
        {
            List<Type> typeList = new List<Type>();
            IEnumerable<Assembly> userAssemblies = GetUserAssemblies();

            //IEnumerable<Assembly> extensionAssemblies = this._extensions.GetExtensionAssemblies();
            IEnumerable<Assembly> extensionAssemblies = new List<Assembly>();

            foreach (Assembly assembly in userAssemblies)
            {
                Type[] types = this.FindTypes(assembly, extensionAssemblies);
                if (types != null)
                {
                    foreach (var type in types.Where<Type>(new Func<Type, bool>(TestFunctionTypeLocator.IsJobClass)))
                    {
                        if (!IsHttpTriggeredFunction(type))
                        {
                            typeList.Add(type);
                        }
                    }
                }
            }
            return (IReadOnlyList<Type>)typeList;
        }

        private static bool IsHttpTriggeredFunction(Type type)
        {
            return type.GetMethods()
                .Any(m => m.GetParameters()
                    .Any(p => p.GetCustomAttributes()
                        .Any(a => a.GetType().Name == "HttpTriggerAttribute")));
        }

        private static bool AssemblyReferencesSdkOrExtension(Assembly assembly, IEnumerable<Assembly> extensionAssemblies)
        {
            if (typeof(TestFunctionTypeLocator).Assembly == assembly)
                return false;
            foreach (AssemblyName referencedAssembly in assembly.GetReferencedAssemblies())
            {
                AssemblyName referencedAssemblyName = referencedAssembly;
                if (string.Equals(referencedAssemblyName.Name, TestFunctionTypeLocator.WebJobsAssemblyName, StringComparison.OrdinalIgnoreCase) || extensionAssemblies.Any<Assembly>((Func<Assembly, bool>)(p => string.Equals(referencedAssemblyName.Name, AssemblyNameCache.GetName(p).Name, StringComparison.OrdinalIgnoreCase))))
                    return true;
            }
            return false;
        }

        public static bool IsJobClass(Type type)
        {
            if (type == (Type)null || !type.IsClass || type.IsAbstract && !type.IsSealed || !type.IsPublic)
                return false;
            return !type.ContainsGenericParameters;
        }
        private static IEnumerable<Assembly> GetUserAssemblies()
        {
            return (IEnumerable<Assembly>)AppDomain.CurrentDomain.GetAssemblies();
        }

        private Type[] FindTypes(Assembly assembly, IEnumerable<Assembly> extensionAssemblies)
        {
            if (!TestFunctionTypeLocator.AssemblyReferencesSdkOrExtension(assembly, extensionAssemblies))
                return (Type[])null;

            Type[] typeArray = (Type[])null;
            try
            {
                typeArray = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                //this._log.WriteLine("Warning: Only got partial types from assembly: {0}", (object)assembly.FullName);
                typeArray = ex.Types;
            }
            catch (Exception ex)
            {
                throw new Exception($"Warning: Failed to get types from assembly: {assembly.FullName}", ex);
            }
            return typeArray;
        }
    }
}
