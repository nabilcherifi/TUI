namespace Tui.Flights.Web.Api.RequestModels
{
    /// <summary>
    /// FlightRequest
    /// </summary>
    public class FlightRequest
    {
        /// <summary>
        /// FlightId
        /// </summary>
        public string FlightId { get; set; }

        /// <summary>
        /// FlightStartDate
        /// </summary>
        public string FlightStartDate { get; set; }

        /// <summary>
        /// FlightEndDate
        /// </summary>
        public string FlightEndDate { get; set; }

        /// <summary>
        /// DepartureAirport
        /// </summary>
        public string DepartureAirport { get; set; }

        /// <summary>
        /// ArrivalAirport
        /// </summary>
        public string ArrivalAirport { get; set; }
    }
}
