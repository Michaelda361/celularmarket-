using System;
using System.ComponentModel.DataAnnotations;

namespace CelularesMarket.Models
{
    public class FacturaViewModel
    {
        [Required]
    public required string NombreCliente { get; set; }
    [Required]
    public required string DocumentoCliente { get; set; }
        [Required]
        public int CelularId { get; set; }
        [Required]
        [Range(0, 100000000)]
        public decimal PrecioVenta { get; set; }
        [Required]
        public DateTime FechaVenta { get; set; } = DateTime.Now;
    }
}
