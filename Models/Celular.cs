using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CelularesMarket.Models
{
    public enum EstadoCelular
    {
        Nuevo,
        Usado,
        Reacondicionado
    }

    public class Celular
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Marca { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Modelo { get; set; } = string.Empty;

        [Required, StringLength(20, MinimumLength = 10)]
        [Display(Name = "IMEI")]
        public string IMEI { get; set; } = string.Empty;

        [Range(0, 100000000)]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Precio")]
        public decimal Precio { get; set; }

        [Display(Name = "Estado")]
        public EstadoCelular Estado { get; set; } = EstadoCelular.Usado;

        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        [Display(Name = "En venta")]
        public bool EnVenta { get; set; } = true;

        [Display(Name = "Publicado")]
        [DataType(DataType.Date)]
        public DateTime FechaPublicacion { get; set; } = DateTime.UtcNow;

    [Url]
    [Display(Name = "Imagen (URL)")]
    public string? ImagenUrl { get; set; } // No Required, es opcional
    }
}
