using System.Threading.Tasks;
using Tui.Flights.Core.EventBus;

namespace Tui.Flights.Web.Infrastructure.IntegrationEvents
{
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
