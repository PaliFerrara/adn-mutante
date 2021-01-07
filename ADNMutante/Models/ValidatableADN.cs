using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ADNMutante.Models
{
    public class ValidatableADN : IValidatableObject
    {
    //    [RegularExpression("^[A|C|T|G]*$",
    //ErrorMessage = "La muestra de ADN proporcionada no es válida")]

        public String[] dna { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var anchoFila = dna[0].Length;
            var altoColumna = dna.Length;
            var regex = "^[A|C|T|G]*$";

            if (altoColumna < 4 && anchoFila < 4)
            {
                yield return new ValidationResult($"La muestra de ADN proporcionada no es válida.",new[] { nameof(dna) });

            }
            foreach (var fila in dna)
            {
                if (fila.Length != anchoFila )
                {
                    yield return new ValidationResult( $"La muestra de ADN proporcionada no es válida.", new[] { nameof(dna) });
                    break;
                }
            }

        }
    }
}
