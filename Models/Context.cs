using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace context.Models;

public partial class Context : DbContext
{

    public Context()
    {

    }

    public Context(DbContextOptions<Context> options)
    : base(options)
    {
    }

    public virtual DbSet<Tasks> Tasks { get; set; }

    public virtual DbSet<Users> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=JeyKe65");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Users>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("Users");

            entity.Property(e => e.Id).HasColumnName("Id");
            entity.Property(e => e.Login)
                .HasColumnType("character varying")
                .HasColumnName("Login");
            entity.Property(e => e.Password)
                .HasColumnType("character varying")
                .HasColumnName("Password");
            entity.Property(e => e.Name).HasColumnName("Name");
            entity.Property(e => e.Surname).HasColumnName("Surname");
            entity.Property(e => e.MiddleName).HasColumnName("MiddleName");


        });

        modelBuilder.Entity<Tasks>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("task_pkey");

            entity.ToTable("Tasks");

            entity.Property(e => e.Id).HasColumnName("Id");
            entity.Property(e => e.IdUser).HasColumnName("IdUser");
            entity.Property(e => e.NameTask)
               .HasMaxLength(50)
               .HasColumnName("NameTask");
            entity.Property(e => e.Deadline).HasColumnName("Deadline");
            entity.Property(e => e.Description)
               .HasMaxLength(255)
               .HasColumnName("Description");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Tasks)
               .HasForeignKey(d => d.IdUser)
               .HasConstraintName("users_id_task_fkey");

        });

   
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}