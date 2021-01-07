using System;
using ADNMutante.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using ADNMutante.Servicios.Contracts;

namespace ADNMutante.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ADNMutanteController : ControllerBase
    {
        //harcodeado para probar
        public String[] dnaEjemplo = { "ATGCGA", "CAGTGC", "TTATGT", "AGAAGG", "CCCCTA", "TCACTG" };
        private readonly IADNMutanteService _adnMutanteService;
        private readonly ILogger<ADNMutanteController> _logger;

        public ADNMutanteController(ILogger<ADNMutanteController> logger,IADNMutanteService adnMutanteService)
        {
            _adnMutanteService = adnMutanteService;
            _logger = logger;
        }
        [HttpPost]
        [Route("/mutant/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult Mutant([FromBody] ADN adn)
        {
            adn.IsMutant = _adnMutanteService.IsMutant(adn.dna);
            return adn.IsMutant.Value ? Ok() : StatusCode(403);
        }
        [HttpPost]
        [Route("/mutant2/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]

        public IActionResult Mutant2([FromBody] ADN adn)
        {
            bool mutante = _adnMutanteService.isMutantParallel(adn.dna);
            return mutante ? StatusCode(200) : StatusCode(403);
        }

        [HttpGet]
        [Route("/stats")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Stats()
        {
            var result = new
            {
                count_mutant_dna = _adnMutanteService.CantidadMutantes(),
                count_human_dna= _adnMutanteService.CantidadHumanos(),
                ratio=_adnMutanteService.Ratio()
            };
            return Ok(result);
        }

        [HttpGet]
        public IActionResult Index()
        {
            var esMutante = _adnMutanteService.IsMutant(dnaEjemplo);
            return Ok(esMutante);
        }

    }
}
