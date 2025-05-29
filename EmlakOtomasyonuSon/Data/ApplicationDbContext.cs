using Microsoft.EntityFrameworkCore;
using EmlakOtomasyonuSon.Models;

namespace EmlakOtomasyonu.Data
{
    /// <summary>
    /// Veritabanı bağlantısını ve tabloları yöneten ana context sınıfı
    /// Entity Framework Core'un DbContext sınıfından türetilmiştir
    /// Uygulamadaki tüm veritabanı işlemleri bu sınıf üzerinden gerçekleştirilir
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// ApplicationDbContext sınıfının yapıcı metodu
        /// Dependency Injection ile veritabanı bağlantı ayarları enjekte edilir
        /// </summary>
        /// <param name="options">Veritabanı bağlantı ayarlarını içeren options nesnesi</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) // Bağlantı ayarlarını temel sınıfa (DbContext) gönderir
        {
            // Yapıcı metot içinde özel bir işlem yapılmıyor
            // Bağlantı ayarları Startup.cs içinde konfigüre edilir
        }

        /// <summary>
        /// Yönetici bilgilerini içeren tablo - Admin giriş işlemleri için kullanılır
        /// </summary>
        public DbSet<TblAdmin> TblAdmin { get; set; }

        /// <summary>
        /// Emlak ilanlarının ana bilgilerini içeren tablo - Tüm emlak türleri için ortak alanlar
        /// </summary>
        public DbSet<TblIlan> TblIlan { get; set; }

        /// <summary>
        /// Emlak kategorilerini içeren tablo (Ev, İşyeri, Arsa)
        /// </summary>
        public DbSet<TblKategori> TblKategori { get; set; }

        /// <summary>
        /// Emlak işlem türlerini içeren tablo (Satılık, Kiralık)
        /// </summary>
        public DbSet<TblIslem> TblIslem { get; set; }

        /// <summary>
        /// Ev tipi emlaklar için özel özellikleri içeren tablo
        /// </summary>
        public DbSet<TblEvOzellikleri> TblEvOzellikleri { get; set; }

        /// <summary>
        /// Arsa tipi emlaklar için özel özellikleri içeren tablo
        /// </summary>
        public DbSet<TblArsaOzellikleri> TblArsaOzellikleri { get; set; }

        /// <summary>
        /// İşyeri tipi emlaklar için özel özellikleri içeren tablo
        /// </summary>
        public DbSet<Tblİsyeri> Tblİsyeri { get; set; }

        /// <summary>
        /// Emlak ilanlarının resimlerini içeren tablo - Bir ilana ait birden çok resim olabilir
        /// </summary>
        public DbSet<TblResimm> TblResimm { get; set; }

        /// <summary>
        /// İlanlarla ilgili kullanıcı taleplerini içeren tablo - İlgilenen kişilerin iletişim bilgileri
        /// </summary>
        public DbSet<IlanIstek> IlanIstekler { get; set; }

        /// <summary>
        /// Veritabanı modelini oluşturan metot - EF Core'un model-database eşleştirme ayarlarını tanımlar
        /// Bu metot, veritabanı tablolarını ve aralarındaki ilişkileri belirler
        /// Entity Framework tarafından veritabanı oluşturulurken veya sorgular yapılırken bu konfigürasyonlar kullanılır
        /// </summary>
        /// <param name="modelBuilder">Model konfigürasyonu için kullanılan Entity Framework model builder nesnesi</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Temel sınıfın (DbContext) OnModelCreating metodunu çağır
            // Bu, varsayılan konfigürasyonların uygulanmasını sağlar
            base.OnModelCreating(modelBuilder);

            // TblAdmin tablosu konfigürasyonu
            modelBuilder.Entity<TblAdmin>(entity =>
            {
                // Veritabanındaki tablonun adını belirler
                entity.ToTable("TblAdmin");
                // Birincil anahtar alanını belirler
                entity.HasKey(e => e.AdminId);
                // Zorunlu alanları belirler - null olamaz
                entity.Property(a => a.AdminAd).IsRequired();
                entity.Property(a => a.AdminSoyad).IsRequired();
                entity.Property(a => a.AdminKullaniciAdi).IsRequired();
                entity.Property(a => a.AdminSifre).IsRequired();
                // Veritabanı ilk oluşturulduğunda eklenecek varsayılan admin kullanıcı verisi
                entity.HasData(new EmlakOtomasyonuSon.Models.TblAdmin
                {
                    AdminId = 1,
                    AdminAd = "Yönetici",
                    AdminSoyad = "Yönetici",
                    AdminKullaniciAdi = "ICARDI99",
                    AdminSifre = "KUK"
                });
            });

            modelBuilder.Entity<TblIlan>(entity =>
            {
                entity.ToTable("Tbl_Ilan");
                entity.HasKey(e => e.IlanId);
                entity.Property(i => i.IlanId).HasColumnName("ilanId");
                entity.Property(i => i.IlanBaslik).HasColumnName("ilanBaslik").IsRequired();
                entity.Property(i => i.IlanFiyat).HasColumnName("ilanFiyat").IsRequired();
                entity.Property(i => i.IlanTarih).HasColumnName("ilanTarih").IsRequired();
                entity.Property(i => i.IlanKategoriId).HasColumnName("ilanKategoriId").IsRequired();
                entity.Property(i => i.IlanIslemId).HasColumnName("ilanIslemId").IsRequired();
                entity.Property(i => i.IlanAciklama).HasColumnName("ilanAciklama");
                entity.Property(i => i.IlanTelefon).HasColumnName("ilanTelefon");
                entity.Property(i => i.IlanVResim).HasColumnName("ilanVResim");
            });

            modelBuilder.Entity<TblIlan>()
                .HasOne(i => i.TblKategori)
                .WithMany(k => k.TblIlan)
                .HasForeignKey(i => i.IlanKategoriId);

            modelBuilder.Entity<TblIlan>()
                .HasOne(i => i.TblIslem)
                .WithMany(islem => islem.TblIlan)
                .HasForeignKey(i => i.IlanIslemId);

            modelBuilder.Entity<TblIlan>()
                .HasOne(i => i.TblEvOzellikleri)
                .WithOne(e => e.TblIlan)
                .HasForeignKey<TblEvOzellikleri>(e => e.IlanId);

            modelBuilder.Entity<TblIlan>()
                .HasOne(i => i.TblArsaOzellikleri)
                .WithOne(a => a.TblIlan)
                .HasForeignKey<TblArsaOzellikleri>(a => a.IlanId);

            modelBuilder.Entity<TblIlan>()
                .HasOne(i => i.Tblİsyeri)
                .WithOne(isyeri => isyeri.TblIlan)
                .HasForeignKey<Tblİsyeri>(isyeri => isyeri.IlanId);

            modelBuilder.Entity<TblIlan>()
                .HasMany(i => i.TblResimm)
                .WithOne(r => r.TblIlan)
                .HasForeignKey(r => r.IlanId);

            modelBuilder.Entity<TblKategori>(entity =>
            {
                entity.ToTable("Tbl_Kategori");
                entity.HasKey(e => e.IlanKategoriId);
                entity.Property(k => k.IlanKategoriId).HasColumnName("ilanKategoriId");
                entity.Property(k => k.KategoriAd).HasColumnName("kategoriAd").IsRequired();
            });

            modelBuilder.Entity<TblIslem>(entity =>
            {
                entity.ToTable("Tbl_Islem");
                entity.HasKey(e => e.IlanIslemId);
                entity.Property(i => i.IlanIslemId).HasColumnName("ilanIslemId");
                entity.Property(i => i.IslemAd).HasColumnName("islemAd").IsRequired();
            });

            modelBuilder.Entity<TblEvOzellikleri>(entity =>
            {
                entity.ToTable("Tbl_EvOzellikleri");
                entity.HasKey(e => e.EvOzellikId);
                entity.Property(e => e.EvOzellikId).HasColumnName("evOzellikId");
                entity.Property(e => e.IlanId).HasColumnName("ilanId");
                entity.Property(e => e.Metrekare).HasColumnName("metrekare").HasPrecision(18, 2);
                entity.Property(e => e.OdaSayisi).HasColumnName("odaSayisi");
                entity.Property(e => e.BinaYasi).HasColumnName("binaYasi");
                entity.Property(e => e.BulunduguKat).HasColumnName("bulunduguKat");
                entity.Property(e => e.KatSayisi).HasColumnName("katSayisi");
                entity.Property(e => e.IsitmaTuru).HasColumnName("isitmaTuru");
                entity.Property(e => e.Asansor).HasColumnName("asansor");
                entity.Property(e => e.Klima).HasColumnName("klima");
                entity.Property(e => e.Balkon).HasColumnName("balkon");
                entity.Property(e => e.Otopark).HasColumnName("otopark");
                entity.Property(e => e.DusKabini).HasColumnName("dusKabini");
                entity.Property(e => e.EsyaliMi).HasColumnName("esyaliMi");
                entity.Property(e => e.Guvenlik).HasColumnName("guvenlik");
                entity.Property(e => e.Jakuzi).HasColumnName("jakuzi");
                entity.Property(e => e.Kapici).HasColumnName("kapici");
                entity.Property(e => e.MobilyaTakimi).HasColumnName("mobilyaTakimi");
                entity.Property(e => e.OyunParki).HasColumnName("oyunParki");
                entity.Property(e => e.SalonSayisi).HasColumnName("salonSayisi");
                entity.Property(e => e.SiteİcerisindeMi).HasColumnName("siteİcerisindeMi");
                entity.Property(e => e.Somine).HasColumnName("somine");
                entity.Property(e => e.SporSalonu).HasColumnName("sporSalonu");
                entity.Property(e => e.YuzmeHavuzu).HasColumnName("yuzmeHavuzu");
            });

            modelBuilder.Entity<TblArsaOzellikleri>(entity =>
            {
                entity.ToTable("Tbl_ArsaOzellikleri");
                entity.HasKey(e => e.ArsaOzellikId);
                entity.Property(a => a.ArsaOzellikId).HasColumnName("arsaOzellikId");
                entity.Property(a => a.IlanId).HasColumnName("ilanId");
                entity.Property(a => a.Metrekare).HasColumnName("metrekare").HasPrecision(18, 2);
                entity.Property(a => a.MetrekareFiyati).HasColumnName("metrekareFiyatı").HasColumnType("int");
                entity.Property(a => a.ImarDurumu).HasColumnName("imarDurumu");
                entity.Property(a => a.TapuDurumu).HasColumnName("tapuDurumu");
                entity.Property(a => a.KullanimAmaci).HasColumnName("kullanimAmaci");
                entity.Property(a => a.ZeminYapisi).HasColumnName("zeminYapisi");
                entity.Property(a => a.ElektrikVar).HasColumnName("elektrikVar");
                entity.Property(a => a.SuVar).HasColumnName("suVar");
                entity.Property(a => a.DogalgazVar).HasColumnName("dogalgazVar");
                entity.Property(a => a.YolVar).HasColumnName("yolVar");
            });

            modelBuilder.Entity<Tblİsyeri>(entity =>
            {
                entity.ToTable("Tbl_İsyeri");
                entity.HasKey(e => e.IsyeriOzellikId);
                entity.Property(i => i.IsyeriOzellikId).HasColumnName("isyeriOzellikId");
                entity.Property(i => i.IlanId).HasColumnName("ilanId");
                entity.Property(i => i.Metrekare).HasColumnName("metrekare").HasPrecision(18, 2);
                entity.Property(i => i.BinaYasi).HasColumnName("binaYasi");
                entity.Property(i => i.BulunduguKat).HasColumnName("bulunduguKat");
                entity.Property(i => i.KatSayisi).HasColumnName("katSayisi");
                entity.Property(i => i.IsitmaTuru).HasColumnName("isitmaTuru");
                entity.Property(i => i.KullanimAmaci).HasColumnName("kullanimAmaci");
                entity.Property(i => i.Asansor).HasColumnName("asansor");
                entity.Property(i => i.Klima).HasColumnName("klima");
                entity.Property(i => i.Balkon).HasColumnName("balkon");
                entity.Property(i => i.Tuvalet).HasColumnName("tuvalet");
                entity.Property(i => i.Otopark).HasColumnName("otopark");
                entity.Property(i => i.Depo).HasColumnName("depo");
                entity.Property(i => i.Guvenlik).HasColumnName("guvenlik");
                entity.Property(i => i.Kapici).HasColumnName("kapici");
            });

            modelBuilder.Entity<TblResimm>(entity =>
            {
                entity.ToTable("Tbl_Resimm");
                entity.HasKey(e => e.ResimId);
                entity.Property(r => r.ResimId).HasColumnName("resimId");
                entity.Property(r => r.IlanId).HasColumnName("ilanId");
                entity.Property(r => r.ResimAd).HasColumnName("resimAd");
                entity.Property(r => r.ResimYolu).HasColumnName("resimYolu");
            });

            modelBuilder.Entity<IlanIstek>(entity =>
            {
                entity.ToTable("Tbl_Istek");
                entity.HasKey(e => e.IstekId);
                entity.Property(i => i.AdSoyad).IsRequired();
                entity.Property(i => i.Telefon).IsRequired();
                entity.Property(i => i.Email).IsRequired();
                entity.Property(i => i.Mesaj).IsRequired();
                entity.Property(i => i.IstekTarihi).IsRequired();
                entity.Property(i => i.ilanId).IsRequired();
                entity.Property(i => i.Okundu).IsRequired();

                entity.HasOne<TblIlan>()
                    .WithMany()
                    .HasForeignKey(i => i.ilanId);
            });
        }
    }
}