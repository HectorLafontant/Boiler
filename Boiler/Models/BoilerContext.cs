using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Boiler.Models;

public partial class BoilerContext : DbContext
{
    public BoilerContext()
    {
    }

    public BoilerContext(DbContextOptions<BoilerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AccountAchievement> AccountAchievements { get; set; }

    public virtual DbSet<AccountGame> AccountGames { get; set; }

    public virtual DbSet<Achievement> Achievements { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<GameCategory> GameCategories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("Account");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Balance)
                .HasColumnType("decimal(6, 2)")
                .HasColumnName("balance");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");
        });

        modelBuilder.Entity<AccountAchievement>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdAccount).HasColumnName("id_account");
            entity.Property(e => e.IdAchievements).HasColumnName("id_achievements");

            entity.HasOne(d => d.IdAccountNavigation).WithMany(p => p.AccountAchievements)
                .HasForeignKey(d => d.IdAccount)
                .HasConstraintName("FK_AccountAchievements_User");

            entity.HasOne(d => d.IdAchievementsNavigation).WithMany(p => p.AccountAchievements)
                .HasForeignKey(d => d.IdAchievements)
                .HasConstraintName("FK_AccountAchievements_achievements");
        });

        modelBuilder.Entity<AccountGame>(entity =>
        {
            entity.ToTable("AccountGame");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdAccount).HasColumnName("id_account");
            entity.Property(e => e.IdGame).HasColumnName("id_game");
            entity.Property(e => e.Relation).HasColumnName("relation");

            entity.HasOne(d => d.IdAccountNavigation).WithMany(p => p.AccountGames)
                .HasForeignKey(d => d.IdAccount)
                .HasConstraintName("FK_AccountGame_User");

            entity.HasOne(d => d.IdGameNavigation).WithMany(p => p.AccountGames)
                .HasForeignKey(d => d.IdGame)
                .HasConstraintName("FK_AccountGame_Game");
        });

        modelBuilder.Entity<Achievement>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdGame).HasColumnName("id_game");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");

            entity.HasOne(d => d.IdGameNavigation).WithMany(p => p.Achievements)
                .HasForeignKey(d => d.IdGame)
                .HasConstraintName("FK_Achievements_Game");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Catergory");

            entity.ToTable("Category");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.ToTable("Game");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("price");
        });

        modelBuilder.Entity<GameCategory>(entity =>
        {
            entity.ToTable("GameCategory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdCategory).HasColumnName("id_category");
            entity.Property(e => e.IdGame).HasColumnName("id_game");

            entity.HasOne(d => d.IdCategoryNavigation).WithMany(p => p.GameCategories)
                .HasForeignKey(d => d.IdCategory)
                .HasConstraintName("FK_GameCategory_Category");

            entity.HasOne(d => d.IdGameNavigation).WithMany(p => p.GameCategories)
                .HasForeignKey(d => d.IdGame)
                .HasConstraintName("FK_GameCategory_Game");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
