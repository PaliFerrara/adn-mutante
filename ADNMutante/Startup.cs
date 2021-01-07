using ADNMutante.Dominio.Context;
using ADNMutante.Dominio.Contracts;
using ADNMutante.Dominio.Implementation;
using ADNMutante.Servicios.Contracts;
using ADNMutante.Servicios.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;

namespace ADNMutante
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(builder => builder.ClearProviders().AddEventSourceLogger());
            services.AddDbContext<ADNMutanteDbContext>(options => options.UseSqlite(Configuration.GetConnectionString("AdnMutanteBase")));
            services.AddControllers().AddNewtonsoftJson();
            AddSwagger(services);
            services.AddOptions();
            services.AddHttpClient();
            services.Configure<ADNMutanteDbContext>(Configuration);
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IADNMutanteRepository, ADNMutanteRepository>();
            services.AddScoped<IADNMutanteService, ADNMutanteService>();

        }
        private void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                var groupName = "v1";

                options.SwaggerDoc(groupName, new OpenApiInfo
                {
                    Title = $"ADN Mutante {groupName}",
                    Version = groupName,
                    Description = "ADN Mutante API",
                    Contact = new OpenApiContact
                    {
                        Name = "Paola Ferrara",
                        Email = "pao.ferrara@live.com" 
                    }
                });
            });
        }
            // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ADN Mutante API V1");
            });
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
