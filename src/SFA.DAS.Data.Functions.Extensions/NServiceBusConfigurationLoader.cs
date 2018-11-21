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
            //var assembly = typeof();
            var assemblies = GetUserAssemblies();
            foreach (var assembly in assemblies)
            {
                //var assembly = assemblies.FirstOrDefault(a => a.Name == "Azure.Functions.V1");
                Type[] types = this.FindTypes(assembly);
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

                                var connectionStringName = triggerAttribute?.Connection;

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

                //TODO: Use one or the other, not both
                var connectionStringName = triggerAttribute?.Connection ?? attribute.Connection;
                var queueName = triggerAttribute?.QueueName ?? attribute.Queue;
                var subscriptionName = attribute.Subscription;
                var messageType = attribute.MessageType;
                var messageTypeName = attribute.MessageType.Name;

                var connectionString = Environment.GetEnvironmentVariable(connectionStringName, EnvironmentVariableTarget.Process);

                //var tokenProvider = GetTokenProvider(attribute.Connection);
                var managementClient = new ManagementClient(connectionString);

                var queues = await managementClient.GetQueuesAsync();

                //var queue = await managementClient.GetQueueAsync(queueName);
                var queue = queues.SingleOrDefault(q => q.Path == queueName);

                if (queue == null)
                {
                    queue = await managementClient.CreateQueueAsync(queueName);
                }

                var bundleTopicPath = "bundle-1";
                
                var topics = await managementClient.GetTopicsAsync();
                var bundleTopic = topics.SingleOrDefault(b => b.Path == bundleTopicPath);

                if (bundleTopic == null)
                {
                    try
                    {
                        await managementClient.CreateTopicAsync(bundleTopicPath)
                            //Should we add this on all?
                            .ConfigureAwait(false);
                    }
                    catch (MessagingEntityAlreadyExistsException)
                    {
                    }
                }

                var subscriptions = await managementClient.GetSubscriptionsAsync(bundleTopicPath);
                var subscription = subscriptions.SingleOrDefault(s => s.TopicPath == bundleTopicPath && s.SubscriptionName == subscriptionName);

                if (subscription == null)
                {
                    var subscriptionDescription = new SubscriptionDescription(bundleTopicPath, subscriptionName)
                    {
                        ForwardTo = queueName,
                        UserMetadata = $"Events {subscriptionName} is subscribed to",
                    };
                    var description = await managementClient.CreateSubscriptionAsync(subscriptionDescription);

                    await managementClient.DeleteRuleAsync(bundleTopicPath, description.SubscriptionName, "$Default");
                    
                    var eventName = messageType.GetFullName();
                    var rule = new RuleDescription
                    {
                        Name = eventName,
                        Filter = new SqlFilter($"[NServiceBus.EnclosedMessageTypes] LIKE '%{eventName}%'")
                    };
                    await managementClient.CreateRuleAsync("bundle-1", description.SubscriptionName, rule);
                }

                //var rules = await managementClient.GetRulesAsync(bundleTopicPath);


                /*
                var eventName = "MyEvent";
    var functionName = "Functionname";
    var subscriptionDescription = new SubscriptionDescription("bundle-1", functionName) {
       UserMetadata = $"Events {functionName} is subscribed to"
    };
    var description = await managementClient.CreateSubscriptionAsync(subscriptionDescription);

    await managementClient.DeleteRuleAsync(BundleName, description.SubscriptionName, "$Default");
    var rule = new RuleDescription();
    rule.Name = eventName;
    rule.Filter = new SqlFilter($"[NServiceBus.EnclosedMessageTypes] LIKE '%{eventName}%'");
    await managementClient.CreateRuleAsync("bundle-1", description.SubscriptionName, rule);
                */
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
