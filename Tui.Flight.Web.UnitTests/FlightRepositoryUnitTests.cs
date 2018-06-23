using System.Linq;
using NUnit.Framework;
using Tui.Flights.Web.Infrastructure.DataLayer;
using Tui.Flights.Web.Tests.Mock;

namespace Tui.Flights.Web.Tests.UnitTests
{
    /// <summary>
    /// FlightRepositoryTester
    /// </summary>
    [TestFixture]
    public static class FlightRepositoryUnitTests
    {
        /// <summary>
        /// ShouldReturnPeriods
        /// </summary>
        /// <param name="expectedPeriod1">expectedPeriod1</param>
        /// <param name="expectedPeriod2">expectedPeriod2</param>
        [TestCase("May 2018", "June 2017")]
        public static void ShouldReturnPeriods(string expectedPeriod1, string expectedPeriod2)
        {
            var mockTuicontext = MockTuiContextInMemoryWithData.MockContext();
            var flightRepository = new FlightRepository(mockTuicontext);
            Assert.IsNotNull(flightRepository);

            var periods = flightRepository.GetPeriods();
            Assert.IsNotNull(periods);

            var requestedPeriods = periods.ToArray();

            Assert.AreEqual(requestedPeriods[0], expectedPeriod1);
            Assert.AreEqual(requestedPeriods[1], expectedPeriod2);
        }

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
            var mockTuicontext = MockTuiContextInMemoryWithData.MockContext();
            var flightRepository = new FlightRepository(mockTuicontext);
            Assert.IsNotNull(flightRepository);

            var requestedFlight = new TuiNewFlight(departureAirport, arrivalAirport, flightStartDate, flightEndDate);
            var requestedFlightId = flightRepository.GetFlights(requestedFlight);
            Assert.IsNotNull(requestedFlightId);

            var expectedFlightId = new[] { "13584", "13585", "13586", "13587", "13588", "13589", "13590", "13591", "13592", "13593", "13594", "13595", "13596", "11303", "11304", "11305", "11306", "11307" };
            Assert.AreEqual(requestedFlightId, expectedFlightId);
        }
    }
}
