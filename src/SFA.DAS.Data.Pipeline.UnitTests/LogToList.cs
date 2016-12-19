using System.Collections.Generic;

namespace SFA.DAS.Data.Pipeline.UnitTests
{
    public class LogToList
    {
        public LogToList()
        {
            Messages = new List<string>();
        }

        public List<string> Messages { get; set; }

        public void Log(LoggingLevel level, string message)
        {
            Messages.Add(message);
        }
    }
}