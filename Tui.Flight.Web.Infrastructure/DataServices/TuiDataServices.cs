using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Tui.Flights.Web.Infrastructure.DataLayer;
using Tui.Flights.Web.Infrastructure.DataLayer.Itf;

namespace Tui.Flights.Web.Infrastructure.DataServices
{
    /// <summary>
    /// TuiDataServices
    /// </summary>
    public class TuiDataServices : ITuiDataServices
    {
        private readonly ILogger<TuiDataServices> _logger;

        /// <summary>
        /// TuiContextng
        /// </summary>
        public ITuiUnitOfWork TuiUnitOfWork { get; set; }

        /// <summary>
        /// TuiDataServices
        /// </summary>
        /// <param name="qngUnitOfWork">qngUnitOfWork</param>
        /// <param name="logger">logger</param>
        public TuiDataServices(ITuiUnitOfWork qngUnitOfWork, ILogger<TuiDataServices> logger)
        {
            this.TuiUnitOfWork = qngUnitOfWork ?? throw new ArgumentException("TuiUnitOfWork is null");
            this._logger = logger;
        }

        /// <summary>
        /// TuiDataServices
        /// </summary>
        public TuiDataServices()
        {
        }

        /// <summary>
        /// FlightReport
        /// </summary>
        /// <param name="flightPeriod">flightPeriod</param>
        /// <param name="flightName">flightName</param>
        /// <returns>flightReport</returns>
        public TuiNewFlight FlightReport(string flightPeriod, string flightName)
        {
            this._logger?.LogInformation("Entering FlightReport() from TuiDataServices");

            var flightReport = new TuiNewFlight(flightPeriod, flightName)
            {
                FlightPeriod = flightPeriod,
            };
            return flightReport;
        }

        /// <inheritdoc />
        /// <summary>
        /// GetFlights
        /// </summary>
        /// <param name="departureAirport">departureAirport</param>
        /// <param name="arrivalAirport">arrivalAirport</param>
        /// <param name="startDate">startDate</param>
        /// <param name="endDate">endDate</param>
        /// <returns>TuiNewFlight</returns>
        public IEnumerable<TuiNewFlight> GetFlights(
            string departureAirport,
            string arrivalAirport,
            string startDate,
            string endDate)
        {
            var requestedFlight = new TuiNewFlight(departureAirport, arrivalAirport, startDate, endDate);

            var flights = this.TuiUnitOfWork.FlightRepository.GetFlights(requestedFlight);

            return flights;
        }
    }
}
