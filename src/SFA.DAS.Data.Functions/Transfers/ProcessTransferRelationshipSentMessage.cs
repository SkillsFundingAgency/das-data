using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.ServiceBus.Messaging;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Domain.Enum;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.Data.Infrastructure.Data;
using SFA.DAS.EmployerAccounts.Events.Messages;

namespace SFA.DAS.Data.Functions.Transfers
{
    public class ProcessTransferRelationshipSentMessage
    {
        private static string connectionString =
            "Server=(localdb)\\ProjectsV13;Database=SFA.DAS.Data.Database;Integrated Security = true;Trusted_Connection=True;Pooling=False;Connect Timeout=30;MultipleActiveResultSets=True";
        [FunctionName("ProcessTransferRelationshipSentMessage")]
        public static void Run([ServiceBusTrigger("sent_transfer_connection_invitation", "RDS_SentTransferConnectionInvitiationProcessor", AccessRights.Manage,Connection = "MessageBusConnectionString")] SentTransferConnectionInvitationEvent Message, ExecutionContext executionContext, TraceWriter log)
        {

            //This needs to be pulled from DI rather than being created here.
            ITransferRelationshipRepository transferRelationshipRepository = new TransferRelationshipRepository(connectionString);


            var transferRelationship = new TransferRelationship()
            {
                SenderAccountId = Message.SenderAccountId,
                ReceiverAccountId = Message.ReceiverAccountId,
                RelationshipStatus = TransferRelationshipStatus.Pending,
                SenderUserId = Message.SentByUserId
            };

            transferRelationshipRepository.SaveTransferRelationship(transferRelationship);

            log.Info($"C# service bus trigger function executed at ProcessTransferRelationshipStartMessage: {DateTime.Now}");
        }

        public static T GetBody<T>(BrokeredMessage brokeredMessage)
        {
            var ct = brokeredMessage.ContentType;
            Type bodyType = Type.GetType(ct, true);

            var stream = brokeredMessage.GetBody<Stream>();
            DataContractSerializer serializer = new DataContractSerializer(bodyType);
            XmlDictionaryReader reader = XmlDictionaryReader.CreateBinaryReader(stream, XmlDictionaryReaderQuotas.Max);
            object deserializedBody = serializer.ReadObject(reader);
            T msgBase = (T)deserializedBody;
            return msgBase;
        }
    }
}
