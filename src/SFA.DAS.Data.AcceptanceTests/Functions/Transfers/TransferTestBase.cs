using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Events.Messages;
using Microsoft.Azure.WebJobs.Extensions;
using SFA.DAS.Data.AcceptanceTests.Data;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Infrastructure.Services;
using SFA.DAS.Data.Infrastructure.Data;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.AcceptanceTests.Functions.Transfers
{
   
    public class TransferTestBase
    {
        public TransferTestsRepository transferTestsRepository;
        public ILog log;
        public ITransferRelationshipService transferRelationshipService;

        //[SetUp]
        public async Task SetupDatabase()
        {
            try
            {
                log = new NLogLogger(typeof(TransferRelationshipMessageService), null);
                transferRelationshipService = new TransferRelationshipMessageService(new TransferRelationshipRepository(DataAcceptanceTests.Config.DatabaseConnectionString));
                transferTestsRepository = new TransferTestsRepository(DataAcceptanceTests.Config.DatabaseConnectionString);
                await transferTestsRepository.DeleteTransferRelationships();
            }
            catch (Exception e)
            {
                var a = e;
                throw;
            }
        
        }
  

    }
    
}
