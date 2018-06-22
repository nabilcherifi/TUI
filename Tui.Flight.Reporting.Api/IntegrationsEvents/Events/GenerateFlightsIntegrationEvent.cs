using Tui.Flights.Core.EventBus;

namespace Tui.Flights.Reporting.Api.IntegrationsEvents.Events
{
    /// <summary>
    /// GenerateFlightsIntegrationEvent
    /// </summary>
    public class GenerateFlightsIntegrationEvent : IntegrationEvent
    {
        /// <summary>
        /// FlightName
        /// </summary>
        public string FlightName { get; set; }

        /// <summary>
        /// FlightPeriod
        /// </summary>
        public string FlightPeriod { get; set; }

        /// <summary>
        /// FlightId
        /// </summary>
        public string FlightId { get; set; }

        /// <summary>
        /// SerialNumbers
        /// </summary>
        public string[] SerialNumbers { get; set; }
    }
}
