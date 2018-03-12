using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using Microsoft.Azure.WebJobs;
using NUnit.Framework;
using SFA.DAS.Data.Functions.AcceptanceTests.Infrastructure;
using StructureMap;

namespace SFA.DAS.Data.Functions.AcceptanceTests
{
    public abstract class FunctionEventTestBase
    {
        protected static IContainer ParentContainer { get; set; }

        protected static Config Config => ParentContainer.GetInstance<Config>();

        protected static List<Process> Processes = new List<Process>();

        protected static readonly string FunctionsCliPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Azure.Functions.Cli", "1.0.9", "func.exe");

        private static string GetAppPath(string appName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var codebase = new Uri(assembly.CodeBase);
            var path = codebase.LocalPath;
            return Path.GetFullPath(Path.Combine(path, $@"..\..\..\..\{appName}\bin\Debug\net462"));
        }

        protected static void StartFunction(string functionName)
        {
            if (!Config.IsDevEnvironment)
            {
                Console.WriteLine("Can only start the function in dev environment.");
                return;
            }

            Console.WriteLine($"Starting the function cli. Path: {FunctionsCliPath}");
            var appPath = GetAppPath(functionName);
            Console.WriteLine($"Function path: {appPath}");
            if (!Directory.Exists(appPath))
            {
                throw new Exception($"Function path: {appPath} path does not exist");
            }

            var process = new Process
            {
                StartInfo =
                {
                    FileName = FunctionsCliPath,
                    Arguments = $"host start",
                    WorkingDirectory = appPath,
                    //UseShellExecute = true,
                }
            };
            process.Start();
            Processes.Add(process);
            Console.WriteLine("Giving the function time to start.");
            Thread.Sleep(TimeSpan.FromSeconds(5));
        }

        [OneTimeSetUp]
        public void ClassSetup()
        {
            //ParentContainer = new Container(c =>
            //{
            //    c.AddRegistry<DefaultRegistry>();
            //    //c.AddRegistry<Functions.Ioc.DefaultRegistry>();
            //});

            var config = new JobHostConfiguration();
            config.UseTimers();
            config.Tracing.ConsoleLevel = TraceLevel.Verbose;
            config.UseDependencyInjection();
            var host = new JobHost(config);
            host.RunAndBlock();
        }

        [SetUp]
        public void Setup()
        {
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            Processes?.ForEach(process =>
            {
                //process.WaitForExit()
                var processName = process.ProcessName;
                process.Kill();
                //foreach (var process1 in Process.GetProcessesByName(processName))
                //{
                //    process1.Kill();
                //}
            });
            Processes?.Clear();

            Thread.Sleep(500);
            ParentContainer.Dispose();
        }
    }

}
