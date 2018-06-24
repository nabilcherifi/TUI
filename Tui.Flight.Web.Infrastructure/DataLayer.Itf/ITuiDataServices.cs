namespace Tui.Flights.Web.Infrastructure.DataLayer.Itf
{
    using System.Collections.Generic;

    /// <summary>
    /// ITuiDataServices
    /// </summary>
    public interface ITuiDataServices
    {
        /// <summary>
        /// GetFlights
        /// </summary>
        /// <param name="departureAirport">departureAirport</param>
        /// <param name="arrivalAirport">arrivalAirport</param>
        /// <param name="startDate">startDate</param>
        /// <param name="endDate">endDate</param>
        /// <returns>TuiNewFlight</returns>
        IEnumerable<TuiNewFlight> GetFlights(string departureAirport, string arrivalAirport, string startDate, string endDate);

        /// <summary>
        /// FlightReport
        /// </summary>
        /// <param name="flightPeriod">flightPeriod</param>
        /// <param name="flightName">flightName</param>
        /// <returns>TuiDataFlight</returns>
        TuiNewFlight FlightReport(string flightPeriod, string flightName);

        /// <summary>
        /// FlightReport
        /// </summary>
        /// <param name="departureAirport">departureAirport</param>
        /// <param name="arrivalAirport">arrivalAirport</param>
        /// <param name="flightStartDate">flightStartDate</param>
        /// <param name="flightEndDate">flightEndDate</param>
        /// <returns>TuiNewFlight</returns>
        TuiNewFlight FlightReport(string departureAirport, string arrivalAirport, string flightStartDate, string flightEndDate);
    }
}