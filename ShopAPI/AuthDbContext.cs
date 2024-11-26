using Microsoft.EntityFrameworkCore;
using ShopAPI.Models;
using System.Collections.Generic;

namespace ShopAPI
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

        public DbSet<User> users { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<Cart> carts { get; set; }
        public DbSet<CartItem> cart_items { get; set; }
        public DbSet<Order> orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");

                entity.HasKey(u => u.id)
                      .HasName("id");

                entity.Property(u => u.username)
                      .IsRequired()
                      .HasMaxLength(50)
                      .HasColumnName("username");

                entity.Property(u => u.email)
                      .IsRequired()
                      .HasMaxLength(100)
                      .HasColumnName("email");

                entity.Property(u => u.passwordHash)
                      .IsRequired()
                      .HasColumnName("passwordHash");

                entity.Property(u => u.role)
                      .IsRequired()
                      .HasColumnName("role");

                entity.Property(u => u.createdAt)
                      .IsRequired()
                      .HasColumnName("createdAt");

                entity.Property(u => u.lastLogin)
                       .HasDefaultValue(null)
                       .HasColumnName("lastLogin");

                entity.Property(u => u.isEnabled)
                       .IsRequired()
                       .HasColumnName("isEnabled");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Products");

                entity.HasKey(u => u.id)
                      .HasName("id");

                entity.Property(u => u.name)
                      .IsRequired()
                      .HasMaxLength(100)
                      .HasColumnName("name");

                entity.Property(u => u.image)
                      .IsRequired()
                      .HasMaxLength(50)
                      .HasColumnName("image");

                entity.Property(u => u.category)
                       .IsRequired()
                       .HasMaxLength(50)
                       .HasColumnName("category");

                entity.Property(u => u.description)
                      .IsRequired()
                      .HasColumnName("description");

                entity.Property(u => u.price)
                      .IsRequired()
                      .HasColumnName("price");

                entity.Property(u => u.isEnabled)
                      .IsRequired()
                      .HasColumnName("isEnabled");

                entity.Property(u => u.createdAt)
                       .IsRequired()
                       .HasColumnName("createdAt");
            });


            modelBuilder.Entity<Cart>(entity =>
            {
                entity.ToTable("carts");

                entity.HasKey(u => u.id)
                      .HasName("id");

                entity.Property(u => u.userId)
                      .IsRequired()
                      .HasColumnName("userId");

                entity.Property(u => u.isActive)
                      .IsRequired()
                      .HasColumnName("isActive");

                entity.Property(u => u.createdAt)
                       .IsRequired()
                       .HasColumnName("createdAt");
            });

            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.ToTable("cart_items");

                entity.HasKey(u => u.id)
                      .HasName("id");

                entity.Property(u => u.cartId)
                      .IsRequired()
                      .HasColumnName("cartId");

                entity.Property(u => u.productId)
                      .IsRequired()
                      .HasColumnName("productId");

                entity.Property(u => u.quantity)
                      .IsRequired()
                      .HasColumnName("quantity");

                entity.Property(u => u.updatedAt)
                       .IsRequired()
                       .HasColumnName("updatedAt");

                entity.Property(u => u.createdAt)
                       .IsRequired()
                       .HasColumnName("createdAt");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("orders");

                entity.HasKey(u => u.id)
                      .HasName("id");

                entity.Property(u => u.userId)
                      .IsRequired()
                      .HasColumnName("userId");

                entity.Property(u => u.cartId)
                        .IsRequired()
                        .HasColumnName("cartId");

                entity.Property(u => u.totalAmount)
                       .IsRequired()
                       .HasColumnName("totalAmount");

                entity.Property(u => u.createdAt)
                       .IsRequired()
                       .HasColumnName("createdAt");
            });
        }
    }
}
