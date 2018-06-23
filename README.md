## The main purpose of TUI Application is to provide the user with a flight reservation process.

### TUI Application is at the present time Back-End only. 
It is built on ASP Net MVC Core and uses **Entity Framework Core** as object database mapping model for Reading user requested flight information from Database.
TUI Application is also built around **RabbitMQ** communication Bus in order to support and make user requested flight informations persistent.
This principle is based on **CQRS pattern**, a modern implementation that **separates READ and WRITE operations**. 

Data READ operations are separated from data WRITE operations by using different interfaces. As per this architecture, TUI Application uses Entity Framework object mapping model to get user flight request (READ operations) and uses RabbitMQ asynchronous event based communication to support all WRITE/UPDATE operations, in order to make persistent user requested flight information. 
This allows optimizing performances, guaranty extensibility and provide a safety READ/WRITE architecture. This model takes care of system evolution via more flexibility and prevents the WRITE/UPDATE commands from introducing conflicts with READ commands.

**UnitOfWork** has been also implemented along with **Repository**. The UnitOfWork and Repository patterns are intended to act like an abstraction layer between business logic and data access layer. This can help insulate the application from changes in the data store and can facilitate automated unit testing / test driven development (TDD).  

I also started to implement **SSRS** to provide a summary report of all entred and calculated data.

### TESTS

**Unit Tests** and **Integration Tests** have been added using **NUnit**

**MOCK** of DBContext (SQL Database) has been implemented.

**A set of In memory data has been provided for Test Mock purpose.**

### Front End is at the present time illustrated using Swagger UI.

The Front-End part is at this time illustrated by **Swagger-UI** interface. 
Later the application will use a Front-End based Angular development in place of Swagger UI.

### Here below are the TUI Back-End Application architecture components as they appear in the Visual Studio 2017 Solution:

**0.Common.Ressources (Coding RULES for Best Pratctices)**
  - *ruleSetHigh.dev.ruleset*
  - *RuleSetMedium.dev.ruleset*
  
**1.Global (RabbitMQ bus resources)**
  - *Tui.Flights.Core*
  - *Tui.Flights.Core.EventBus*
  - *Tui.Flights.Core.Logger*
  - *Tui.FlightsCore.EventBusClient*
  
**2.Application (UnitOfWork, Repository, Data Access Layer)**
  - Tui.Flights.Web.Core (Contains EntityFramework Models)
    - Models
      - *Flights.cs*
      - *TuiDbContext.cs*
  - Tui.Flights.Web.Api (Entry point for our Back End solution)
  - Tui.Flights.Web.Infrastructure
    - DataLayer
      - *FlightRepository.cs (EntityFramework based)*
      - *TuiUnitOfWork.cs*
    - DataServices
    - IntegrationEvents (RabbitMQ events for Publisher/Consumer)
  
**3.Services (Data Persistence through RabbitMQ bus communication)**
  - 3.1.Persistence
    - Tui.Flights.Persistence.Core
    - Tui.Flights.Persistence.Api (Entry point : receives http fligth request from FrontEnd)
  - 3.2.PersistenceManager
    - Tui.Flights.PersistenceManager.Core
    - Tui.Flights.PersistenceManager.Api
   
**4.Tests**
  - 4.1.Application
    - *Tui.Flights.Web.Tests.IntegrationTests*
    - *Tui.Flights.Web.Tests.Mock* 
    - *Tui.Flights.Web.Tests.UnitTests*

**ENTRY POINT** is "GetFlights" method at "HomeController" in "Tui.Flights.Web.Api" project located in "Application" folder. 
"GetFlights" method receives user http Front-End new flight information requests.

**Mr. Nabil CHERIFI. Senior .Net Developper and Technical Trainer.**
