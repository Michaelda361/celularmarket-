using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CelularesMarket.Models
{
    public class Venta
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CelularId { get; set; }
        [ForeignKey("CelularId")]
        public Celular? Celular { get; set; }

        [Required]
        public string NombreCliente { get; set; } = string.Empty;

        [Required]
        public string DocumentoCliente { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioVenta { get; set; }

        [Required]
        public DateTime FechaVenta { get; set; } = DateTime.Now;
    }
}
