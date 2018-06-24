namespace Tui.Flights.Web.Tests.Mock
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Microsoft.EntityFrameworkCore;
    using Tui.Flights.Web.Core.Models;

    /// <summary>
    /// MockTuiContextInMemoryWithData
    /// </summary>
    public class MockTuiContextInMemoryWithData
    {
        private MockTuiContextInMemoryWithData()
        {
        }

        /// <summary>
        /// MockContext
        /// </summary>
        /// <returns>void</returns>
        public static TuiDbContext MockContext()
        {
            // In-memory database by GUID. This is to make sure that every test run has new database.
            var options = new DbContextOptionsBuilder<TuiDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new TuiDbContext(options);

            var flights = new List<Core.Models.Flights>
            {
                new Core.Models.Flights { Id = 1, FlightId = "13584", DepartureAirport = "Paris", ArrivalAirport = "Doubaï", StartDate = Convert.ToDateTime("1/1/2018", CultureInfo.InvariantCulture), EndDate = Convert.ToDateTime("1/2/2018", CultureInfo.InvariantCulture) },
                new Core.Models.Flights { Id = 3, FlightId = "13585", DepartureAirport = "Paris", ArrivalAirport = "Marrackech", StartDate = Convert.ToDateTime("1/2/2018", CultureInfo.InvariantCulture), EndDate = Convert.ToDateTime("1/3/2018", CultureInfo.InvariantCulture) },
                new Core.Models.Flights { Id = 2, FlightId = "13586", DepartureAirport = "Paris", StartDate = Convert.ToDateTime("1/3/2018", CultureInfo.InvariantCulture), EndDate = Convert.ToDateTime("1/4/2018", CultureInfo.InvariantCulture) }
            };

            // Load InMemory Mock-Flights with Data
            foreach (var flight in flights)
            {
                context.Flights.Add(flight);
            }

            context.SaveChanges();

            return context;
        }
    }
}
