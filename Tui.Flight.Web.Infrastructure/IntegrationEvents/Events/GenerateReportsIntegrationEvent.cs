﻿namespace Tui.Flights.Web.Infrastructure.IntegrationEvents.Events
{
    using Tui.Flights.Core.EventBus;

    /// <summary>
    /// GenerateReportsIntegrationEvent
    /// </summary>
    public class GenerateReportsIntegrationEvent : IntegrationEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateReportsIntegrationEvent"/> class.
        /// GenerateReportsIntegrationEvent
        /// </summary>
        /// <param name="flightId">flightId</param>
        /// <param name="flightPeriod">flightPeriod</param>
        /// <param name="flightDepartureAirport">flightDepartureAirport</param>
        /// <param name="flightArrivalAirport">flightArrivalAirport</param>
        public GenerateReportsIntegrationEvent(string flightId, string flightPeriod, string flightDepartureAirport, string flightArrivalAirport)
        {
            this.FlightId = flightId;
            this.FlightPeriod = flightPeriod;
            this.FlightDepartureAirport = flightDepartureAirport;
            this.FlightArrivalAirport = flightArrivalAirport;
        }

        /// <summary>
        /// Gets or sets flightId
        /// </summary>
        public string FlightId { get; set; }

        /// <summary>
        /// Gets or sets flightPeriod
        /// </summary>
        public string FlightPeriod { get; set; }

        /// <summary>
        /// Gets or sets flightArrivalAirport
        /// </summary>
        public string FlightArrivalAirport { get; set; }

        /// <summary>
        /// Gets or sets flightDepartureAirport
        /// </summary>
        public string FlightDepartureAirport { get; set; }
    }
}