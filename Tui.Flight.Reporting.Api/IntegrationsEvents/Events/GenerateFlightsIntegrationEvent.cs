using Tui.Flights.Core.EventBus;

namespace Tui.Flights.Reporting.Api.IntegrationsEvents.Events
{
    /// <summary>
    /// GenerateFlightsIntegrationEvent
    /// </summary>
    public class GenerateFlightsIntegrationEvent : IntegrationEvent
    {
        /// <summary>
        /// Gets or sets flightName
        /// </summary>
        public string FlightName { get; set; }

        /// <summary>
        /// Gets or sets flightPeriod
        /// </summary>
        public string FlightPeriod { get; set; }

        /// <summary>
        /// Gets or sets flightId
        /// </summary>
        public string FlightId { get; set; }

        /// <summary>
        /// Gets or sets serialNumbers
        /// </summary>
        public string[] SerialNumbers { get; set; }
    }
}
