using CommerceHub.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace CommerceHub.Web.Data
{
    public class CommerceDbContext : DbContext
    {
        //Nereye bağlanacağım?
        //Tablolar ve ilişkileri nasıl olmalı?

        public CommerceDbContext(DbContextOptions<CommerceDbContext> options) : base(options)
        {

        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("bağlantı cümlesi");
        //}

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Category> Categories => Set<Category>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                        .HasOne(p=>p.Category)
                        .WithMany(c=>c.Products)
                        .HasForeignKey(p=>p.CategoryId)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Category>().HasData(new Category { Id = 1, Name = "Bilgisayar" });
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Kablosuz Klavye", Description = "Logitech Bluetooth", BasePrice = 2000, CategoryId = 1, DiscountRate = 0.25, IsOnSale = true, StockCount = 100,SKU="logi-keyb-1" },

                new Product { Id = 2, Name = "Kablolu Mouse", Description = "Gamer mouse", BasePrice = 250, CategoryId = 1, DiscountRate = 0, IsOnSale = false, StockCount = 100 },

                new Product { Id = 3, Name = "24'' monitör", Description = "MSI", BasePrice = 6000, CategoryId = 1, DiscountRate = 0.15, IsOnSale = true, StockCount = 100 }
                );

        }
    }
}
