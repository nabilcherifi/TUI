namespace Tui.Flights.Web.Api
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using RabbitMQ.Client;
    using Tui.Flights.Core.EventBus;
    using Tui.Flights.Core.EventBusClient;
    using Tui.Flights.Core.Logger;
    using Tui.Flights.Web.Core.Models;
    using Tui.Flights.Web.Infrastructure.DataLayer;
    using Tui.Flights.Web.Infrastructure.DataLayer.Itf;
    using Tui.Flights.Web.Infrastructure.DataServices;
    using Tui.Flights.Web.Infrastructure.IntegrationEvents;

    /// <summary>
    /// Startup class
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// Constructor
        /// </summary>
        /// <param name="configuration">Configuration key/values</param>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// Configuration
        /// </summary>
        public IConfiguration Configuration { get; set; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">Service collection</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Authentication
            var authenticationScheme = Microsoft.AspNetCore.Server.HttpSys.HttpSysDefaults.AuthenticationScheme;
            services.AddAuthenticationCore(options =>
            {
                options.DefaultAuthenticateScheme = authenticationScheme;
                options.DefaultChallengeScheme = authenticationScheme;
            });

            services.AddCors(options =>
            {
                options.AddPolicy(
                    "AllowAll",
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                    });
            });

            services.AddMvc().AddControllersAsServices();

            // Add DataBase services to the container from configuration file : appsettings.json
            services.AddDbContext<TuiDbContext>(options =>
                options.UseSqlServer(this.Configuration.GetConnectionString("TUIDatabase")));

            // UnitOfWork configuration
            services.AddScoped<ITuiUnitOfWork, TuiUnitOfWork>();

            // DataServices configuration
            services.AddScoped<ITuiDataServices, TuiDataServices>();

           // FlightRepository configuration logger
            services.AddTransient<IFlightRepository, FlightRepository>();

            // Swagger configuration
            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "Tui.Flights.Web.Api", Version = "v1", Description = "Tui Flight Web Api." });
            });

            // Add RabbitMq service
            services.AddTransient<ITuiIntegrationEventService, TuiIntegrationEventService>();
            services.AddSingleton<IRabbitMqPersistentConnection>(svc =>
            {
                var logger = svc.GetRequiredService<ILogger<RabbitMqPersistentConnection>>();
                return new RabbitMqPersistentConnection(new ConnectionFactory(), logger);
            });
            RegisterEventBus(services);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">Application pipeline</param>
        /// <param name="env">Hosting environment</param>
        /// <param name="loggerFactory">loggerFactory</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddLog4Net(System.IO.Path.Combine(env.ContentRootPath, "logger.config"));
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                loggerFactory.AddDebug();
                loggerFactory.AddConsole();
            }

            app.UseCors("AllowAll");

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tui Flight Web Api V1");
            });
        }

        /// <summary>
        /// RegisterEventBus
        /// </summary>
        /// <param name="services">services</param>
        private static void RegisterEventBus(IServiceCollection services)
        {
            services.AddSingleton<IEventBus, ClientBus>(svc =>
            {
                var rabbitMqPersistentConnection = svc.GetRequiredService<IRabbitMqPersistentConnection>();
                var evtBusSubsManager = svc.GetRequiredService<IEventBusSubscriptionsManager>();
                var logger = svc.GetRequiredService<ILogger<ClientBus>>();
                return new ClientBus(rabbitMqPersistentConnection, logger, svc, evtBusSubsManager, "Web");
            });
            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
        }

        /// <summary>
        /// ConfigureEventBus
        /// </summary>
        /// <param name="app">app</param>
        protected virtual void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
        }
    }
}
