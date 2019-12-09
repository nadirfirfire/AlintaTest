namespace Customers.API
{
    using Customers.DbContexts;
    using Customers.Repo;
    using CustomersAPI.Service.Controllers;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Versioning;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json.Serialization;
    using Swashbuckle.AspNetCore.Swagger;
    using System;

    /// <summary>
    /// Defines the <see cref="Startup" />
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration<see cref="IConfiguration"/></param>
        /// <param name="env">The env<see cref="IHostingEnvironment"/></param>
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            var contentRoot = env.ContentRootPath;
        }

        /// <summary>
        /// Gets the Configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// The ConfigureServices
        /// </summary>
        /// <param name="services">The services<see cref="IServiceCollection"/></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2).AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
            
            services.AddScoped<ILogger, Logger<CustomerController>>();
            services.AddScoped<CustomerRepo>();

            services.AddDbContext<CustomerDBContext>(options => options.UseInMemoryDatabase(databaseName: "CustomerDB"));

            services.AddApiVersioning(delegate (ApiVersioningOptions v)
            {
                v.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
                v.ReportApiVersions = true;
                v.AssumeDefaultVersionWhenUnspecified = true;
                v.DefaultApiVersion = new ApiVersion(1, 0);
            });

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = "Customer Management API", Version = "v1" });


                options.OperationFilter<RemoveVersionParameter>(Array.Empty<object>());

                options.DocumentFilter<ReplaceVersionWithExactValueInPath>(Array.Empty<object>());

            });
        }

        /// <summary>
        /// The Configure
        /// </summary>
        /// <param name="app">The app<see cref="IApplicationBuilder"/></param>
        /// <param name="env">The env<see cref="IHostingEnvironment"/></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable Swagger as a JSON endpoint.
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "Customer Management API V1");
                c.InjectStylesheet("../css/swagger.min.css");
            });

            app.UseMvc();
        }
    }
}
