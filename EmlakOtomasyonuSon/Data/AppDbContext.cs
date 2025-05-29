using System;
using System.Collections.Generic;
using EmlakOtomasyonuSon.Models;
using Microsoft.EntityFrameworkCore;

namespace EmlakOtomasyonuSon.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<TblAdmin> TblAdmin { get; set; }
    public DbSet<TblIlan> TblIlan { get; set; }
    public DbSet<TblKategori> TblKategori { get; set; }
    public DbSet<TblIslem> TblIslem { get; set; }
    public DbSet<TblEvOzellikleri> TblEvOzellikleri { get; set; }
    public DbSet<TblArsaOzellikleri> TblArsaOzellikleri { get; set; }
    public DbSet<Tblİsyeri> Tblİsyeri { get; set; }
    public DbSet<TblResimm> TblResimm { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TblIlan>(entity =>
        {
            entity.ToTable("Tbl_Ilan");
            entity.HasKey(e => e.IlanId);

            entity.HasOne(i => i.TblKategori)
                .WithMany()
                .HasForeignKey(i => i.IlanKategoriId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(i => i.TblIslem)
                .WithMany()
                .HasForeignKey(i => i.IlanIslemId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(i => i.TblEvOzellikleri)
                .WithOne()
                .HasForeignKey<TblEvOzellikleri>(e => e.IlanId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(i => i.TblArsaOzellikleri)
                .WithOne()
                .HasForeignKey<TblArsaOzellikleri>(a => a.IlanId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(i => i.Tblİsyeri)
                .WithOne()
                .HasForeignKey<Tblİsyeri>(i => i.IlanId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(i => i.TblResimm)
                .WithOne()
                .HasForeignKey(r => r.IlanId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(i => i.IlanBaslik).IsRequired();
            entity.Property(i => i.IlanFiyat).IsRequired();
            entity.Property(i => i.IlanTarih).IsRequired();
            entity.Property(i => i.IlanKategoriId).IsRequired();
            entity.Property(i => i.IlanIslemId).IsRequired();
        });

        modelBuilder.Entity<TblKategori>(entity =>
        {
            entity.ToTable("Tbl_Kategori");
            entity.HasKey(e => e.IlanKategoriId);
            entity.Property(k => k.KategoriAd).IsRequired();
        });

        modelBuilder.Entity<TblIslem>(entity =>
        {
            entity.ToTable("Tbl_Islem");
            entity.HasKey(e => e.IlanIslemId);
            entity.Property(i => i.IslemAd).IsRequired();
        });

        modelBuilder.Entity<TblEvOzellikleri>(entity =>
        {
            entity.ToTable("Tbl_EvOzellikleri");
            entity.HasKey(e => e.EvOzellikId);
            entity.Property(e => e.Metrekare).HasPrecision(18, 2);
        });

        modelBuilder.Entity<TblArsaOzellikleri>(entity =>
        {
            entity.ToTable("Tbl_ArsaOzellikleri");
            entity.HasKey(e => e.ArsaOzellikId);
            entity.Property(a => a.Metrekare).HasPrecision(18, 2);
        });

        modelBuilder.Entity<Tblİsyeri>(entity =>
        {
            entity.ToTable("Tbl_İsyeri");
            entity.HasKey(e => e.IsyeriOzellikId);
            entity.Property(i => i.Metrekare).HasPrecision(18, 2);
        });

        modelBuilder.Entity<TblResimm>(entity =>
        {
            entity.ToTable("Tbl_Resimm");
            entity.HasKey(e => e.ResimId);
        });
    }
}
