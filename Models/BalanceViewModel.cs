using System.Collections.Generic;

namespace CelularesMarket.Models
{
    public class BalanceViewModel
    {
        public IEnumerable<Celular> Celulares { get; set; } = new List<Celular>();
        public IEnumerable<Venta> Ventas { get; set; } = new List<Venta>();
    }
}
