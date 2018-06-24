namespace Tui.Flights.Web.Infrastructure.DataLayer.Itf
{
    using System;

    /// <summary>
    /// ITuiUnitOfWork
    /// </summary>
    public interface ITuiUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets flightRepository
        /// </summary>
        IFlightRepository FlightRepository { get; }

        /// <summary>
        /// Rollback
        /// </summary>
        void Rollback();

        /// <summary>
        /// Save
        /// </summary>
        void Save();

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing">disposing</param>
        void Dispose(bool disposing);

        /// <summary>
        /// Dispose
        /// </summary>
        new void Dispose();
    }
}