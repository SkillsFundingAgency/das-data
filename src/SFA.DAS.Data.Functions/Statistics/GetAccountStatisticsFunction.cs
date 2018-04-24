using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.Data.Functions.Ioc;
using SFA.DAS.NLog.Logger;
using EasProcessingCompletedMessage = SFA.DAS.Data.Functions.Statistics.Commands.EasProcessingCompletedMessage;

namespace SFA.DAS.Data.Functions.Statistics
{
    public class GetAccountStatisticsFunction
    {
        public GetAccountStatisticsFunction()
        {
            
        }

        [FunctionName("GetAccountStatisticsFunction")]
        [return: Queue(QueueNames.CommitmentsQueueName)]
        //[DefaultRegistryAttribute]
        public static async Task<EasProcessingCompletedMessage> Run([TimerTrigger("*/60 * * * * *")] TimerInfo myTimer , [Inject] ILog log,
            [Inject("SFA.DAS.Data.Functions.Ioc.DefaultRegistry")] IStatisticsService statsService)
        {
            log.Debug("Gathering statistics for the EAS area of the system");
            var message = await statsService.CollateEasMetrics();

            return message as EasProcessingCompletedMessage;
        }
    }
}
