using System;
using System.Threading.Tasks;
using ADNMutante.Dominio.Contracts;
using ADNMutante.Dominio.Entities;
using ADNMutante.Servicios.Contracts;
using Microsoft.Extensions.Logging;

namespace ADNMutante.Servicios.Services
{
    public class ADNMutanteService : IADNMutanteService
    {
        protected readonly IADNMutanteRepository _adnMutanteRepositorio;
        protected readonly ILogger<ADNMutanteService> Logger;

        public ADNMutanteService(IADNMutanteRepository adnMutanteRepositorio, ILogger<ADNMutanteService> logger)

        {
            Logger = logger;
            _adnMutanteRepositorio = adnMutanteRepositorio;
        }

        public bool IsMutant(String[] dna)
        {
            bool esMutante = false;
            int filas = dna.Length;
            int columnas = dna[0].Length;
            bool posibleMutante = false;
            int cadenaMutante = 0;

            for (int fila = 0; fila < filas; fila++)
            {
                #region Coincidencia Horizontal
                if (dna[fila].Contains("AAAA")) cadenaMutante += 1;
                if (dna[fila].Contains("CCCC")) cadenaMutante += 1;
                if (dna[fila].Contains("TTTT")) cadenaMutante += 1;
                if (dna[fila].Contains("GGGG")) cadenaMutante += 1;
                if (cadenaMutante >= 2)
                {
                    esMutante = true;
                }
                #endregion
                for (int columnaH = 0; columnaH < columnas; columnaH++)
                {
                    #region Coincidencia Vertical
                    if (fila < filas - 3)
                    {
                        posibleMutante = dna[fila][columnaH].Equals(dna[fila + 3][columnaH])
                                        && dna[fila][columnaH].Equals(dna[fila + 1][columnaH])
                                        && dna[fila][columnaH].Equals(dna[fila + 2][columnaH]);

                        if (posibleMutante)
                        {
                            cadenaMutante += 1;
                        }
                        if (cadenaMutante >= 2)
                        {
                            esMutante = true;
                            return esMutante;
                        }

                    }
                    #endregion
                    #region Coincidencia oblicua
                    if (fila < filas - 3 && columnaH < columnas - 3)
                    {
                        posibleMutante = dna[fila][columnaH].Equals(dna[fila + 3][columnaH + 3])
                                        && dna[fila][columnaH].Equals(dna[fila + 1][columnaH + 1])
                                        && dna[fila][columnaH].Equals(dna[fila + 2][columnaH + 2]);

                        if (posibleMutante)
                        {
                            cadenaMutante += 1;
                        }
                        if (cadenaMutante >= 2)
                        {
                            esMutante = true;
                            return esMutante;
                        }
                    }
                    #endregion
                    #region Coincidencia oblicua inversa
                    if (fila < filas - 3 && columnaH >= 3)

                    {

                        posibleMutante = dna[fila][columnaH].Equals(dna[fila + 3][columnaH - 3])
                                            && dna[fila][columnaH].Equals(dna[fila + 1][columnaH - 1])
                                            && dna[fila][columnaH].Equals(dna[fila + 2][columnaH - 2]);
                        if (posibleMutante) { cadenaMutante += 1; }
                        if (cadenaMutante >= 2)
                        {
                            esMutante = true;
                            return esMutante;
                        }
                    }
                    #endregion
                }
            }
            return esMutante;
        }

        public long CantidadMutantes()
        {
            return _adnMutanteRepositorio.CountMutants(true);
        }
        public long CantidadHumanos()
        {
            return _adnMutanteRepositorio.CountMutants(false);
        }
        public double Ratio()
        {
            long mutantes = CantidadMutantes();
            long humanos = CantidadHumanos();
            if (mutantes == 0 || humanos == 0)
            {
                Logger.LogWarning("No se puede calcular el ratio ya que uno de los valores es 0. Cantidad de humanos={humanos} - Cantidad de mutantes={mutantes}", humanos, mutantes);
                return 0;
            }
            return (double)mutantes / (double)humanos;
        }

        public async Task SaveMutant(String[] dna, bool isMutant)
        {
            try
            {
                var nuevoMutante = new ADNMutanteDB()
                {
                    CadenaADN = "",
                    IsMutant = isMutant
                };
                foreach (var adn in dna)
                {
                    nuevoMutante.CadenaADN = nuevoMutante.CadenaADN + " " + adn;
                }
                if (!_adnMutanteRepositorio.AdnRegistrado(nuevoMutante.CadenaADN))
                {
                    _adnMutanteRepositorio.Add(nuevoMutante);
                    _adnMutanteRepositorio.SaveChangesAsync();
                }
                else
                {
                    Logger.LogError("La cadena {nuevoMutante.CadenaADN} ya se encontraba guardada en la base de datos.", nuevoMutante.CadenaADN);
                }
            }
            catch (Exception ex)
            {
                var exception = new Exception("Ocurrió un error al editar el item", ex);
                Logger.LogError(ex, "No se pudo guardar la cadena {dna}", dna);
            }
        }
        public bool isMutantParallel(string[] dna)
        {
            bool esMutante = false;
            int filas = dna.Length;
            int columnas = dna[0].Length;
            bool posibleMutante = false;
            int cadenaMutante = 0;

            Parallel.For(0, filas, (int fila, ParallelLoopState stateFila) =>
            {
                #region (hilos) coincidencia horizontal
                if (dna[fila].Contains("AAAA")) cadenaMutante += 1;
                if (dna[fila].Contains("CCCC")) cadenaMutante += 1;
                if (dna[fila].Contains("TTTT")) cadenaMutante += 1;
                if (dna[fila].Contains("GGGG")) cadenaMutante += 1;
                if (cadenaMutante >= 2)
                {
                    esMutante = true;
                    stateFila.Stop();
                }
                #endregion
                Parallel.For(0, columnas, (int columnaH, ParallelLoopState state) =>
                {

                    //coincidencia horizontal
                    //if (columnaH < columnas - 3)

                    //{
                    //    posibleMutante = dna[fila][columnaH].Equals(dna[fila][columnaH + 3])
                    //                    && dna[fila][columnaH].Equals(dna[fila][columnaH + 1])
                    //                    && dna[fila][columnaH].Equals(dna[fila][columnaH + 2]);
                    //    //posibleMutante = dna[fila][columnaH] == dna[fila][columnaH + 3]
                    //    //            && dna[fila][columnaH] == dna[fila][columnaH + 1]
                    //    //            && dna[fila][columnaH] == dna[fila][columnaH + 2];
                    //    if (posibleMutante) { cadenaMutante += 1; }
                    //    if (cadenaMutante >= 2)
                    //    {
                    //        esMutante = true;
                    //        //state.Stop();
                    //        //return esMutante;
                    //    }
                    //}
                    #region (hilos) coincidencia vertical
                    if (fila < filas - 3)
                    {
                        posibleMutante = dna[fila][columnaH].Equals(dna[fila + 3][columnaH])
                                        && dna[fila][columnaH].Equals(dna[fila + 1][columnaH])
                                        && dna[fila][columnaH].Equals(dna[fila + 2][columnaH]);
                        if (posibleMutante)
                        {
                            cadenaMutante += 1;
                        }
                        if (cadenaMutante >= 2)
                        {
                            esMutante = true;
                            state.Stop();
                            //return esMutante;
                        }
                    }
                    #endregion

                    #region (hilos)coincidencia oblicua
                    if (fila < filas - 3 && columnaH < columnas - 3)
                    {
                        posibleMutante = dna[fila][columnaH].Equals(dna[fila + 3][columnaH + 3])
                                        && dna[fila][columnaH].Equals(dna[fila + 1][columnaH + 1])
                                        && dna[fila][columnaH].Equals(dna[fila + 2][columnaH + 2]);

                        if (posibleMutante)
                        {
                            cadenaMutante += 1;
                        }
                        if (cadenaMutante >= 2)
                        {
                            esMutante = true;
                            state.Stop();
                        }
                    }
                    #endregion
                    #region(hilos) coincidencia oblicua inversa 
                    //coincidencia oblicua inversa
                    if (fila < filas - 3 && columnaH >= 3)

                    {

                        posibleMutante = dna[fila][columnaH].Equals(dna[fila + 3][columnaH - 3])
                                            && dna[fila][columnaH].Equals(dna[fila + 1][columnaH - 1])
                                            && dna[fila][columnaH].Equals(dna[fila + 2][columnaH - 2]);
                        if (posibleMutante) { cadenaMutante += 1; }
                        if (cadenaMutante >= 2)
                        {
                            esMutante = true;
                            state.Stop();
                        }
                    }
                    #endregion

                });
                if (esMutante) stateFila.Stop();
            });
            return esMutante;
        }


    }
}
