using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace Soliders.Models;

public partial class PrContext : DbContext
{
    //Строка для получения пути к бд
    string der = PacHt();
    public PrContext()
    {
    }

    public PrContext(DbContextOptions<PrContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Commission> Commissions { get; set; }

    public virtual DbSet<Conscript> Conscripts { get; set; }

    public virtual DbSet<Work> Works { get; set; }

    


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite(der);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Commission>(entity =>
        {
            entity.ToTable("Commission");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ConscriptFk).HasColumnName("conscript_fk");
            entity.Property(e => e.WorksFk).HasColumnName("works_fk");

            entity.HasOne(d => d.ConscriptFkNavigation).WithMany(p => p.Commissions).HasForeignKey(d => d.ConscriptFk);

            entity.HasOne(d => d.WorksFkNavigation).WithMany(p => p.Commissions).HasForeignKey(d => d.WorksFk);
        });

        modelBuilder.Entity<Conscript>(entity =>
        {
            entity.ToTable("Conscript");

            entity.HasIndex(e => e.Passport, "IX_Conscript_passport").IsUnique();

            entity.HasIndex(e => e.Snils, "IX_Conscript_snils").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.AddressNext).HasColumnName("address_next");
            entity.Property(e => e.Category).HasColumnName("category");
            entity.Property(e => e.Children).HasColumnName("children");
            entity.Property(e => e.Dateof).HasColumnName("dateof");
            entity.Property(e => e.FamilyStatus).HasColumnName("family_status");
            entity.Property(e => e.Firstname).HasColumnName("firstname");
            entity.Property(e => e.Lastname).HasColumnName("lastname");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Passport).HasColumnName("passport");
            entity.Property(e => e.Snils).HasColumnName("snils");
            entity.Property(e => e.SocialStatus).HasColumnName("social_status");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<Work>(entity =>
        {
            entity.HasIndex(e => e.Login, "IX_Works_login").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Firstname).HasColumnName("firstname");
            entity.Property(e => e.Lastname).HasColumnName("lastname");
            entity.Property(e => e.Login).HasColumnName("login");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Pass).HasColumnName("pass");
            entity.Property(e => e.Admin).HasColumnName("admin");
            entity.Property(e => e.Block).HasColumnName("block");
        });

        OnModelCreatingPartial(modelBuilder);
    }


    //Относительный путь
    static private string PacHt()
    {
        var x = Directory.GetCurrentDirectory();
        var y = Directory.GetParent(x).FullName;
        var c = Directory.GetParent(y).FullName;
        var r = "Data Source=" + Directory.GetParent(c).FullName + @"\DA\pr.db";
        return r;
    }


    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
