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
            services.AddOptions();
            services.AddHttpClient();
            services.Configure<ADNMutanteDbContext>(Configuration);
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IADNMutanteRepository, ADNMutanteRepository>();
            services.AddScoped<IADNMutanteService, ADNMutanteService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
