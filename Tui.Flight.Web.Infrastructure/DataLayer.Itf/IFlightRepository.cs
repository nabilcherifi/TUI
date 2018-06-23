using System;
using System.Collections.Generic;
using Tui.Flights.Web.Core.Models;

namespace Tui.Flights.Web.Infrastructure.DataLayer.Itf
{
    /// <summary>
    /// IFlightRepository
    /// </summary>
    public interface IFlightRepository : IDisposable
    {
        /// <summary>
        /// GetFlights
        /// </summary>
        /// <param name="requestedFlight">requestedFlight</param>
        /// <returns>IEnumerable.TuiNewFlight</returns>
        IEnumerable<TuiNewFlight> GetFlights(TuiNewFlight requestedFlight);

        /// <summary>
        /// GetFlightById
        /// </summary>
        /// <param name="flightId">flightId</param>
        /// <returns>Flight</returns>
        Core.Models.Flights GetFlightById(int flightId);

        /// <summary>
        /// InsertFlight
        /// </summary>
        /// <param name="flights">flights</param>
        void InsertFlight(Core.Models.Flights flights);

        /// <summary>
        /// DeleteFlight
        /// </summary>
        /// <param name="flightId">flightId</param>
        void DeleteFlight(int flightId);

        /// <summary>
        /// UpdateFlight
        /// </summary>
        /// <param name="flight">flight</param>
        void UpdateFlight(Core.Models.Flights flight);

        /// <summary>
        /// Save
        /// </summary>
        void Save();

        /// <summary>
        /// GetSummaryFlights
        /// </summary>
        /// <param name="departureAirport">departureAirport</param>
        /// <param name="arrivalAirport">arrivalAirport</param>
        /// <param name="flightStartDate">flightStartDate</param>
        /// <param name="flightEndDate">flightEndDate</param>
        /// <returns>string[]</returns>
        string[] GetSummaryFlights(string departureAirport, string arrivalAirport, string flightStartDate, string flightEndDate);
    }
}