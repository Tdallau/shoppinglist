using Microsoft.EntityFrameworkCore;
using my_shoppinglist_api.Models.Database;

namespace my_shoppinglist_api.Helpers.Context
{
  public class MainContext : DbContext
  {
    public DbSet<User> User { get; set; }
    public DbSet<UserToken> UserToken { get; set; }
    public DbSet<ShoppingGroup> ShoppingGroup { get; set; }
    public DbSet<ShoppingGroupUser> ShoppingGroupUser { get; set; }
    public DbSet<Shop> Shop { get; set; }
    public DbSet<GroceryItem> GroceryItem { get; set; }
    public DbSet<ShoppingSettings> ShoppingSetting { get; set; }

    public MainContext(DbContextOptions<MainContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      builder.Entity<User>()
          .HasIndex(u => u.Email)
          .IsUnique();
      builder.Entity<ShoppingGroup>()
        .HasOne(x => x.Owner)
        .WithMany(x => x.ShoppingGroups)
        .HasForeignKey(x => x.OwnerId);
      builder.Entity<ShoppingGroupUser>()
        .HasKey(x => new { x.UserId, x.ShoppingGroupId });
    }

  }
}