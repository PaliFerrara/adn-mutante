using ADNMutante.Dominio.Entities;

namespace ADNMutante.Dominio.Contracts
{
    public interface IADNMutanteRepository : IRepository<ADNMutanteDB>
    {
        long CountMutants(bool isMutant);
    }
}
