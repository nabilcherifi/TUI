using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Tui.Flights.Core.EventBus;
using Tui.Flights.Core.EventBusClient;
using Tui.Flights.Core.Logger;
using Tui.Flights.Reporting.Api.IntegrationsEvents.EventHandling;
using Tui.Flights.Reporting.Api.IntegrationsEvents.Events;

namespace Tui.Flights.Persistence.Api
{
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// Startup
        /// </summary>
        /// <param name="configuration">configuration</param>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// Startup
        /// </summary>
        /// <param name="loggerFactory">loggerFactory</param>
        public Startup(ILoggerFactory loggerFactory)
        {
            loggerFactory.CreateLogger("Logger");
        }

        /// <summary>
        /// Configuration
        /// </summary>
        public IConfiguration Configuration { get; set; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">Services list</param>
        public void ConfigureServices(IServiceCollection services)
        {
          services.AddCors(options =>
            {
                options.AddPolicy(
                    "AllowAny",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });
            services.AddMvc().AddControllersAsServices();

            // Swagger configuration
            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "Tui.Flights.Persistence.Api",
                    Version = "v1",
                    Description = "Tui Flight Persistence Api."
                });
            });

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
        /// <param name="app">app</param>
        /// <param name="env">env</param>
        /// <param name="loggerFactory">loggerFactory</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddLog4Net(System.IO.Path.Combine(env.ContentRootPath, "logger.config"));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseMvc();

            app.UseCors("AllowAny");
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            this.ConfigureEventBus(app);
        }

        private static void RegisterEventBus(IServiceCollection services)
        {
            services.AddSingleton<IEventBus, ClientBus>(svc =>
            {
                var rabbitMqPersistentConnection = svc.GetRequiredService<IRabbitMqPersistentConnection>();
                var evtBusSubsManager = svc.GetRequiredService<IEventBusSubscriptionsManager>();
                var logger = svc.GetRequiredService<ILogger<ClientBus>>();
                return new ClientBus(rabbitMqPersistentConnection, logger, svc, evtBusSubsManager, "Reporting");
            });
            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            // Add Event handlers
            services.AddTransient<GenerateFlightsMessageHandler>();
        }

        /// <summary>
        /// ConfigureEventBus
        /// </summary>
        /// <param name="app">app</param>
        protected virtual void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            // Add subscription
            eventBus.Subscribe<GenerateFlightsIntegrationEvent, GenerateFlightsMessageHandler>();
        }
    }
}
