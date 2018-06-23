using System;
using System.Collections.Generic;

namespace Tui.Flights.Web.Core.Models
{
    /// <summary>
    /// Flight
    /// </summary>
    public class Flights
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Flights"/> class.
        /// Flight
        /// </summary>
        public Flights()
        {
        }

        /// <summary>
        /// Gets or sets id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets flightId
        /// </summary>
        public string FlightId { get; set; }

        /// <summary>
        /// Gets or sets departureAirport
        /// </summary>
        public string DepartureAirport { get; set; }

        /// <summary>
        /// Gets or sets arrivalAirport
        /// </summary>
        public string ArrivalAirport { get; set; }

        /// <summary>
        /// Gets or sets period
        /// </summary>
        public string Period { get; set; }

        /// <summary>
        /// Gets or sets startDate
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets endDate
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets number
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Gets or sets tType
        /// </summary>
        public string Ttype { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether isEnabled
        /// </summary>
        public bool IsEnabled { get; set; }
    }
}
