namespace Tui.Flights.Web.Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Tui.Flights.Web.Infrastructure.DataLayer.Itf;
    using Tui.Flights.Web.Infrastructure.IntegrationEvents;

    /// <inheritdoc />
    /// <summary>
    /// BaseController
    /// </summary>
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// Gets dataService
        /// </summary>
        protected ITuiDataServices DataService { get; }

        /// <summary>
        /// Gets iTuiIntegrationEventService TuiIntegrationEventService
        /// </summary>
        protected ITuiIntegrationEventService TuiIntegrationEventService { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController"/> class.
        /// AController
        /// </summary>
        /// <param name="ds">uow</param>
        /// <param name="eventService">eventService</param>
        protected BaseController(ITuiDataServices ds, ITuiIntegrationEventService eventService)
        {
            this.DataService = ds;
            this.TuiIntegrationEventService = eventService;
        }
    }
}