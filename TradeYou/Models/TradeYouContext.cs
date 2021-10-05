using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace TradeYou.Models
{
    public partial class TradeYouContext : DbContext
    {
        public TradeYouContext()
        {
        }

        public TradeYouContext(DbContextOptions<TradeYouContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OId)
                    .HasName("PK__Order__5AAB0C383D7196D4");

                entity.ToTable("Order");

                entity.Property(e => e.OId).HasColumnName("O_Id");

                entity.Property(e => e.OOrderumber)
                    .HasColumnType("datetime")
                    .HasColumnName("O_Orderumber");

                entity.Property(e => e.OPaymentype).HasColumnName("O_Paymentype");

                entity.Property(e => e.OQuantity).HasColumnName("O_Quantity");

                entity.Property(e => e.OShippingtype).HasColumnName("O_Shippingtype");

                entity.Property(e => e.PId).HasColumnName("P_Id");

                entity.Property(e => e.UId).HasColumnName("U_Id");

                entity.HasOne(d => d.PIdNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.PId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductId");

                entity.HasOne(d => d.UIdNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.UId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserId");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.PId)
                    .HasName("PK__tmp_ms_x__A3420A57D83EB593");

                entity.ToTable("Product");

                entity.Property(e => e.PId).HasColumnName("P_Id");

                entity.Property(e => e.PImagePath)
                    .IsUnicode(false)
                    .HasColumnName("P_ImagePath");

                entity.Property(e => e.PMade)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("P_Made");

                entity.Property(e => e.PNewUsed).HasColumnName("P_NewUsed");

                entity.Property(e => e.PPrice).HasColumnName("P_Price");

                entity.Property(e => e.PProductname)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("P_Productname");

                entity.Property(e => e.PQuantity).HasColumnName("P_Quantity");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UId)
                    .HasName("PK_Table");

                entity.ToTable("User");

                entity.Property(e => e.UId).HasColumnName("U_Id");

                entity.Property(e => e.UDob)
                    .HasColumnType("datetime")
                    .HasColumnName("U_DOB");

                entity.Property(e => e.UEmail)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("U_Email");

                entity.Property(e => e.UIsAdmin).HasColumnName("U_IsAdmin");

                entity.Property(e => e.UName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("U_Name");

                entity.Property(e => e.UPassword)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("U_Password");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
