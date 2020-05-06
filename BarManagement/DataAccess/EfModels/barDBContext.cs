using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BarManagement.DataAccess.EfModels
{
    public partial class barDBContext : DbContext
    {
        public barDBContext()
        {
        }

        public barDBContext(DbContextOptions<barDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cocktails> Cocktails { get; set; }
        public virtual DbSet<CocktailsComposition> CocktailsComposition { get; set; }
        public virtual DbSet<Drinks> Drinks { get; set; }
        public virtual DbSet<Stocks> Stocks { get; set; }
        public virtual DbSet<Transactions> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=.\\;Database=barDB;Trusted_Connection=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cocktails>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Cost).HasColumnName("cost");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PriceToSell).HasColumnName("price_to_sell");
                entity.Property(e => e.CocktailCategory)
                    .HasColumnName("cocktailCategory")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CocktailsComposition>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CocktailId).HasColumnName("cocktail_id");

                entity.Property(e => e.DrinkId).HasColumnName("drink_id");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Cocktail)
                    .WithMany()
                    .HasForeignKey(d => d.CocktailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CocktailsComposition_Cocktails");

                entity.HasOne(d => d.Drink)
                    .WithMany()
                    .HasForeignKey(d => d.DrinkId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CocktailsComposition_Drinks");
            });

            modelBuilder.Entity<Drinks>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Brand)
                    .HasColumnName("brand")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Category)
                    .HasColumnName("category")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Stocks>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DrinkId).HasColumnName("drink_id");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Drink)
                    .WithMany(p => p.Stocks)
                    .HasForeignKey(d => d.DrinkId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Stocks_Drinks");
            });

            modelBuilder.Entity<Transactions>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CocktailId).HasColumnName("cocktail_id");

                entity.Property(e => e.SellDate)
                    .HasColumnName("sell_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Value).HasColumnName("value");

                entity.HasOne(d => d.Cocktail)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.CocktailId)
                    .HasConstraintName("FK_Transactions_Cocktails");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
