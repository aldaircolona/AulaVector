using AulaVector.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AulaVector.Data
{
    public class ApplicationDbContext
        : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Domain entities
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ==========================
            // PostgreSQL configuration
            // ==========================

            // Store UTC DateTime values as "timestamp with time zone" (timestamptz)
            builder.Entity<ApplicationUser>()
                .Property(u => u.RegistrationDate)
                .HasColumnType("timestamp with time zone");

            builder.Entity<Order>()
                .Property(o => o.CreatedAt)
                .HasColumnType("timestamp with time zone");

            // ==========================
            // Product
            // ==========================

            builder.Entity<Product>(entity =>
            {
                entity.ToTable("Products");

                entity.HasKey(p => p.Id);

                entity.Property(p => p.Title)
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(p => p.Author)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(p => p.Description)
                    .HasMaxLength(4000);

                entity.Property(p => p.Price)
                    .HasPrecision(10, 2);

                entity.Property(p => p.PdfFilePath)
                    .HasMaxLength(500);

                entity.Property(p => p.CoverImageUrl)
                    .HasMaxLength(500);
            });

            // ==========================
            // Order
            // ==========================

            builder.Entity<Order>(entity =>
            {
                entity.ToTable("Orders");

                entity.HasKey(o => o.Id);

                entity.Property(o => o.TotalAmount)
                    .HasPrecision(10, 2);

                entity.Property(o => o.PaymentStatus)
                    .HasMaxLength(50);

                entity.Property(o => o.PaymentMethod)
                    .HasMaxLength(100);

                entity.Property(o => o.TransactionId)
                    .HasMaxLength(200);

                // Relationship with Identity User
                entity.HasOne<ApplicationUser>()
                    .WithMany()
                    .HasForeignKey(o => o.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ==========================
            // OrderDetail
            // ==========================

            builder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("OrderDetails");

                entity.HasKey(od => od.Id);

                entity.Property(od => od.UnitPrice)
                    .HasPrecision(10, 2);

                entity.HasOne(od => od.Order)
                    .WithMany(o => o.OrderDetails)
                    .HasForeignKey(od => od.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(od => od.Product)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(od => od.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}