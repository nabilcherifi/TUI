namespace Tui.Flights.Web.Tests.IntegrationTests
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using NUnit.Framework;
    using Tui.Flights.Web.Core.Models;
    using Tui.Flights.Web.Infrastructure.DataLayer;

    /// <summary>
    /// FlightRepositoryIntegrationTests
    /// </summary>
    [TestFixture]
    public static class FlightRepositoryIntegrationTests
    {
        /// <summary>
        /// ShouldReturnFlights
        /// </summary>
        /// <param name="departureAirport">departureAirport</param>
        /// <param name="arrivalAirport">arrivalAirport</param>
        /// <param name="flightStartDate">flightStartDate</param>
        /// <param name="flightEndDate">flightEndDate</param>
        [TestCase("Paris", "Doubaï", "1/1/2018", "1/2/2018")]
        [TestCase("Paris", "Chicago", "1/1/2018", "1/2/2018")]
        public static void ShouldReturnFlights(string departureAirport, string arrivalAirport, string flightStartDate, string flightEndDate)
        {
            // Gets an instance of SQL DBContex : the real NON MOCKED SQL database
            var sqlDbContext = SqlTuiContext();
            var flightRepository = new FlightRepository(sqlDbContext);
            Assert.IsNotNull(flightRepository);

            var requestedFlight = new TuiNewFlight(departureAirport, arrivalAirport, flightStartDate, flightEndDate);
            var requestedFlightId = flightRepository.GetFlights(requestedFlight);
            Assert.IsNotNull(requestedFlightId);

            var expectedFlightId = new[] { "13584", "13585", "13586", "13587", "13588", "13589", "13590", "13591", "13592", "13593", "13594", "13595", "13596", "11303", "11304", "11305", "11306", "11307" };
            Assert.AreEqual(requestedFlightId, expectedFlightId);
        }

        // Returns NON MOCKED DBContext that points to the real SQL Database
        private static TuiDbContext SqlTuiContext()
        {
            var webHostBuilder = new WebHostBuilder();
            var env = webHostBuilder.GetSetting("environment");
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env}.json", true)
                .AddEnvironmentVariables();

            var config = builder.Build();
            var connstr = config.GetConnectionString("TUIDatabase");
            Assert.IsNotNull(connstr);

            var options = new DbContextOptionsBuilder<TuiDbContext>()
                .UseSqlServer(connstr)
                .Options;
            Assert.IsNotNull(options);

            var context = new TuiDbContext(options);
            Assert.IsNotNull(context);

            return context;
        }
    }
}
