using ADNMutante.Dominio.Context;
using ADNMutante.Dominio.Contracts;
using ADNMutante.Dominio.Entities;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace ADNMutante.Dominio.Implementation
{
    public class ADNMutanteRepository : Repository<ADNMutanteDB> , IADNMutanteRepository
    {
        private readonly ILogger _logger;
        public ADNMutanteRepository(ADNMutanteDbContext dbContext, ILogger<ADNMutanteRepository> logger) : base(dbContext)
        {
            _logger = logger;
        }
        public long CountMutants(bool isMutant)
        {
            return DbContext.ADNs.Count(x => x.IsMutant == isMutant);
        }

    }
}
