using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    modelBuilder.Entity<Product>()
    .HasOne(p => p.Category)
    .WithMany(c => c.Products)
    .HasForeignKey(p => p.CategoryId);
    
    modelBuilder.Entity<Order>()
    .HasOne(o => o.CreatedBy)
    .WithMany() // or WithMany(u => u.OrdersCreated) if you have reverse nav
    .HasForeignKey(o => o.CreatedByUserId);


    modelBuilder.Entity<OrderItem>()
    .HasOne(oi => oi.Product)
    .WithMany()
    .HasForeignKey(oi => oi.ProductId);
    
        // Pre-hashed passwords (generated once)
    string hash1 = "$2a$11$u1eFSaxfFdHZzHlKpAZOnOOymz0Y5TGRH3XyxRZFbJQj9q29D6pHe"; // hash for "1"
        // Seed Users
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Username = "admin" , PasswordHash = hash1 }
        );

        // Seed Categories
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Electronics" },
            new Category { Id = 2, Name = "Books" }
        );
    }
}