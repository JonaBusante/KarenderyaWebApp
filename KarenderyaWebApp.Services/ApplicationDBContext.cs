
using Microsoft.EntityFrameworkCore;
using KarenderyaWebApp.Model;

namespace KarenderyaWebApp.Services;

public class ApplicationDBContext : DbContext
{
    public ApplicationDBContext(
        DbContextOptions<ApplicationDBContext> options) : base(options)
    {
        this.Database.SetCommandTimeout(900);
    }

    public DbSet<USERS> USERS { get; set; }

    public DbSet<FoodItem> FoodItems { get; set; }

    public DbSet<Order> Orders { get; set; }

    public DbSet<OrderItem> OrderItems { get; set; }
}
