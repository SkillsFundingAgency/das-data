using System;

namespace SFA.DAS.Data.Application
{
    public class PerformancePlanData
    {
        public PerformancePlanData(DateTime timestamp, string dataType, long recordsSinceLastRun, long totalNumberOfRecords)
        {
            Timestamp = timestamp;
            DataType = dataType;
            RecordsSinceLastRun = recordsSinceLastRun;
            TotalNumberOfRecords = totalNumberOfRecords;
        }

        public DateTime Timestamp { get; }
        public string DataType { get; }
        public long RecordsSinceLastRun { get; }
        public long TotalNumberOfRecords { get; }
    }
}
