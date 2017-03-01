namespace SFA.DAS.Data.Worker.EventHandlers
{
    public interface IEventHandler<in T>
    {
        void Handle(T @event);
    }
}
