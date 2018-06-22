using Microsoft.EntityFrameworkCore;

namespace Tui.Flights.Web.Core.Models
{
    /// <inheritdoc />
    /// <summary>
    /// TUIContext
    /// </summary>
    public class TuiDbContext : DbContext
    {
        /// <summary>
        /// Flight
        /// </summary>
        public virtual DbSet<Flights> Flights { get; set; }

        /// <summary>
        /// TuiContext
        /// </summary>
        /// <param name="options">options</param>
        public TuiDbContext(DbContextOptions<TuiDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// OnModelCreating
        /// </summary>
        /// <param name="modelBuilder">modelBuilder</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Flights>(entity =>
            {
                entity.ToTable("Flights");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.ArrivalAirport)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.DepartureAirport)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.Period)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.FlightId)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.StartDate)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.EndDate)
                    .IsRequired()
                    .HasMaxLength(15);
            });
        }
    }
}
