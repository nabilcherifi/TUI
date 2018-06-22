using System.Collections.Generic;

namespace Tui.Flights.Web.Infrastructure.DataLayer.Itf
{
    /// <summary>
    /// ITuiDataServices
    /// </summary>
    public interface ITuiDataServices
    {
        /// <summary>
        /// FlightReport
        /// </summary>
        /// <param name="flightPeriod">flightPeriod</param>
        /// <param name="flightName">flightName</param>
        /// <returns>TuiDataFlight</returns>
        TuiNewFlight FlightReport(string flightPeriod, string flightName);

        /// <summary>
        /// GetFlights
        /// </summary>
        /// <param name="departureAirport">departureAirport</param>
        /// <param name="arrivalAirport">arrivalAirport</param>
        /// <param name="startDate">startDate</param>
        /// <param name="endDate">endDate</param>
        /// <returns>TuiNewFlight</returns>
        IEnumerable<TuiNewFlight> GetFlights(string departureAirport, string arrivalAirport, string startDate, string endDate);
    }
}