using Microsoft.EntityFrameworkCore;
using FoodBridge.Models;

namespace FoodBridge.Data
{
    public class FoodBridgeDbContext : DbContext
    {
        public FoodBridgeDbContext(DbContextOptions<FoodBridgeDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ItemType> ItemTypes { get; set; }
        public DbSet<Suburb> Suburbs { get; set; }
        public DbSet<Storage> Storages { get; set; }
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<Donations> Donations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FoodItem>()
                .HasOne(f => f.PostedByUser)
                .WithMany()
                .HasForeignKey(f => f.PostedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FoodItem>()
                .HasOne(f => f.ClaimedByUser)
                .WithMany()
                .HasForeignKey(f => f.ClaimedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FoodItem>()
                .HasOne(f => f.Category)
                .WithMany()
                .HasForeignKey(f => f.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FoodItem>()
                .HasOne(f => f.ItemType)
                .WithMany()
                .HasForeignKey(f => f.ItemTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FoodItem>()
                .HasOne(f => f.Suburb)
                .WithMany()
                .HasForeignKey(f => f.SuburbId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FoodItem>()
                .HasOne(f => f.Storage)
                .WithMany()
                .HasForeignKey(f => f.StorageId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
