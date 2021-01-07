using System;
using System.ComponentModel.DataAnnotations;

namespace ADNMutante.Dominio.Entities
{
    /// <summary>
    /// ADNMutante entity
    /// </summary>
    public class ADNMutanteDB
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public String CadenaADN { get; set; }
        [Required]
        public bool IsMutant { get; set; }
    }
}
