using System;
using System.Threading.Tasks;
using Tui.Flights.Core.EventBus;

namespace Tui.Flights.Web.Infrastructure.IntegrationEvents
{
    /// <summary>
    /// TuiIntegrationEventService
    /// </summary>
    public class TuiIntegrationEventService : ITuiIntegrationEventService
    {
        private readonly IEventBus _eventBus;

        /// <summary>
        /// Initializes a new instance of the <see cref="TuiIntegrationEventService"/> class.
        /// TuiIntegrationEventService
        /// </summary>
        /// <param name="eventBus">eventBus</param>
        public TuiIntegrationEventService(IEventBus eventBus)
        {
            this._eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        /// <summary>
        /// PublishThroughEventBusAsync
        /// </summary>
        /// <param name="evt">evt</param>
        /// <returns>Task</returns>
        public Task PublishThroughEventBusAsync(IntegrationEvent evt)
        {
            this._eventBus.Publish(evt);
            return Task.FromResult(0);
        }
    }
}
