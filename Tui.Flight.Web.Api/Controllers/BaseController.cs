using Microsoft.AspNetCore.Mvc;
using Tui.Flights.Web.Infrastructure.DataLayer.Itf;
using Tui.Flights.Web.Infrastructure.IntegrationEvents;

namespace Tui.Flights.Web.Api.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// BaseController
    /// </summary>
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// DataService
        /// </summary>
        protected ITuiDataServices DataService { get; }

        /// <summary>
        /// ITuiIntegrationEventService TuiIntegrationEventService
        /// </summary>
        protected ITuiIntegrationEventService TuiIntegrationEventService { get; }

        /// <summary>
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