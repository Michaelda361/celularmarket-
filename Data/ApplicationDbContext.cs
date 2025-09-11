using Microsoft.EntityFrameworkCore;
using CelularesMarket.Models;

namespace CelularesMarket.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

    public DbSet<Celular> Celulares { get; set; }
    public DbSet<Venta> Ventas { get; set; }
    }
}
