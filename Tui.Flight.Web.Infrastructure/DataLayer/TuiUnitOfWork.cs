using System;
using System.Linq;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Tui.Flights.Web.Core.Models;
using Tui.Flights.Web.Infrastructure.DataLayer.Itf;

namespace Tui.Flights.Web.Infrastructure.DataLayer
{
    /// <summary>
    /// The UnitOfWork serves one purpose : to make sure that when you use multiple repositories, they share a single database context
    /// </summary>
    public class TuiUnitOfWork : ITuiUnitOfWork
    {
        private readonly TuiDbContext _context;

        /// <summary>
        /// FlightRepository
        /// </summary>
        public IFlightRepository FlightRepository { get; }

        /// <summary>
        /// UnitOfWork
        /// </summary>
        /// <param name="dbContext">dbContext</param>
        public TuiUnitOfWork(TuiDbContext dbContext)
        {
            this._context = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.FlightRepository = this._context.GetService<IFlightRepository>();
        }

        /// <summary>
        /// Rollback
        /// </summary>
        public void Rollback()
        {
            this._context
                .ChangeTracker
                .Entries()
                .ToList()
                .ForEach(x => x.Reload());
        }

        /// <summary>
        /// Save
        /// </summary>
        public void Save()
        {
            this._context.SaveChanges();
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
                    this._context.Dispose();
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

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing">disposing</param>
        void ITuiUnitOfWork.Dispose(bool disposing)
        {
            this.Dispose(true);
        }
    }
}
