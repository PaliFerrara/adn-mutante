using ADNMutante.Dominio.Entities;

namespace ADNMutante.Dominio.Contracts
{
    public interface IADNMutanteRepository : IRepository<ADNMutanteDB>
    {
        bool AdnRegistrado(string cadenaADN);
        long CountMutants(bool isMutant);
    }
}
