using LoggerLibrary;
using LoggerLibrary.Extension;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VacationManagerBackend.Models.Config;

namespace VacationManagerBackend
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;

            Logger.Initialize(env, Configuration);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<DbConfig>(Configuration.GetSection("DbConfig"));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseRequestLogging();
            app.UseUnhandledExceptionLogging();
            app.UseHttpsRedirection();
            app.UseMvc();

            loggerFactory.AddLogger();

            var logger = loggerFactory.CreateLogger<Startup>();
            logger.Info("Application started!");
        }
    }
}
