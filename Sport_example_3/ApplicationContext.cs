using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Sport_example_3
{
    internal class ApplicationContext:DbContext
    {
        public DbSet<Models.PositionEmployee> Positions { get; set; } = null!;
        public DbSet<Models.Employee> Employees { get; set; } = null!;
        public DbSet<Models.ProductCategory> Categories { get; set; } = null!;
        public DbSet<Models.Product> Products { get; set; } = null!;
        public DbSet<Models.Supplier> Suppliers { get; set; } = null!;
        public DbSet<Models.ProductPrice> ProductPrices { get; set; } = null!;
        public DbSet<Models.Receipt> Receipts { get; set; } = null!;
        public DbSet<Models.Order> Orders { get; set; } = null!;
        public DbSet<Models.UserRole> UserRoles { get; set; } = null!;
        public DbSet<Models.User> Users { get; set; } = null!;

        public ApplicationContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=SportShop.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            

        }
    }
}
