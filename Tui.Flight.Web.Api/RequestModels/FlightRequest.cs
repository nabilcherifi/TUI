namespace Tui.Flights.Web.Api.RequestModels
{
    /// <summary>
    /// FlightRequest
    /// </summary>
    public class FlightRequest
    {
        /// <summary>
        /// Gets or sets flightId
        /// </summary>
        public string FlightId { get; set; }

        /// <summary>
        /// Gets or sets flightStartDate
        /// </summary>
        public string FlightStartDate { get; set; }

        /// <summary>
        /// Gets or sets flightEndDate
        /// </summary>
        public string FlightEndDate { get; set; }

        /// <summary>
        /// Gets or sets departureAirport
        /// </summary>
        public string DepartureAirport { get; set; }

        /// <summary>
        /// Gets or sets arrivalAirport
        /// </summary>
        public string ArrivalAirport { get; set; }
    }
}
