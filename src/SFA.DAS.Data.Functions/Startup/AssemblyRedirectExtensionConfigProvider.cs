using System;
using System.Linq;
using System.Reflection;
using Microsoft.Azure.WebJobs.Host.Config;
using SFA.DAS.Data.Functions.Extensions;

namespace SFA.DAS.Data.Functions.NServiceBusExtensions
{
    public class AssemblyRedirectExtensionConfigProvider : IExtensionConfigProvider
    {
        public void Initialize(ExtensionConfigContext context)
        {
            Console.WriteLine("Initializing AssemblyRedirectExtensionConfigProvider");

            //Workaround for redirecting Newtonsoft assembly
            //RedirectAssembly();
        }

        //https://stackoverflow.com/questions/38093972/azure-functions-binding-redirect
        //https://codopia.wordpress.com/2017/07/21/how-to-fix-the-assembly-binding-redirect-problem-in-azure-functions/

        //https://stackoverflow.com/questions/51304256/azure-functions-newtonsoft-json-load-error

        private void RedirectAssembly()
        {
            var list = AppDomain.CurrentDomain.GetAssemblies()
                .Select(a => a.GetName())
                .OrderByDescending(a => a.Name)
                .ThenByDescending(a => a.Version)
                .Select(a => a.FullName)
                .ToList();
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                var requestedAssembly = new AssemblyName(args.Name);
                foreach (string asmName in list)
                {
                    if (asmName.StartsWith(requestedAssembly.Name + ","))
                    {
                        return Assembly.Load(asmName);
                    }
                }

                return null;
            };
        }
    }
}
