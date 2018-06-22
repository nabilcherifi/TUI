using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Tui.Flights.Persistence.Api.Controllers
{
    /// <summary>
    /// Home controller : http://localhost:27960/api/v1/Home
    /// </summary>
    [Route("api/v1/[controller]")]
    public class HomeController : Controller
    {
        /// <summary>
        /// Default Get methods (Deployment test)
        /// </summary>
        /// <returns>The Web API project name</returns>
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new[] { "TUI.Persistence.Api" };
        }
    }
}
