namespace Tui.Flights.ReportManager.Api
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

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
            Configuration = configuration;
        }

        /// <summary>
        /// Configuration
        /// </summary>
        public static IConfiguration Configuration { get; set; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">Service collection</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            // Swagger configuration
            services.AddSwaggerGen(options =>
             {
                 options.DescribeAllEnumsAsStrings();
                 options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "Tui.Flights.PersistenceManager.Api", Version = "v1", Description = "Tui Flight PersistenceManager Api." });
             });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">Application pipeline</param>
        /// <param name="env">Hosting environment</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tui Flight PersistenceManager Api V1");
            });
        }
    }
}
