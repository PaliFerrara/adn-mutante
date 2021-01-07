using System.IO;
using System.Linq;
using ADNMutante.Dominio.Context;
using ADNMutante.Dominio.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ADNMutante
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string dbName = "ADNMutanteDatabase.db";
            if (File.Exists(dbName))
            {
                File.Delete(dbName);
            }
            var optionsBuilder = new DbContextOptionsBuilder<ADNMutanteDbContext>();

            using (var dbContext = new ADNMutanteDbContext(optionsBuilder.Options))
            {
                dbContext.Database.EnsureCreated();
                if (!dbContext.ADNs.Any())
                {
                    dbContext.ADNs.AddRange(new ADNMutanteDB[]
                        {
                             new ADNMutanteDB{ Id=1, CadenaADN="ATGCGA CAGTGC TTATGT AGAAGG CCCCTA TCACTG", IsMutant=true },
                             new ADNMutanteDB{ Id=2, CadenaADN="ATTCGA CAGTGC TTTCTC AGAAGT CCACTA TCACTG", IsMutant=false },
                             new ADNMutanteDB{ Id=3, CadenaADN="ATTCGA CAGTGC TTATTT AGAAGT CCACTA TCACTG", IsMutant=false }
                        });
                    dbContext.SaveChanges();
                }
            }
            CreateHostBuilder(args).Build().Run();

        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
