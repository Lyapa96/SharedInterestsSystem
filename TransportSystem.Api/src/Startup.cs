using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;
using Swashbuckle.AspNetCore.Swagger;
using TransportSystem.Api.Models.Neighbors;
using TransportSystem.Api.Models.PassengerBehaviour;
using TransportSystem.Api.Models.System;
using TransportSystem.Api.Models.TransportChooseAlgorithms.QLearning.Storage;
using TransportSystem.Api.Models.TransportSystemSatisfaction;
using TransportSystem.Api.Utilities;

namespace TransportSystem.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ITransportSystem, Models.System.TransportSystem>();
            services.AddSingleton<IAgentStateStorage, MemoryStorage>();
            services.AddSingleton<IRandomizer, DefaultRandomizer>();
            services.AddSingleton<IPassengersFactory, PassengersFactory>();
            services.AddSingleton<INeighborsManager, NeighborsManager>();
            services.AddSingleton<IPassengerBehaviourProvider, PassengerBehaviourProvider>();
            services.AddSingleton<ITransportSystemSatisfaction, AverageTransportSystemSatisfaction>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Transport system API", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options => options.SerializerSettings.Converters.Add(new StringEnumConverter()));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseCors(
                builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Transport system API V1");
                c.RoutePrefix = string.Empty;
            });


            app.UseMvc();
        }
    }
}