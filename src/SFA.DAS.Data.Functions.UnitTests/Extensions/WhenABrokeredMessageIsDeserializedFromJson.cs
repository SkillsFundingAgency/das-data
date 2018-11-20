using System.IO;
using System.Text;
using Microsoft.ServiceBus.Messaging;
using NUnit.Framework;
using SFA.DAS.Data.Functions.Extensions;

namespace SFA.DAS.Data.Functions.UnitTests.Extensions
{
    [TestFixture]
    public class WhenABrokeredMessageIsDeserializedFromJson
    {
        private class TestType
        {
            public string Text { get; set; }
            public int Number { get; set; }
        }

        [Test]
        public void ThenTheMessageIsDeserializedCorrectly()
        {
            var json = "{ Text: \"Hello World\", Number: 98 }";

            var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)); ;
            
            var message = new BrokeredMessage(stream);

            var result = message.DeserializeJsonMessage<TestType>();

            Assert.AreEqual("Hello World", result.Text);
            Assert.AreEqual(98, result.Number);
        }
    }
}
