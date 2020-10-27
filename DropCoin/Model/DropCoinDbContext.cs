using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DropCoin.Model
{
    public partial class DropCoinDbContext : DbContext
    {
        public DropCoinDbContext()
        {
        }

        public DropCoinDbContext(DbContextOptions<DropCoinDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Transactions> Transactions { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=192.168.1.58, 1433;Database=DropCoinDb;User Id=sa;Password=test;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transactions>(entity =>
            {
                entity.HasKey(e => e.TransactionId)
                    .HasName("Transactions_pkey");

                entity.HasIndex(e => e.From)
                    .HasName("fki_FromUser");

                entity.HasIndex(e => e.To)
                    .HasName("fki_ToUser");

                entity.Property(e => e.TransactionDate).HasColumnType("date");

                entity.Property(e => e.TransactionHash)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.FromNavigation)
                    .WithMany(p => p.TransactionsFromNavigation)
                    .HasForeignKey(d => d.From)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FromUser");

                entity.HasOne(d => d.ToNavigation)
                    .WithMany(p => p.TransactionsToNavigation)
                    .HasForeignKey(d => d.To)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ToUser");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("Users_pkey");

                entity.Property(e => e.DrpAddress)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Password).HasMaxLength(200);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
