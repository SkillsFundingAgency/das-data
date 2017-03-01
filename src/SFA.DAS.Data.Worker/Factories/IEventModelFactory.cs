using System.Threading.Tasks;

namespace SFA.DAS.Data.Worker.Factories
{
    public interface IEventModelFactory
    {
        T Create<T>(string data);
    }
}