
using Delivora.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;


namespace Delivora.Repositories.Data;

// Add-Migration init -o "Repositories/Data/Migrations"
public class DeliveryContext : IdentityDbContext<AppUser, IdentityRole<int>, int>
{
    #region Users
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<Driver> Drivers { get; set; }
    #endregion

    public DbSet<Address> Addresses { get; set; }


    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    


    public DbSet<Food> Foods { get; set; }
    public DbSet<Category> Categories { get; set; }



    public DbSet<Payment> Payments { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }


    public DbSet<OrderReview> OrderReviews { get; set; }
    public DbSet<FoodReview> FoodReviews { get; set; }



    public DeliveryContext(DbContextOptions<DeliveryContext> options) : base(options)
    {
    }



    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Customer 1 : 1 AppUser
        builder.Entity<Customer>()
            .HasOne(c => c.User)
            .WithOne(u => u.Customer)
            .HasForeignKey<Customer>(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);


        // Driver 1 : 1 AppUser
        builder.Entity<Driver>()
            .HasOne(d => d.User)
            .WithOne(u => u.Driver)
            .HasForeignKey<Driver>(d => d.UserId)
            .OnDelete(DeleteBehavior.Cascade);


        // Admin 1 : 1 AppUser
        builder.Entity<Admin>()
            .HasOne(a => a.User)
            .WithOne(u => u.Admin)
            .HasForeignKey<Admin>(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);


        // Order 1 : 1 Payment
        builder.Entity<Order>()
            .HasOne(o => o.Payment)
            .WithOne(p => p.Order)
            .HasForeignKey<Payment>(p => p.OrderId)
            .OnDelete(DeleteBehavior.Restrict);



        // Cascade paths on Orders
        builder.Entity<Order>()
            .HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Order>()
            .HasOne(o => o.Driver)
            .WithMany(d => d.Orders)
            .HasForeignKey(o => o.DriverId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Order>()
            .HasOne(o => o.Address)
            .WithMany(a => a.Orders)
            .HasForeignKey(o => o.AddressId)
            .OnDelete(DeleteBehavior.Restrict);


        // Handle decimal precision
        builder.Entity<Driver>().Property(d => d.TotalEarnings).HasPrecision(18, 2);
        builder.Entity<Food>().Property(f => f.Price).HasPrecision(18, 2);
        builder.Entity<Order>().Property(o => o.DeliveryFee).HasPrecision(18, 2);
        builder.Entity<Order>().Property(o => o.SubTotal).HasPrecision(18, 2);
        builder.Entity<Order>().Property(o => o.Tax).HasPrecision(18, 2);
        builder.Entity<Order>().Property(o => o.Total).HasPrecision(18, 2);
        builder.Entity<OrderItem>().Property(oi => oi.TotalPrice).HasPrecision(18, 2);
        builder.Entity<OrderItem>().Property(oi => oi.UnitPrice).HasPrecision(18, 2);
        builder.Entity<Payment>().Property(p => p.Amount).HasPrecision(18, 2);



    }

}
