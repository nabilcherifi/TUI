namespace Tui.Flights.Web.Infrastructure.IntegrationEvents
{
    using System.Threading.Tasks;
    using Tui.Flights.Core.EventBus;

    /// <summary>
    /// ITuiIntegrationEventService
    /// </summary>
    public interface ITuiIntegrationEventService
    {
        /// <summary>
        /// PublishThroughEventBusAsync
        /// </summary>
        /// <param name="evt">evt</param>
        /// <returns>Task</returns>
        Task PublishThroughEventBusAsync(IntegrationEvent evt);
    }
}
