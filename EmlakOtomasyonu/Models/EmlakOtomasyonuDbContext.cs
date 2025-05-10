using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EmlakOtomasyonu.Models;

public partial class EmlakOtomasyonuDbContext : DbContext
{
    public EmlakOtomasyonuDbContext()
    {
    }

    public EmlakOtomasyonuDbContext(DbContextOptions<EmlakOtomasyonuDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AdminTablosu> AdminTablosus { get; set; }

    public virtual DbSet<DisOzellikTablosu> DisOzellikTablosus { get; set; }

    public virtual DbSet<IcOzellikTablosu> IcOzellikTablosus { get; set; }

    public virtual DbSet<IlanDetayTablosu> IlanDetayTablosus { get; set; }

    public virtual DbSet<IlanTablosu> IlanTablosus { get; set; }

    public virtual DbSet<IslemTablosu> IslemTablosus { get; set; }

    public virtual DbSet<KategoriTablosu> KategoriTablosus { get; set; }

    public virtual DbSet<ResimTablosu> ResimTablosus { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=USER\\SQLEXPRESS;Database=EmlakOtomasyonuDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AdminTablosu>(entity =>
        {
            entity.HasKey(e => e.AdminId);

            entity.ToTable("Admin_Tablosu");

            entity.Property(e => e.AdminId).HasColumnName("adminID");
            entity.Property(e => e.AdminAd)
                .HasMaxLength(30)
                .IsFixedLength()
                .HasColumnName("adminAd");
            entity.Property(e => e.AdminKullaniciAdi)
                .HasMaxLength(30)
                .IsFixedLength()
                .HasColumnName("adminKullaniciAdi");
            entity.Property(e => e.AdminSifre)
                .HasMaxLength(30)
                .IsFixedLength()
                .HasColumnName("adminSifre");
            entity.Property(e => e.AdminSoyad)
                .HasMaxLength(30)
                .IsFixedLength()
                .HasColumnName("adminSoyad");
        });

        modelBuilder.Entity<DisOzellikTablosu>(entity =>
        {
            entity.HasKey(e => e.DoId);

            entity.ToTable("Dis_Ozellik_Tablosu");

            entity.Property(e => e.DoId).HasColumnName("doID");
            entity.Property(e => e.DoGuvenlik).HasColumnName("doGuvenlik");
            entity.Property(e => e.DoKapici).HasColumnName("doKapici");
            entity.Property(e => e.DoOtopark).HasColumnName("doOtopark");
            entity.Property(e => e.DoOyunParkı).HasColumnName("doOyunParkı");
            entity.Property(e => e.IlanId).HasColumnName("ilanID");

            entity.HasOne(d => d.Ilan).WithMany(p => p.DisOzellikTablosus)
                .HasForeignKey(d => d.IlanId)
                .HasConstraintName("FK_Dis_Ozellik_Tablosu_Ilan_Tablosu");
        });

        modelBuilder.Entity<IcOzellikTablosu>(entity =>
        {
            entity.HasKey(e => e.IoID);

            entity.ToTable("Ic_Ozellik_Tablosu");

            entity.Property(e => e.IoID).HasColumnName("ioID");
            entity.Property(e => e.IlanID).HasColumnName("ilanID");
            entity.Property(e => e.IoAsansor).HasColumnName("ioAsansor");
            entity.Property(e => e.IoDusKabini).HasColumnName("ioDusKabini");
            entity.Property(e => e.IoMobilyaTakimi).HasColumnName("ioMobilyaTakımı");
            entity.Property(e => e.IoSomine).HasColumnName("ioSomine");

            entity.HasOne(d => d.Ilan)
                .WithOne(p => p.IcOzellik) 
                .HasForeignKey<IcOzellikTablosu>(d => d.IlanID)
                .HasConstraintName("FK_Ic_Ozellik_Tablosu_Ilan_Tablosu");
        });


        modelBuilder.Entity<IlanDetayTablosu>(entity =>
        {
            entity.HasKey(e => e.IlanDetayId);

            entity.ToTable("Ilan_Detay_Tablosu");

            entity.Property(e => e.IlanDetayId).HasColumnName("ilanDetayID");
            entity.Property(e => e.IdBinaIsıtma)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("idBinaIsıtma");
            entity.Property(e => e.IdBinaKacinciKat).HasColumnName("idBinaKacinciKat");
            entity.Property(e => e.IdBinaKatSayisi).HasColumnName("idBinaKatSayisi");
            entity.Property(e => e.IdBinaYasi).HasColumnName("idBinaYasi");
            entity.Property(e => e.IdEsyaliMi).HasColumnName("idEsyaliMi");
            entity.Property(e => e.IdOdaSayisi).HasColumnName("idOdaSayisi");
            entity.Property(e => e.IdSalonSayisi).HasColumnName("idSalonSayisi");
            entity.Property(e => e.IlanId).HasColumnName("ilanID");

            entity.HasOne(d => d.Ilan).WithMany(p => p.IlanDetayTablosus)
                .HasForeignKey(d => d.IlanId)
                .HasConstraintName("FK_Ilan_Detay_Tablosu_Ilan_Tablosu");
        });

        modelBuilder.Entity<IlanTablosu>(entity =>
        {
            entity.HasKey(e => e.IlanId);

            entity.ToTable("Ilan_Tablosu");

            entity.Property(e => e.IlanId).HasColumnName("ilanID");
            entity.Property(e => e.IlanAciklama)
                .HasMaxLength(1000)
                .IsFixedLength()
                .HasColumnName("ilanAçıklama");
            entity.Property(e => e.IlanBaslik)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("ilanBaslık");
            entity.Property(e => e.IlanFiyat).HasColumnName("ilanFiyat");
            entity.Property(e => e.IlanKategoriId).HasColumnName("ilanKategoriID");
            entity.Property(e => e.IlanTarih)
                .HasColumnType("datetime")
                .HasColumnName("ilanTarih");
            entity.Property(e => e.IlanVitrin).HasColumnName("ilanVitrin");
            entity.Property(e => e.IlanVresim)
                .HasMaxLength(250)
                .IsFixedLength()
                .HasColumnName("ilanVResim");
            entity.Property(e => e.IlanIslemId).HasColumnName("ilanİslemID");

            entity.HasOne(d => d.IlanKategori).WithMany(p => p.IlanTablosus)
                .HasForeignKey(d => d.IlanKategoriId)
                .HasConstraintName("FK_Ilan_Tablosu_Kategori_Tablosu");

            entity.HasOne(d => d.IlanIslem).WithMany(p => p.IlanTablosus)
                .HasForeignKey(d => d.IlanIslemId)
                .HasConstraintName("FK_Ilan_Tablosu_Islem_Tablosu");
        });

        modelBuilder.Entity<IslemTablosu>(entity =>
        {
            entity.HasKey(e => e.IlanIslemId);

            entity.ToTable("Islem_Tablosu");

            entity.Property(e => e.IlanIslemId).HasColumnName("ilanIslemID");
            entity.Property(e => e.IslemAd)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("islemAd");
        });

        modelBuilder.Entity<KategoriTablosu>(entity =>
        {
            entity.HasKey(e => e.IlanKategoriId);

            entity.ToTable("Kategori_Tablosu");

            entity.Property(e => e.IlanKategoriId).HasColumnName("ilanKategoriID");
            entity.Property(e => e.KategoriAd)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("kategoriAd");
        });

        modelBuilder.Entity<ResimTablosu>(entity =>
        {
            entity.HasKey(e => e.ResimId);

            entity.ToTable("Resim_Tablosu");

            entity.Property(e => e.ResimId).HasColumnName("resimID");
            entity.Property(e => e.IlanId).HasColumnName("ilanID");
            entity.Property(e => e.ResimAd)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("resimAd");
            entity.Property(e => e.ResimResim)
                .HasMaxLength(500)
                .IsFixedLength()
                .HasColumnName("resimResim");

            entity.HasOne(d => d.Ilan).WithMany(p => p.ResimTablosus)
                .HasForeignKey(d => d.IlanId)
                .HasConstraintName("FK_Resim_Tablosu_Ilan_Tablosu");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
