using System;
using System.ComponentModel.DataAnnotations;

namespace ADNMutante.Models
{
    public class ADN : ValidatableADN
    {
        [RegularExpression("[ACTG]",
         ErrorMessage = "La muestra de ADN proporcionada no es válida")]
        public String[] dna { get; set; }
        public bool? IsMutant { get; set; }
    }
}
