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
        public DbSet<Category> Categories { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<ProductImage> ProductImages { get; set; }


        //este metodo crea un indice para que no haya 2 paises con el mismo nombre
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //para las categorias
            modelBuilder.Entity<Category>()
                .HasIndex(t => t.Name)
                .IsUnique();

            //para las ciudades
            modelBuilder.Entity<City>()
                .HasIndex(t => t.Name)
                .IsUnique();
            //para los paises
            modelBuilder.Entity<Country>()
                .HasIndex(t => t.Name)
                .IsUnique();
            //para los departamentos
            modelBuilder.Entity<Department>()
                .HasIndex(t => t.Name)
                .IsUnique();
            //productos
            modelBuilder.Entity<Product>()
                .HasIndex(t => t.Name)
                .IsUnique();


        }
    }

}
