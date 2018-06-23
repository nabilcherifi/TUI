using System;

namespace Tui.Flights.Core.EventBus
{
    /// <summary>
    /// IntegrationEvent
    /// </summary>
    public class IntegrationEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationEvent"/> class.
        /// IntegrationEvent
        /// </summary>
        public IntegrationEvent()
        {
            this.Id = Guid.NewGuid();
            this.CreationDate = DateTime.UtcNow;
        }

        /// <summary>
        /// Gets id
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Gets creationDate
        /// </summary>
        public DateTime CreationDate { get; }
    }
}
