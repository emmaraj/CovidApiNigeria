using CovidApiNigeria.DbContexts;
using CovidApiNigeria.Services;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using System;
using System.IO;

namespace CovidApiNigeria {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {

            services.AddControllers();

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo {
                    Title = "CovidAPINigeria",
                    Version = "v1",
                    Description = "API for COVID 19 Data fetched from the NCDC website",
                });
                var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "CovidAPI.xml");
                c.IncludeXmlComments(filePath);
            });

            services.AddDbContext<DatabaseContext>(opt => opt.UseSqlServer(
                Configuration.GetConnectionString("connectString")
                ));
            services.AddScoped<IDataService, DataService>();

            //Add Hangfire
            services.AddHangfire(options => {
                options.UseSqlServerStorage(Configuration.GetConnectionString("connectString"));
                JobStorage.Current = new SqlServerStorage(Configuration.GetConnectionString("connectString"));
            }
            );

            services.AddHangfireServer();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDataService dataService) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CovidApiNigeria v1"));
            app.UseHttpsRedirection();
            app.UseHangfireDashboard();
            //set up recurring job to fecth data
            RecurringJob.AddOrUpdate(() => dataService.ScrapeData(), "30 12 * * *", TimeZoneInfo.Local);
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
