using System.Threading.Tasks;

namespace Tui.Flights.Core.EventBus
{
    /// <summary>
    /// IIntegrationMessageHandler
    /// </summary>
    /// <typeparam name="TIntegrationEvent">param</typeparam>
    public interface IIntegrationMessageHandler<in TIntegrationEvent>
        where TIntegrationEvent : IntegrationEvent
    {
        /// <summary>
        /// Handle
        /// </summary>
        /// <param name="flightEvent">flightEvent</param>
        /// <returns>Task</returns>
        Task Handle(TIntegrationEvent flightEvent);
    }
}
