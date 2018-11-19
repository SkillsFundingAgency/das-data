using SFA.DAS.Provider.Events.Api.Types;

namespace SFA.DAS.Data.Worker.Events
{
    public class PeriodEndEvent<T>
    {
        public PeriodEnd PeriodEnd { get; set; }
    }
}
