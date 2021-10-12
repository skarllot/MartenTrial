using Marten;
using MartenTrial.Common.MediatR;
using MartenTrial.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Weasel.Postgresql;

namespace MartenTrial.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostEnvironment hosting)
        {
            Configuration = configuration;
            Hosting = hosting;
        }

        public IConfiguration Configuration { get; }
        public IHostEnvironment Hosting { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MartenTrial.Web", Version = "v1" });
            });

            services.AddMarten(options =>
            {
                options.Connection(Configuration.GetConnectionString("postgres"));

                if (Hosting.IsDevelopment())
                {
                    options.AutoCreateSchemaObjects = AutoCreate.All;
                    options.DatabaseSchemaName = "projection";
                    options.Events.DatabaseSchemaName = "stream";
                    options.Events.MetadataConfig.CausationIdEnabled = true;
                    options.Events.MetadataConfig.CorrelationIdEnabled = true;
                }
            });

            services
                .AddMediatR()
                .AddQuestHandlers()
                .AddEventHandlers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (Hosting.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MartenTrial.Web v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}