//using System;
//using Microsoft.Azure.WebJobs;
////using System.ComponentModel;
////using System.Threading;
//using System.Threading.Tasks;
//using Microsoft.Azure.WebJobs.Host;
//using SFA.DAS.Data.Functions.Framework.Infrastructure;
//using SFA.DAS.Data.Functions.Framework.Logging;
//using SFA.DAS.NLog.Logger;
//using StructureMap;

//namespace SFA.DAS.Data.Functions.Framework
//{
//    public class FunctionRunner
//    {
//        public static async Task Run<TFunction>(TraceWriter writer, ExecutionContext executionContext, Func<IContainer, ILog, Task> runAction) where TFunction : IFunction
//        {
//            ILog logger = null;
//            try
//            {
//                var container = ContainerBootstrapper.Bootstrap();
//                using (var nestedContainer = container.GetNestedContainer())
//                {
//                    ConfigureContainer(executionContext, writer, container);
//                    logger = container.GetInstance<ILog>();
//                    await runAction(nestedContainer, logger);
//                }
//            }
//            catch (Exception ex)
//            {
//                if (logger != null)
//                    logger.Error(ex, $"Error invoking function: {typeof(TFunction)}.");
//                else
//                    writer.Error($"Error invoking function: {typeof(TFunction)}.", ex: ex);

//                throw;
//            }
//        }

//        public static void Run<TFunction>(TraceWriter writer, ExecutionContext executionContext, Action<IContainer, ILog> runAction) where TFunction : IFunction
//        {
//            ILog logger = null;
//            try
//            {
//                var container = ContainerBootstrapper.Bootstrap();
//                using (var nestedContainer = container.GetNestedContainer())
//                {
//                    ConfigureContainer(executionContext, writer, container);
//                    logger = container.GetInstance<ILog>();
//                    runAction(nestedContainer, logger);
//                }
//            }
//            catch (Exception ex)
//            {
//                if (logger != null)
//                    logger.Error(ex, $"Error invoking function: {typeof(TFunction)}.");
//                else
//                    writer.Error($"Error invoking function: {typeof(TFunction)}.", ex: ex);
//                throw;
//            }
//        }

//        public static TReturn Run<TFunction, TReturn>(TraceWriter writer, ExecutionContext executionContext, Func<IContainer, ILog, TReturn> runAction) where TFunction : IFunction
//        {
//            try
//            {
//                var container = ContainerBootstrapper.Bootstrap();
//                using (var nestedContainer = container.GetNestedContainer())
//                {
//                    ConfigureContainer(executionContext, writer, container);
//                    return runAction(nestedContainer, container.GetInstance<ILog>());
//                }
//            }
//            catch (Exception ex)
//            {
//                writer.Error($"Error invoking function: {typeof(TFunction)}.", ex: ex);
//                throw;
//            }
//        }

//        public static async Task<TReturn> Run<TFunction, TReturn>(TraceWriter writer, ExecutionContext executionContext, Func<IContainer, ILog, Task<TReturn>> runAction) where TFunction : IFunction
//        {
//            ILog logger = null;
//            try
//            {
//                var container = ContainerBootstrapper.Bootstrap();
//                using (var nestedContainer = container.GetNestedContainer())
//                {
//                    ConfigureContainer(executionContext, writer, container);
//                    logger = container.GetInstance<ILog>();
//                    return await runAction(nestedContainer, container.GetInstance<ILog>());
//                }
//            }
//            catch (Exception ex)
//            {
//                if (logger != null)
//                    logger.Error(ex, $"Error invoking function: {typeof(TFunction)}.");
//                else
//                    writer.Error($"Error invoking function: {typeof(TFunction)}.", ex: ex);
//                throw;
//            }
//        }

//        private static void ConfigureContainer(ExecutionContext executionContext, TraceWriter writer, IContainer container)
//        {
//            container.Configure(c =>
//            {
//                c.For<ILog>().Use(x => LoggerSetup.Create(executionContext.FunctionAppDirectory, writer, x.ParentType));
//            });
//        }
//    }
//}
