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
        /// Flight
        /// </summary>
        public Flights()
        {
        }

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// FlightId
        /// </summary>
        public string FlightId { get; set; }

        /// <summary>
        /// DepartureAirport
        /// </summary>
        public string DepartureAirport { get; set; }

        /// <summary>
        /// ArrivalAirport
        /// </summary>
        public string ArrivalAirport { get; set; }

        /// <summary>
        /// Period
        /// </summary>
        public string Period { get; set; }

        /// <summary>
        /// StartDate
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// EndDate
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Number
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// TType
        /// </summary>
        public string Ttype { get; set; }

        /// <summary>
        /// IsEnabled
        /// </summary>
        public bool IsEnabled { get; set; }
    }
}
