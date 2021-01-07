using System;
using System.Threading.Tasks;

namespace ADNMutante.Servicios.Contracts
{
    public interface IADNMutanteService
    {
        bool IsMutant(String[] dna);
        long CantidadMutantes();
        long CantidadHumanos();
        double Ratio();
        Task SaveMutant(string[] dna, bool isMutant);
        bool isMutantParallel(string[] dna);
    }
}
