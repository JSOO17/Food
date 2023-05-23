using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Foods.Infrastructure.Data.Models
{
    public partial class foodContext : DbContext
    {
        public foodContext()
        {
        }

        public foodContext(DbContextOptions<foodContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Dish> Dishes { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<Orderdish> Orderdishes { get; set; } = null!;
        public virtual DbSet<Restaurant> Restaurants { get; set; } = null!;
        public virtual DbSet<Restaurantemployee> Restaurantemployees { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=localhost;port=3306;database=food;uid=root", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.4.28-mariadb"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("categories");

                entity.Property(e => e.Id).HasColumnType("bigint(20)");

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Dish>(entity =>
            {
                entity.ToTable("dishes");

                entity.HasIndex(e => e.CategoryId, "dishes_FK");

                entity.HasIndex(e => e.RestaurantId, "dishes_FK_1");

                entity.Property(e => e.Id).HasColumnType("bigint(20)");

                entity.Property(e => e.CategoryId).HasColumnType("bigint(20)");

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.IsActive).HasColumnType("bit(1)");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Price).HasPrecision(10);

                entity.Property(e => e.RestaurantId).HasColumnType("bigint(20)");

                entity.Property(e => e.UrlImagen).HasMaxLength(700);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Dishes)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("dishes_FK");

                entity.HasOne(d => d.Restaurant)
                    .WithMany(p => p.Dishes)
                    .HasForeignKey(d => d.RestaurantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("dishes_FK_1");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("orders");

                entity.HasIndex(e => e.RestaurantId, "orders_FK");

                entity.HasIndex(e => e.ChefId, "orders_FK_1");

                entity.Property(e => e.Id).HasColumnType("bigint(20)");

                entity.Property(e => e.ChefId).HasColumnType("bigint(20)");

                entity.Property(e => e.ClientId).HasColumnType("bigint(20)");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.RestaurantId).HasColumnType("bigint(20)");

                entity.Property(e => e.State).HasMaxLength(100);

                entity.HasOne(d => d.Chef)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ChefId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("orders_FK_1");

                entity.HasOne(d => d.Restaurant)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.RestaurantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("orders_FK");
            });

            modelBuilder.Entity<Orderdish>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("orderdishes");

                entity.HasIndex(e => e.DishId, "orderdishes_DishId_IDX");

                entity.HasIndex(e => e.OrderId, "orderdishes_OrderId_IDX");

                entity.HasIndex(e => new { e.OrderId, e.DishId }, "orderdishes_un")
                    .IsUnique();

                entity.Property(e => e.Cuantity).HasColumnType("bigint(20)");

                entity.Property(e => e.DishId).HasColumnType("bigint(20)");

                entity.Property(e => e.OrderId).HasColumnType("int(11)");
            });

            modelBuilder.Entity<Restaurant>(entity =>
            {
                entity.ToTable("restaurants");

                entity.Property(e => e.Id).HasColumnType("bigint(20)");

                entity.Property(e => e.Address).HasMaxLength(200);

                entity.Property(e => e.Cellphone).HasMaxLength(13);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.OwnerId).HasColumnType("bigint(20)");

                entity.Property(e => e.UrlLogo).HasMaxLength(700);
            });

            modelBuilder.Entity<Restaurantemployee>(entity =>
            {
                entity.ToTable("restaurantemployee");

                entity.HasIndex(e => e.RestaurantId, "RestaurantEmployee_FK");

                entity.HasIndex(e => e.EmployeeId, "RestaurantEmployee_FK_1");

                entity.Property(e => e.Id).HasColumnType("bigint(20)");

                entity.Property(e => e.EmployeeId).HasColumnType("bigint(20)");

                entity.Property(e => e.RestaurantId).HasColumnType("bigint(20)");

                entity.HasOne(d => d.Restaurant)
                    .WithMany(p => p.Restaurantemployees)
                    .HasForeignKey(d => d.RestaurantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("RestaurantEmployee_FK");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
