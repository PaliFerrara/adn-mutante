using System;
using System.ComponentModel.DataAnnotations;

namespace ADNMutante.Models
{
    public class ADN : ValidatableADN
    {
        public bool? IsMutant { get; set; }
    }
}
