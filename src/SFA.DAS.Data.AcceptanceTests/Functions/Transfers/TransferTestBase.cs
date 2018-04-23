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

namespace SFA.DAS.Data.AcceptanceTests.Functions.Transfers
{
   
    public class TransferTestBase
    {
        public TransferTestsRepository transferTestsRepository;

        //[SetUp]
        public async Task SetupDatabase()
        {
            try
            {
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
