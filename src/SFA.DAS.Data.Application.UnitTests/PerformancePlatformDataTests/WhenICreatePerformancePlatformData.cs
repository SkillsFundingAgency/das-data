using System;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace SFA.DAS.Data.Application.UnitTests.PerformancePlatformDataTests
{
    [TestFixture]
    public class WhenICreatePerformancePlatformData
    {
        private PerformancePlatformData _data;

        [SetUp]
        public void Arrange()
        {
            _data = new PerformancePlatformData(DateTime.Now, "Test", 50, 100);
        }

        [Test]
        public void ThenTheIdIsGenerated()
        {
            var expectedId = _data.Timestamp.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK") + _data.Service + _data.Period + _data.DataType + _data.Type;
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(expectedId);
            expectedId = Convert.ToBase64String(plainTextBytes);

            _data.Id.Should().Be(expectedId);
        }

        [Test]
        public void ThenTheServiceIsSet()
        {
            _data.Service.Should().Be("apprenticeships for employers");
        }

        [Test]
        public void ThenTheDataTypeIsSet()
        {
            _data.DataType.Should().Be("transaction-volumes");
        }

        [Test]
        public void ThenThePeriodIsSet()
        {
            _data.Period.Should().Be("day");
        }

        [Test]
        public void ThenIsIsSerializedCorrectly()
        {
            var expectedJson = @"{" +
                               $"\"_id\":\"{_data.Id}\"," +
                               $"\"_timestamp\":\"{_data.Timestamp:yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK}\"," +
                               $"\"service\":\"{_data.Service}\"," +
                               $"\"type\":\"{_data.Type}\"," +
                               $"\"count\":{_data.RecordsSinceLastRun}," +
                               $"\"dataType\":\"{_data.DataType}\"," +
                               $"\"period\":\"{_data.Period}\"" +
                               "}";

            var serializedObject = JsonConvert.SerializeObject(_data);

            serializedObject.Should().Be(expectedJson);
        }
    }
}
