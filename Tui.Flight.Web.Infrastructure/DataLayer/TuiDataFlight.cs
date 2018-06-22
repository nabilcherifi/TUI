namespace Tui.Flights.Web.Infrastructure.DataLayer
{
    /// <summary>
    /// TuiDataServiceFlight
    /// </summary>
    public class TuiNewFlight
    {
        private string _name;

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
        /// FlightPeriod
        /// </summary>
        public string FlightPeriod { get; set; }

        /// <summary>
        /// DepartureAirport
        /// </summary>
        public string DepartureAirport { get; set;  }

        /// <summary>
        /// ArrivalAirport
        /// </summary>
        public string ArrivalAirport { get; set; }

        /// <summary>
        /// TuiNewFlight
        /// </summary>
        /// <param name="flightId">flightId</param>
        /// <param name="departureAirport">departureAirport</param>
        /// <param name="arrivalAirport">arrivalAirport</param>
        /// <param name="flightStartDate">flightStartDate</param>
        /// <param name="flightEndDate">flightEndDate</param>
        public TuiNewFlight(string departureAirport, string arrivalAirport, string flightStartDate, string flightEndDate)
        {
            this.DepartureAirport = departureAirport;
            this.ArrivalAirport = arrivalAirport;
            this.FlightStartDate = flightStartDate;
            this.FlightEndDate = flightEndDate;
        }

        /// <summary>
        /// TuiNewFlight
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="flightId">flightId</param>
        public TuiNewFlight(string name, string flightId)
        {
            this._name = name;
            this.FlightId = flightId;
        }
    }
}
