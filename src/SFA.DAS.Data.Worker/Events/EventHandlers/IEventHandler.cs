namespace SFA.DAS.Data.Worker.Events.EventHandlers
{
    public interface IEventHandler<in T>
    {
        void Handle(T @event);
    }
}
