using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ADNMutante.Models
{
    public class ValidatableADN : IValidatableObject
    {
        public String[] dna { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var anchoFila = dna.Length;
            foreach (var fila in dna)
            {
                if (fila.Length != anchoFila) { 
                    yield return new ValidationResult(
            $"La muestra de ADN proporcionada no es válida",
            new[] { nameof(dna) });
                    break;
                }
            }

        }
    }
}
