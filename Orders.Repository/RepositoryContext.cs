using Microsoft.EntityFrameworkCore;
using Orders.Entities.Models;
using System.Reflection;

namespace Orders.Repository
{
    public class RepositoryContext : DbContext
    {
       

        public DbSet<Product>? Products { get; set; } 
        public DbSet<Order>? Orders { get; set; }
        public DbSet<OrderItem>? OrderItems { get; set; }
        public DbSet<Customer>? Customers { get; set; }
        public DbSet<PaymentReference>? PaymentReferences { get; set; }
        
        public RepositoryContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        	
	       	        	        
        }
        
        
    }
}
