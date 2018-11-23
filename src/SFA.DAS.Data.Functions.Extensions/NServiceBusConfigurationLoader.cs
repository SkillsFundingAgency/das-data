using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using Microsoft.Azure.WebJobs;
using StructureMap.TypeRules;

namespace SFA.DAS.Data.Functions.Extensions
{
    public class NServiceBusConfigurationLoader
    {
        public async Task LoadNServiceBusConfiguration()
        {
            //Load assemblies
            var assemblies = GetUserAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = this.FindTypes(assembly);
                if (types != null)
                {
                    foreach (var type in types.Where<Type>(new Func<Type, bool>(IsJobClass)))
                    {
                        //Look for the attribute on methods
                        foreach (var method in type.GetMethods())
                        {
                            foreach (var attribute in method.GetCustomAttributes().OfType<NServiceBusConfigurationAttribute>())
                            {
                                //TODO: Move into method
                                ServiceBusTriggerAttribute triggerAttribute = null;

                                //Look for the servicebus trigger attribute on the method - might get more details from there
                                foreach (var param in method.GetParameters())
                                {
                                    triggerAttribute = param.GetCustomAttributes()
                                        .OfType<ServiceBusTriggerAttribute>().FirstOrDefault();
                                    if (triggerAttribute != null)
                                    {
                                        break;
                                    }
                                }

                                //var triggerAttribute = method
                                //    .GetCustomAttributes()
                                //    .OfType<ServiceBusTriggerAttribute>()
                                //    .SingleOrDefault();

                                await CreateConfiguration(attribute, triggerAttribute);
                            }
                        }
                    }
                }
            }
        }

        private async Task CreateConfiguration(NServiceBusConfigurationAttribute attribute, ServiceBusTriggerAttribute triggerAttribute)
        {
            try
            {
                //TODO: Decide naming convention
                // e.g. use name of this project as prefix - RDS_ or das-data- for subs and queues.

                var enablePartitioning = false;
                var maxSizeInMB = 5120;

                //TODO: Use one or the other, not both
                var connectionStringName = triggerAttribute?.Connection ?? attribute.Connection;
                var queueName = triggerAttribute?.QueueName ?? attribute.Queue;
                var subscriptionName = attribute.Subscription;
                var messageType = attribute.MessageType;
                var eventName = messageType.GetFullName();

                var connectionString = Environment.GetEnvironmentVariable(connectionStringName, EnvironmentVariableTarget.Process);

                //var tokenProvider = GetTokenProvider(attribute.Connection);
                var managementClient = new ManagementClient(connectionString);

                var queues = await managementClient.GetQueuesAsync();

                //var queue = await managementClient.GetQueueAsync(queueName);
                var queue = queues.SingleOrDefault(q => q.Path == queueName);

                if (queue == null)
                {
                    var queueDescription = new QueueDescription(queueName)
                    {
                        //TODO: Confirm settings
                        EnableBatchedOperations = true,
                        LockDuration = TimeSpan.FromMinutes(5),
                        MaxDeliveryCount = int.MaxValue,
                        MaxSizeInMB = maxSizeInMB,
                        EnablePartitioning = enablePartitioning
                    };
                    await managementClient.CreateQueueAsync(queueDescription).ConfigureAwait(false);
                }

                var bundleTopicPath = "bundle-1";
                
                var topics = await managementClient.GetTopicsAsync();
                var bundleTopic = topics.SingleOrDefault(b => b.Path == bundleTopicPath);

                if (bundleTopic == null)
                {
                    var topicDescription = new TopicDescription(bundleTopicPath)
                    {
                        //TODO: Confirm settings
                        EnableBatchedOperations = true,
                        EnablePartitioning = enablePartitioning,
                        MaxSizeInMB = maxSizeInMB
                    };

                    try
                    {
                        await managementClient.CreateTopicAsync(topicDescription)
                            .ConfigureAwait(false);
                    }
                    catch (MessagingEntityAlreadyExistsException)
                    {
                        //TODO: Decide if we use this try..catch pattern (as NServiceBus does)
                        //      or get a list of items to check existence before creating.
                    }
                }

                var subscriptions = await managementClient.GetSubscriptionsAsync(bundleTopicPath);
                var subscription = subscriptions.SingleOrDefault(s => s.TopicPath == bundleTopicPath && s.SubscriptionName == subscriptionName);

                if (subscription == null)
                {
                    var subscriptionDescription = new SubscriptionDescription(bundleTopicPath, subscriptionName)
                    {
                        ForwardTo = queueName,
                        LockDuration = TimeSpan.FromMinutes(5),
                        EnableDeadLetteringOnFilterEvaluationExceptions = false,
                        MaxDeliveryCount = int.MaxValue,
                        EnableBatchedOperations = true,
                        //UserMetadata = mainInputQueueName,
                        UserMetadata = $"Event {eventName} subscription"
                    };
                    subscription = await managementClient.CreateSubscriptionAsync(subscriptionDescription);

                    await managementClient.DeleteRuleAsync(bundleTopicPath, subscription.SubscriptionName, "$Default");
                    
                    var ruleDescription = new RuleDescription
                    {
                        Name = eventName,
                        Filter = new SqlFilter($"[NServiceBus.EnclosedMessageTypes] LIKE '%{eventName}%'")
                    };
                    await managementClient.CreateRuleAsync("bundle-1", subscription.SubscriptionName, ruleDescription);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static IEnumerable<Assembly> GetUserAssemblies()
        {
            return (IEnumerable<Assembly>)AppDomain.CurrentDomain.GetAssemblies();
        }

        public static bool IsJobClass(Type type)
        {
            if (type == (Type)null || !type.IsClass || type.IsAbstract && !type.IsSealed || !type.IsPublic)
                return false;
            return !type.ContainsGenericParameters;
        }

        private Type[] FindTypes(Assembly assembly)
        {
            //if (!CustomTypeLocator.AssemblyReferencesSdkOrExtension(assembly, extensionAssemblies))
            //    return (Type[])null;
            Type[] typeArray = (Type[])null;
            try
            {
                typeArray = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                //this._log.WriteLine("Warning: Only got partial types from assembly: {0}", (object)assembly.FullName);
                //this._log.WriteLine("Exception message: {0}", (object)ex.ToString());
                typeArray = ex.Types;
            }
            catch (Exception ex)
            {
                //this._log.WriteLine("Warning: Failed to get types from assembly: {0}", (object)assembly.FullName);
                //this._log.WriteLine("Exception message: {0}", (object)ex.ToString());
            }
            return typeArray;
        }
    }
}
