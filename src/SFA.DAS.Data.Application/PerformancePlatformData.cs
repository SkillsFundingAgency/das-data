using System;
using Newtonsoft.Json;

namespace SFA.DAS.Data.Application
{
    public class PerformancePlatformData
    {
        public PerformancePlatformData(DateTime timestamp, string type, long recordsSinceLastRun, long totalNumberOfRecords)
        {
            Timestamp = timestamp;
            Type = type;
            RecordsSinceLastRun = recordsSinceLastRun;
            TotalNumberOfRecords = totalNumberOfRecords;
        }

        [JsonProperty(PropertyName = "_timestamp", Order = 2)]
        public DateTime Timestamp { get; }

        [JsonProperty(PropertyName = "type", Order = 4)]
        public string Type { get; }

        [JsonProperty(PropertyName = "count", Order = 5)]
        public long RecordsSinceLastRun { get; }

        [JsonIgnore]
        public long TotalNumberOfRecords { get; }

        [JsonProperty(PropertyName = "service", Order = 3)]
        public string Service => "apprenticeships for employers";

        [JsonProperty(PropertyName = "dataType", Order = 6)]
        public string DataType => "transaction-volumes";

        [JsonProperty(PropertyName = "period", Order = 7)]
        public string Period => "day";

        [JsonProperty(PropertyName = "_id", Order = 1)]
        public string Id => GenerateId();

        private string GenerateId()
        {
            var plainTextId = Timestamp.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK") + Service + Period + DataType + Type;
            return Base64Encode(plainTextId);
        }

        private string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
    }
}
