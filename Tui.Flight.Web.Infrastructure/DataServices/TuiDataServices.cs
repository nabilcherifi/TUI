namespace Tui.Flights.Web.Infrastructure.DataServices
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Logging;
    using Tui.Flights.Web.Infrastructure.DataLayer;
    using Tui.Flights.Web.Infrastructure.DataLayer.Itf;

    /// <summary>
    /// TuiDataServices
    /// </summary>
    public class TuiDataServices : ITuiDataServices
    {
        private readonly ILogger<TuiDataServices> _logger;

        /// <summary>
        /// Gets or sets tuiContextng
        /// </summary>
        public ITuiUnitOfWork TuiUnitOfWork { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TuiDataServices"/> class.
        /// TuiDataServices
        /// </summary>
        public TuiDataServices()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TuiDataServices"/> class.
        /// TuiDataServices
        /// </summary>
        /// <param name="tuiUnitOfWork">qngUnitOfWork</param>
        /// <param name="logger">logger</param>
        public TuiDataServices(ITuiUnitOfWork tuiUnitOfWork, ILogger<TuiDataServices> logger)
        {
            this.TuiUnitOfWork = tuiUnitOfWork ?? throw new ArgumentException("TuiUnitOfWork is null");
            this._logger = logger;
        }

        /// <inheritdoc />
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
                FlightPeriod = flightPeriod
            };
            return flightReport;
        }

        /// <summary>
        /// FlightReport
        /// </summary>
        /// <param name="departureAirport">departureAirport</param>
        /// <param name="arrivalAirport">arrivalAirport</param>
        /// <param name="flightStartDate">flightStartDate</param>
        /// <param name="flightEndDate">flightEndDate</param>
        /// <returns>TuiNewFlight</returns>
        public TuiNewFlight FlightReport(string departureAirport, string arrivalAirport, string flightStartDate, string flightEndDate)
        {
            this._logger?.LogInformation("Entering FlightReport() from TuiDataServices");

            var flightIds =
                this.TuiUnitOfWork.FlightRepository.GetSummaryFlights(departureAirport, arrivalAirport, flightStartDate, flightEndDate);

            var flightReport = new TuiNewFlight(flightIds, departureAirport, arrivalAirport, flightStartDate, flightEndDate)
            {
                FlightIds = flightIds,
                DepartureAirport = departureAirport,
                ArrivalAirport = arrivalAirport,
                FlightStartDate = flightStartDate,
                FlightEndDate = flightEndDate
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
