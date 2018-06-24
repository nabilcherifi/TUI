namespace Tui.Flights.Web.Infrastructure.DataLayer
{
    /// <summary>
    /// TuiDataServiceFlight
    /// </summary>
    public class TuiNewFlight
    {
        private string _name;

        /// <summary>
        /// Gets or sets flightId
        /// </summary>
        public string FlightId { get; set; }

        /// <summary>
        /// Gets or sets <see cref="FlightIds"/>
        /// </summary>
        public string[] FlightIds { get; set; }

        /// <summary>
        /// Gets or sets flightStartDate
        /// </summary>
        public string FlightStartDate { get; set; }

        /// <summary>
        /// Gets or sets flightEndDate
        /// </summary>
        public string FlightEndDate { get; set; }

        /// <summary>
        /// Gets or sets flightPeriod
        /// </summary>
        public string FlightPeriod { get; set; }

        /// <summary>
        /// Gets or sets departureAirport
        /// </summary>
        public string DepartureAirport { get; set;  }

        /// <summary>
        /// Gets or sets arrivalAirport
        /// </summary>
        public string ArrivalAirport { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TuiNewFlight"/> class.
        /// TuiNewFlight
        /// </summary>
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
        /// Initializes a new instance of the <see cref="TuiNewFlight"/> class.
        /// TuiNewFlight
        /// </summary>
        /// /// <param name="flightIds">flightIds</param>
        /// <param name="departureAirport">departureAirport</param>
        /// <param name="arrivalAirport">arrivalAirport</param>
        /// <param name="flightStartDate">flightStartDate</param>
        /// <param name="flightEndDate">flightEndDate</param>
        public TuiNewFlight(string[] flightIds, string departureAirport, string arrivalAirport, string flightStartDate, string flightEndDate)
        {
            this.FlightIds = flightIds;
            this.DepartureAirport = departureAirport;
            this.ArrivalAirport = arrivalAirport;
            this.FlightStartDate = flightStartDate;
            this.FlightEndDate = flightEndDate;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TuiNewFlight"/> class.
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
