using Microsoft.EntityFrameworkCore;
using OnSale.Common.Entities;

namespace OnSale.Web.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        //mapeo a la bd
        public DbSet<Country> Countries { get; set; }
        //este metodo crea un indice para que no haya 2 paises con el mismo nombre
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Country>()
                .HasIndex(t => t.Name)
                .IsUnique();
        }
    }

}
