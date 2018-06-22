using Tui.Flights.Core.EventBus;

namespace Tui.Flights.Web.Infrastructure.IntegrationEvents.Events
{
    /// <summary>
    /// GenerateReportsIntegrationEvent
    /// </summary>
    public class GenerateFlightsIntegrationEvent : IntegrationEvent
    {
        /// <summary>
        /// FlightId
        /// </summary>
        public string FlightId { get; set; }

        /// <summary>
        /// FlightPeriod
        /// </summary>
        public string FlightPeriod { get; set; }

        /// <summary>
        /// FlightArrivalAirport
        /// </summary>
        public string FlightArrivalAirport { get; set; }

        /// <summary>
        /// FlightDepartureAirport
        /// </summary>
        public string FlightDepartureAirport { get; set; }

        /// <summary>
        /// GenerateReportsIntegrationEvent
        /// </summary>
        /// <param name="flightId">flightId</param>
        /// <param name="flightPeriod">flightPeriod</param>
        /// <param name="flightDepartureAirport">flightDepartureAirport</param>
        /// <param name="flightArrivalAirport">flightArrivalAirport</param>
        public GenerateFlightsIntegrationEvent(string flightId, string flightPeriod, string flightDepartureAirport, string flightArrivalAirport)
        {
            this.FlightId = flightId;
            this.FlightPeriod = flightPeriod;
            this.FlightDepartureAirport = flightDepartureAirport;
            this.FlightArrivalAirport = flightArrivalAirport;
        }
    }
}
