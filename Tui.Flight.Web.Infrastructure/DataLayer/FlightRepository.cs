using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tui.Flights.Web.Core.Models;
using Tui.Flights.Web.Infrastructure.DataLayer.Itf;
using Tui.Flights.Web.Infrastructure.IntegrationEvents.Events;

namespace Tui.Flights.Web.Infrastructure.DataLayer
{
    /// <summary>
    /// FlightRepository
    /// </summary>
    public class FlightRepository : GenericRepository<Core.Models.Flights>, IFlightRepository
    {
        private readonly ILogger<IFlightRepository> _logger;
        private readonly TuiDbContext _tuiContext;

        /// <summary>
        /// FlightRepository
        /// </summary>
        /// <param name="tuiContext">qngContext</param>
        public FlightRepository(TuiDbContext tuiContext)
            : base(tuiContext)
        {
            this._tuiContext = tuiContext ?? throw new ArgumentNullException(nameof(tuiContext));
        }

        /// <summary>
        /// FlightRepository
        /// </summary>
        /// <param name="tuiContext">qngContext</param>
        /// <param name="logger">logger</param>
        public FlightRepository(TuiDbContext tuiContext, ILogger<FlightRepository> logger)
            : base(tuiContext)
        {
            this._tuiContext = tuiContext ?? throw new ArgumentNullException(nameof(tuiContext));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// GetPeriods
        /// </summary>
        /// <returns>List</returns>
        public IEnumerable<string> GetPeriods()
        {
            this._logger?.LogInformation("Entering GetPeriods() method");

            // ... AsNoTracking() : Improve performance
            var periods = (from flg in this._tuiContext.Flights.AsNoTracking()
                    select flg.Period).Distinct().ToList();

            this._logger?.LogInformation("Leaving GetPeriods() method");

             return periods;
        }

        /// <summary>
        /// GetFlightById
        /// </summary>
        /// <param name="flightId">flightId</param>
        /// <returns>Flight</returns>
        public Core.Models.Flights GetFlightById(int flightId)
        {
            return this._tuiContext.Flights.Find(flightId);
        }

        /// <summary>
        /// InsertFlight
        /// </summary>
        /// <param name="flights">flight</param>
        public void InsertFlight(Core.Models.Flights flights)
        {
            this._tuiContext.Flights.Add(flights);
        }

        /// <summary>
        /// DeleteFlight
        /// </summary>
        /// <param name="flightId">flightId</param>
        public void DeleteFlight(int flightId)
        {
            var flight = this._tuiContext.Flights.Find(flightId);
            this._tuiContext.Flights.Remove(flight);
        }

        /// <summary>
        /// UpdateFlight
        /// </summary>
        /// <param name="flight">flight</param>
        public void UpdateFlight(Core.Models.Flights flight)
        {
            this._tuiContext.Entry(flight).State = EntityState.Modified;
        }

        /// <summary>
        /// Save
        /// </summary>
        public void Save()
        {
            this._tuiContext.SaveChanges();
        }

        /// <inheritdoc />
        /// <summary>
        /// GetFlights
        /// </summary>
        /// <param name="requestedFlight">requestedFlight</param>
        /// <returns>IEnumerable.TuiNewFlight</returns>
        public IEnumerable<TuiNewFlight> GetFlights(TuiNewFlight requestedFlight)
        {
            this._logger?.LogInformation("Entering GetFlights() method");

            // ... AsNoTracking() : Improve performance
            var flights = (from flg in this._tuiContext.Flights.AsNoTracking()
                where (flg.StartDate.ToShortDateString() == requestedFlight.FlightStartDate)
                      && (flg.EndDate.ToShortDateString() == requestedFlight.FlightEndDate)
                      && (flg.DepartureAirport == requestedFlight.DepartureAirport)
                      && (flg.ArrivalAirport == requestedFlight.ArrivalAirport)
                           select new { flg.Name, flg.FlightId }).AsNoTracking().Distinct().ToList();

            var flightsInPeriod = new List<TuiNewFlight>();
            foreach (var flight in flights)
            {
                flightsInPeriod.Add(new TuiNewFlight(flight.Name, flight.FlightId));
            }

            this._logger?.LogInformation("Leaving GetFlights() method");

            return flightsInPeriod;
        }

        private bool _disposed;

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing">disposing</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    this._tuiContext.Dispose();
                }
            }
            this._disposed = true;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
