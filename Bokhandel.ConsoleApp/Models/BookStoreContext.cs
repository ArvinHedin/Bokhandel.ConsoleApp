using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Bokhandel.ConsoleApp.Models;

public partial class BookStoreContext : DbContext
{
    public BookStoreContext()
    {
    }

    public BookStoreContext(DbContextOptions<BookStoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Accessory> Accessories { get; set; }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<InverntoryBalance> InverntoryBalances { get; set; }

    public virtual DbSet<OrderLog> OrderLogs { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    public virtual DbSet<TitlePerAuthor> TitlePerAuthors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=BookStore;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Accessory>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Store).WithMany(p => p.Accessories)
                .HasForeignKey(d => d.StoreId)
                .HasConstraintName("FK_Accessories_Stores1");
        });

        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Authors__3214EC0796E1DD2B");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.LastName).HasMaxLength(255);
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Isbn);

            entity.Property(e => e.Isbn)
                .HasMaxLength(13)
                .HasColumnName("ISBN");
            entity.Property(e => e.Language).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Author).WithMany(p => p.Books)
                .HasForeignKey(d => d.AuthorId)
                .HasConstraintName("FK_Books_Authors1");
        });

        modelBuilder.Entity<InverntoryBalance>(entity =>
        {
            entity.HasKey(e => new { e.StoreId, e.Isbn }).HasName("PK__Invernto__C38284A70EB013C4");

            entity.ToTable("InverntoryBalance");

            entity.Property(e => e.Isbn)
                .HasMaxLength(13)
                .HasColumnName("ISBN");

            entity.HasOne(d => d.IsbnNavigation).WithMany(p => p.InverntoryBalances)
                .HasForeignKey(d => d.Isbn)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InverntoryBalance_Books");

            entity.HasOne(d => d.Store).WithMany(p => p.InverntoryBalances)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InverntoryBalance_Stores");
        });

        modelBuilder.Entity<OrderLog>(entity =>
        {
            entity.HasKey(e => e.OrderNumber);

            entity.ToTable("OrderLog");

            entity.Property(e => e.OrderNumber).ValueGeneratedNever();
            entity.Property(e => e.Isbn)
                .HasMaxLength(13)
                .HasColumnName("ISBN");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Buyer).WithMany(p => p.OrderLogs)
                .HasForeignKey(d => d.BuyerId)
                .HasConstraintName("FK_OrderLog_Stores1");

            entity.HasOne(d => d.IsbnNavigation).WithMany(p => p.OrderLogs)
                .HasForeignKey(d => d.Isbn)
                .HasConstraintName("FK_OrderLog_Books1");
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Stores__3214EC07D1EE53FA");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<TitlePerAuthor>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("TitlePerAuthor");

            entity.Property(e => e.Age)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.InventoryValue).HasMaxLength(4000);
            entity.Property(e => e.Name).HasMaxLength(511);
            entity.Property(e => e.Titles)
                .HasMaxLength(33)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
