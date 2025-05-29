// Emlak otomasyonu için gerekli model sýnýflarýný içeren namespace - TblIlan, TblEvOzellikleri, vs. gibi veri modellerini içerir
using EmlakOtomasyonuSon.Models;
// MVC yapýsýnýn temel sýnýflarýný içeren namespace - Controller, View, IActionResult gibi MVC bileþenlerini içerir
using Microsoft.AspNetCore.Mvc;
// Performans izleme ve hata ayýklama için kullanýlan namespace - Uygulamanýn performansýný izlemek için kullanýlýr
using System.Diagnostics;
// Veritabaný baðlantýsý için gerekli sýnýflarý içeren namespace - ApplicationDbContext bu namespace altýnda tanýmlanmýþtýr
using EmlakOtomasyonuSon.Data;
// Entity Framework Core fonksiyonlarýný içeren namespace - Include, FirstOrDefault gibi veritabaný sorgu metotlarýný içerir
using Microsoft.EntityFrameworkCore;
// HTTP istekleri için kullanýlan sýnýflarý içeren namespace - IFormFile gibi dosya yükleme sýnýflarýný içerir
using Microsoft.AspNetCore.Http;
// Dosya iþlemleri için gerekli namespace - Resim yükleme/silme iþlemleri için kullanýlýr
using System.IO;
// Temel C# fonksiyonlarýný içeren namespace - Temel veri tipleri ve iþlemleri için kullanýlýr
using System;
// LINQ sorgulama iþlemleri için gerekli namespace - Where, OrderBy gibi sorgu operatörlerini içerir
using System.Linq;
// Asenkron iþlemler için gerekli namespace - async/await kullanýmý için gereklidir
using System.Threading.Tasks;
// Koleksiyon sýnýflarýný içeren namespace - List<T> gibi koleksiyon tiplerini içerir
using System.Collections.Generic;
using EmlakOtomasyonuSon.Models;
using EmlakOtomasyonu.Data;

namespace EmlakOtomasyonu.Controllers
{
    /// <summary>
    /// Ana kontrol sýnýfý - MVC mimarisinde kullanýcý isteklerini karþýlar ve yanýtlar
    /// Bu controller sýnýfý tüm emlak ilan iþlemlerini yönetir: listeleme, detay görüntüleme, ekleme, düzenleme ve silme
    /// Ayrýca admin giriþ iþlemleri ve ilan istekleri yönetimi de bu controller tarafýndan saðlanýr
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Loglama iþlemleri için kullanýlan deðiþken
        /// Hata durumlarýnda veya önemli iþlemlerde log kaydý oluþturmak için kullanýlýr
        /// </summary>
        private readonly ILogger<HomeController> _logger;

        /// <summary>
        /// Veritabaný baðlantýsý için kullanýlan context deðiþkeni
        /// TblIlan, TblKategori gibi veritabaný tablolarýna eriþim saðlar
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor - Sýnýf örneði oluþturulduðunda çalýþan metot
        /// Dependency Injection ile ILogger ve ApplicationDbContext nesneleri enjekte edilir
        /// Bu sayede veritabaný iþlemleri ve loglama iþlemleri için gerekli baðýmlýlýklar enjekte edilmiþ olur
        /// </summary>
        /// <param name="logger">Loglama iþlemleri için ILogger nesnesi</param>
        /// <param name="context">Veritabaný iþlemleri için ApplicationDbContext nesnesi</param>
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger; // Logger nesnesi sýnýf deðiþkenine atanýr
            _context = context; // Veritabaný context nesnesi sýnýf deðiþkenine atanýr
        }

        /// <summary>
        /// Ana sayfa metodu - Tüm emlak ilanlarýný listeler
        /// Bu metot, uygulamaya giriþ noktasý olarak kullanýlýr ve tüm ilanlarý gösterir
        /// </summary>
        /// <returns>Ana sayfa view'ini tüm ilanlar listesiyle birlikte döndürür</returns>
        public IActionResult Index()
        {
            // Tüm ilanlarý, iliþkili tablolarla birlikte çeker
            var ilanlar = _context.TblIlan
                .Include(i => i.TblKategori) // Kategori bilgilerini dahil et (Ev, Arsa, Ýþyeri)
                .Include(i => i.TblIslem) // Ýþlem bilgilerini dahil et (Satýlýk, Kiralýk)
                .Include(i => i.TblResimm) // Resim bilgilerini dahil et (Tüm ilan resimleri)
                .OrderByDescending(i => i.IlanTarih) // Tarihe göre azalan sýralama (en yeni ilanlar en üstte)
                .ToList(); // Sorguyu çalýþtýr ve sonuçlarý listeye dönüþtür

            // Kategorileri ViewBag üzerinden view'a aktarýr (Navigation menüsü için kullanýlýr)
            ViewBag.Kategoriler = _context.TblKategori.ToList();

            // Ýlanlar listesini view'a gönderir
            return View(ilanlar); // Index.cshtml view'ine ilanlar listesini model olarak gönderir
        }

        /// <summary>
        /// Satýlýk ilanlarý listeleyen metot
        /// Sadece satýlýk iþlem türüne sahip ilanlarý filtreleyerek gösterir
        /// </summary>
        /// <returns>Filtrelenmiþ satýlýk ilanlar listesi içeren Index view'ini döndürür</returns>
        public IActionResult Satilik()
        {
            // Satýlýk olan ilanlarý iliþkili tablolarla birlikte çeker
            var ilanlar = _context.TblIlan
                .Include(i => i.TblKategori) // Kategori bilgilerini dahil et
                .Include(i => i.TblIslem) // Ýþlem bilgilerini dahil et
                .Include(i => i.TblResimm) // Resim bilgilerini dahil et
                .Where(i => i.TblIslem.IslemAd == "Satýlýk") // Sadece satýlýk olanlarý filtrele (TblIslem tablosundaki IslemAd alaný "Satýlýk" olanlar)
                .OrderByDescending(i => i.IlanTarih) // Tarihe göre azalan sýralama (en yeni ilanlar en üstte)
                .ToList(); // Sorguyu çalýþtýr ve sonuçlarý listeye dönüþtür

            // Index view'ýný kullanarak satýlýk ilanlarý gösterir
            // Bu sayede ayrý bir view oluþturmak yerine ayný view yapýsý tekrar kullanýlýr (DRY prensibi)
            return View("Index", ilanlar);
        }

        /// <summary>
        /// Kiralýk ilanlarý listeleyen metot
        /// Sadece kiralýk iþlem türüne sahip ilanlarý filtreleyerek gösterir
        /// </summary>
        /// <returns>Filtrelenmiþ kiralýk ilanlar listesi içeren Index view'ini döndürür</returns>
        public IActionResult Kiralik()
        {
            // Kiralýk olan ilanlarý iliþkili tablolarla birlikte çeker
            var ilanlar = _context.TblIlan
                .Include(i => i.TblKategori) // Kategori bilgilerini dahil et
                .Include(i => i.TblIslem) // Ýþlem bilgilerini dahil et
                .Include(i => i.TblResimm) // Resim bilgilerini dahil et
                .Where(i => i.TblIslem.IslemAd == "Kiralýk") // Sadece kiralýk olanlarý filtrele (TblIslem tablosundaki IslemAd alaný "Kiralýk" olanlar)
                .OrderByDescending(i => i.IlanTarih) // Tarihe göre azalan sýralama (en yeni ilanlar en üstte)
                .ToList(); // Sorguyu çalýþtýr ve sonuçlarý listeye dönüþtür

            // Index view'ýný kullanarak kiralýk ilanlarý gösterir
            // Bu sayede ayrý bir view oluþturmak yerine ayný view yapýsý tekrar kullanýlýr (DRY prensibi)
            return View("Index", ilanlar);
        }

        /// <summary>
        /// Gizlilik politikasý sayfasýný gösteren metot
        /// Kullanýcýlarýn gizlilik haklarý ve kiþisel verilerin nasýl iþlendiði gibi bilgileri içerir
        /// </summary>
        /// <returns>Gizlilik politikasý sayfasýný döndürür</returns>
        public IActionResult Privacy()
        {
            // Gizlilik sayfasýný gösterir (Privacy.cshtml)
            return View();
        }

        /// <summary>
        /// Admin giriþ sayfasýný gösteren metot
        /// Yöneticilerin sisteme giriþ yapabilmesi için giriþ formunu içerir
        /// </summary>
        /// <returns>Admin giriþ sayfasýný döndürür</returns>
        public IActionResult AdminLogin()
        {
            // Admin login sayfasýný gösterir (AdminLogin.cshtml)
            // Kullanýcý adý ve þifre giriþ formunu içerir
            return View();
        }

        /// <summary>
        /// Admin giriþ iþlemini gerçekleþtiren metot
        /// Kullanýcýnýn girdiði bilgileri doðrular ve giriþ iþlemini gerçekleþtirir
        /// </summary>
        /// <param name="username">Kullanýcý adý</param>
        /// <param name="password">Þifre</param>
        /// <returns>Giriþ baþarýlýysa admin paneline, baþarýsýzsa tekrar giriþ sayfasýna yönlendirir</returns>
        [HttpPost] // Sadece HTTP POST metoduyla çaðrýlabilir, güvenlik için önemlidir
        public async Task<IActionResult> AdminLogin(string username, string password)
        {
            // Veritabanýndan kullanýcý adý ve þifreye göre admin kaydýný asenkron olarak arar
            // Asenkron sorgulama, uygulamanýn performansýný artýrýr
            var admin = await _context.TblAdmin
                .FirstOrDefaultAsync(a => a.AdminKullaniciAdi == username && a.AdminSifre == password);

            // Eþleþen admin bulunduysa (giriþ baþarýlý)
            if (admin != null)
            {
                // NOT: Gerçek bir uygulamada burada session veya cookie'ye admin bilgilerini kaydetmeniz gerekir
                // Giriþ baþarýlý, admin paneline yönlendirir
                return RedirectToAction("AdminPanel");
            }

            // Giriþ baþarýsýz, hata mesajý gösterir
            ViewBag.Error = "Kullanýcý adý veya þifre hatalý!";
            // Ayný sayfaya geri döner, hata mesajýyla birlikte
            return View();
        }

        /// <summary>
        /// Admin panel sayfasýný gösteren metot
        /// Yöneticilerin sistem üzerindeki iþlemleri yapabildiði ana kontrol paneli
        /// NOT: Gerçek bir uygulamada burada authentication kontrolü yapýlmalýdýr
        /// </summary>
        /// <returns>Admin panel sayfasýný döndürür</returns>
        public IActionResult AdminPanel()
        {
            // Admin panel sayfasýný gösterir (AdminPanel.cshtml)
            // Burada yönetici yetkilerini kontrol eden bir filtre eklenebilir
            return View();
        }

        // Admin'in kendi ilanlarýný listeleyen metot (Filtreleme özelliði eklenmiþ)
        public IActionResult Ilanlarim(
            // Genel filtreler
            int? IlanKategoriId = null,
            int? IlanIslemId = null,

            // Fiyat ve metrekare aralýklarý
            int? minFiyat = null,
            int? maxFiyat = null,
            decimal? minMetrekare = null,
            decimal? maxMetrekare = null,

            // Ev özellikleri filtreleri
            bool? asansor = null,
            bool? esyaliMi = null,
            bool? otopark = null,
            bool? yuzmeHavuzu = null,
            bool? sporSalonu = null,
            bool? siteIcerisinde = null,
            int? minOdaSayisi = null,
            int? maxOdaSayisi = null,
            int? minSalonSayisi = null,
            int? maxSalonSayisi = null,
            int? minKatSayisi = null,
            int? maxKatSayisi = null,
            int? minBulunduguKat = null,
            int? maxBulunduguKat = null,

            // Arsa özellikleri filtreleri
            int? minMetrekareFiyati = null,
            int? maxMetrekareFiyati = null,
            bool? elektrikVar = null,
            bool? suVar = null,
            bool? dogalgazVar = null,
            bool? yolVar = null,

            // Ýþyeri özellikleri filtreleri
            bool? depo = null,
            int? minBinaYasi = null,
            int? maxBinaYasi = null)
        {
            // Tüm ilanlarý, iliþkili tablolarla birlikte çeker
            var ilanlarQuery = _context.TblIlan
                .Include(i => i.TblKategori) // Kategori bilgilerini dahil et
                .Include(i => i.TblIslem) // Ýþlem bilgilerini dahil et
                .Include(i => i.TblEvOzellikleri) // Ev özelliklerini dahil et
                .Include(i => i.TblArsaOzellikleri) // Arsa özelliklerini dahil et
                .Include(i => i.TblÝsyeri) // Ýþyeri özelliklerini dahil et
                .AsQueryable(); // AsQueryable ile sorguya dönüþtür

            // Genel filtreler
            if (IlanKategoriId.HasValue)
            {
                ilanlarQuery = ilanlarQuery.Where(i => i.IlanKategoriId == IlanKategoriId.Value);
            }

            if (IlanIslemId.HasValue)
            {
                ilanlarQuery = ilanlarQuery.Where(i => i.IlanIslemId == IlanIslemId.Value);
            }

            // Fiyat aralýðý filtresi
            if (minFiyat.HasValue)
            {
                ilanlarQuery = ilanlarQuery.Where(i => i.IlanFiyat >= minFiyat.Value);
            }

            if (maxFiyat.HasValue)
            {
                ilanlarQuery = ilanlarQuery.Where(i => i.IlanFiyat <= maxFiyat.Value);
            }

            // Ev özellikleri filtreleri (TblEvOzellikleri tablosu üzerinden)
            if (minMetrekare.HasValue || maxMetrekare.HasValue ||
                asansor.HasValue || esyaliMi.HasValue || otopark.HasValue ||
                yuzmeHavuzu.HasValue || sporSalonu.HasValue || siteIcerisinde.HasValue ||
                minOdaSayisi.HasValue || maxOdaSayisi.HasValue ||
                minSalonSayisi.HasValue || maxSalonSayisi.HasValue ||
                minKatSayisi.HasValue || maxKatSayisi.HasValue ||
                minBulunduguKat.HasValue || maxBulunduguKat.HasValue)
            {
                // Ýliþkili tablo üzerinden filtreleme yapacaðýmýz için null olmayanlarý filtreliyoruz
                ilanlarQuery = ilanlarQuery.Where(i => i.TblEvOzellikleri != null);

                if (minMetrekare.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblEvOzellikleri.Metrekare >= minMetrekare.Value);
                }

                if (maxMetrekare.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblEvOzellikleri.Metrekare <= maxMetrekare.Value);
                }

                if (asansor.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblEvOzellikleri.Asansor == asansor.Value);
                }

                if (esyaliMi.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblEvOzellikleri.EsyaliMi == esyaliMi.Value);
                }

                if (otopark.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblEvOzellikleri.Otopark == otopark.Value);
                }

                if (yuzmeHavuzu.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblEvOzellikleri.YuzmeHavuzu == yuzmeHavuzu.Value);
                }

                if (sporSalonu.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblEvOzellikleri.SporSalonu == sporSalonu.Value);
                }

                if (siteIcerisinde.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblEvOzellikleri.SiteÝcerisindeMi == siteIcerisinde.Value);
                }

                if (minOdaSayisi.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblEvOzellikleri.OdaSayisi == minOdaSayisi.Value);
                }

                if (minSalonSayisi.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblEvOzellikleri.SalonSayisi == minSalonSayisi.Value);
                }

                if (minKatSayisi.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblEvOzellikleri.KatSayisi == minKatSayisi.Value);
                }

                if (minBulunduguKat.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblEvOzellikleri.BulunduguKat == minBulunduguKat.Value);
                }
            }

            // Arsa özellikleri filtreleri (TblArsaOzellikleri tablosu üzerinden)
            if (minMetrekare.HasValue || maxMetrekare.HasValue ||
                minMetrekareFiyati.HasValue || maxMetrekareFiyati.HasValue ||
                elektrikVar.HasValue || suVar.HasValue || dogalgazVar.HasValue || yolVar.HasValue)
            {
                // Ýliþkili tablo üzerinden filtreleme yapacaðýmýz için null olmayanlarý filtreliyoruz
                ilanlarQuery = ilanlarQuery.Where(i => i.TblArsaOzellikleri != null);

                if (minMetrekare.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblArsaOzellikleri.Metrekare >= minMetrekare.Value);
                }

                if (maxMetrekare.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblArsaOzellikleri.Metrekare <= maxMetrekare.Value);
                }

                if (minMetrekareFiyati.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblArsaOzellikleri.MetrekareFiyati >= minMetrekareFiyati.Value);
                }

                if (maxMetrekareFiyati.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblArsaOzellikleri.MetrekareFiyati <= maxMetrekareFiyati.Value);
                }

                if (elektrikVar.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblArsaOzellikleri.ElektrikVar == elektrikVar.Value);
                }

                if (suVar.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblArsaOzellikleri.SuVar == suVar.Value);
                }

                if (dogalgazVar.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblArsaOzellikleri.DogalgazVar == dogalgazVar.Value);
                }

                if (yolVar.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblArsaOzellikleri.YolVar == yolVar.Value);
                }
            }

            // Ýþyeri özellikleri filtreleri (TblÝsyeri tablosu üzerinden)
            if (minMetrekare.HasValue || maxMetrekare.HasValue || asansor.HasValue ||
                otopark.HasValue || depo.HasValue || minKatSayisi.HasValue ||
                minBulunduguKat.HasValue || minBinaYasi.HasValue || maxBinaYasi.HasValue)
            {
                // Ýliþkili tablo üzerinden filtreleme yapacaðýmýz için null olmayanlarý filtreliyoruz
                ilanlarQuery = ilanlarQuery.Where(i => i.TblÝsyeri != null);

                if (minMetrekare.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblÝsyeri.Metrekare >= minMetrekare.Value);
                }

                if (maxMetrekare.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblÝsyeri.Metrekare <= maxMetrekare.Value);
                }

                if (asansor.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblÝsyeri.Asansor == asansor.Value);
                }

                if (otopark.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblÝsyeri.Otopark == otopark.Value);
                }

                if (depo.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblÝsyeri.Depo == depo.Value);
                }

                if (minKatSayisi.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblÝsyeri.KatSayisi == minKatSayisi.Value);
                }

                if (minBulunduguKat.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblÝsyeri.BulunduguKat == minBulunduguKat.Value);
                }

                if (minBinaYasi.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblÝsyeri.BinaYasi >= minBinaYasi.Value);
                }

                if (maxBinaYasi.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblÝsyeri.BinaYasi <= maxBinaYasi.Value);
                }
            }

            // Filtreleri ViewBag'e aktar
            ViewBag.Kategoriler = _context.TblKategori.ToList();
            ViewBag.Islemler = _context.TblIslem.ToList();

            // Filtrelenmiþ ilanlarý tarihe göre sýrala ve listeye dönüþtür
            var ilanlar = ilanlarQuery
                .OrderByDescending(i => i.IlanTarih)
                .ToList();

            // Ýlanlarý view'a gönderir
            return View(ilanlar);
        }

        // Ýlan detayýný gösteren metot
        public IActionResult IlanDetay(int id, bool isAdmin = false)
        {
            // ID'ye göre ilaný, tüm iliþkili tablolarla birlikte çeker
            var ilan = _context.TblIlan
                .Include(i => i.TblKategori) // Kategori bilgilerini dahil et
                .Include(i => i.TblIslem) // Ýþlem bilgilerini dahil et
                .Include(i => i.TblEvOzellikleri) // Ev özelliklerini dahil et (varsa)
                .Include(i => i.TblArsaOzellikleri) // Arsa özelliklerini dahil et (varsa)
                .Include(i => i.TblÝsyeri) // Ýþyeri özelliklerini dahil et (varsa)
                .Include(i => i.TblResimm) // Resim bilgilerini dahil et
                .FirstOrDefault(i => i.IlanId == id); // ID'ye göre ilaný bul

            if (ilan == null)
            {
                // Ýlan bulunamazsa 404 hatasý döndürür
                return NotFound();
            }

            // Admin panelinden geliyorsa ViewBag'e bilgi gönder
            ViewBag.IsAdmin = isAdmin;

            // Ýlan detayýný view'a gönderir
            return View(ilan);
        }

        // Yeni ilan oluþturma sayfasýný gösteren metot
        public IActionResult YeniIlan()
        {
            // Kategorileri veritabanýndan çeker
            var kategoriler = _context.TblKategori.ToList();
            // Ýþlemleri ViewBag aracýlýðýyla view'a aktarýr
            ViewBag.Islemler = _context.TblIslem.ToList();
            // Kategori listesini view'a gönderir
            return View(kategoriler);
        }

        // Yeni ilan oluþturma iþlemini gerçekleþtiren metot - HTTP POST iþlemi için kullanýlýr
        [HttpPost]
        public IActionResult YeniIlan(TblIlan ilan)
        {
            // Model doðrulamasý baþarýlýysa
            if (ModelState.IsValid)
            {
                // Ýlan tarihini þu anki zaman olarak atar
                ilan.IlanTarih = DateTime.Now;
                // Ýlaný veritabanýna ekler
                _context.TblIlan.Add(ilan);
                // Deðiþiklikleri kaydeder
                _context.SaveChanges();
                // Ýlanlarým sayfasýna yönlendirir
                return RedirectToAction(nameof(Ilanlarim));
            }

            // Model doðrulamasý baþarýsýzsa, hata mesajlarý ile birlikte ayný sayfaya geri döner
            ViewBag.Kategoriler = _context.TblKategori.ToList();
            ViewBag.Islemler = _context.TblIslem.ToList();
            return View(ilan);
        }

        // Yeni ilan detaylarýný girmek için sayfayý gösteren metot
        public IActionResult YeniIlanDetay(int id)
        {
            // Ýþlem listesini dropdown için hazýrlar
            ViewBag.Islemler = _context.TblIslem
                .Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = i.IlanIslemId.ToString(), // Deðer olarak ID kullanýlýr
                    Text = i.IslemAd // Görüntülenecek metin olarak iþlem adý kullanýlýr
                }).ToList();

            // Yeni bir view model oluþturur
            var vm = new IlanDetayViewModel
            {
                // Kategori ID'sini parametre olarak alýnan deðere ayarlar
                Ilan = new TblIlan { IlanKategoriId = id }
            };

            // Kategori ID'sini ViewBag'e ekler
            ViewBag.KategoriId = id;
            // View modeli view'a gönderir
            return View(vm);
        }

        // Yeni ilan detaylarýný kaydetmek için kullanýlan metot - HTTP POST iþlemi için kullanýlýr
        // CSRF saldýrýlarýna karþý koruma saðlayan ValidateAntiForgeryToken özniteliði
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult YeniIlanDetay(IlanDetayViewModel vm)
        {
            ModelState.Remove("Ilan.IlanVResim");

            // Ýlan bilgisi null deðilse iþlem yap
            if (vm.Ilan != null)
            {
                // Ýlan kategorisi 1 ise (Ev kategorisi)
                {
                    ModelState.Remove("ArsaOzellikleri.ImarDurumu");
                    ModelState.Remove("ArsaOzellikleri.TapuDurumu");
                    ModelState.Remove("ArsaOzellikleri.ZeminYapisi");
                    ModelState.Remove("ArsaOzellikleri.KullanimAmaci");
                    ModelState.Remove("IsyeriOzellikleri.IsitmaTuru");
                    ModelState.Remove("IsyeriOzellikleri.KullanimAmaci");
                    ModelState.Remove("ArsaOzellikleri");
                    ModelState.Remove("IsyeriOzellikleri");
                }
                // Ýlan kategorisi 2 ise (Ýþyeri kategorisi)
                {
                    ModelState.Remove("EvOzellikleri.Asansor");
                    ModelState.Remove("EvOzellikleri.Somine");
                    ModelState.Remove("EvOzellikleri.MobilyaTakimi");
                    ModelState.Remove("EvOzellikleri.DusKabini");
                    ModelState.Remove("EvOzellikleri.Klima");
                    ModelState.Remove("EvOzellikleri.Balkon");
                    ModelState.Remove("EvOzellikleri.EsyaliMi");
                    ModelState.Remove("EvOzellikleri.Jakuzi");
                    ModelState.Remove("EvOzellikleri.Otopark");
                    ModelState.Remove("EvOzellikleri.OyunParki");
                    ModelState.Remove("EvOzellikleri.Guvenlik");
                    ModelState.Remove("EvOzellikleri.Kapici");
                    ModelState.Remove("EvOzellikleri.YuzmeHavuzu");
                    ModelState.Remove("EvOzellikleri.SporSalonu");
                    ModelState.Remove("EvOzellikleri.SiteÝcerisindeMi");
                    ModelState.Remove("EvOzellikleri.OdaSayisi");
                    ModelState.Remove("EvOzellikleri.SalonSayisi");
                    ModelState.Remove("EvOzellikleri.BinaYasi");
                    ModelState.Remove("EvOzellikleri.BulunduguKat");
                    ModelState.Remove("EvOzellikleri.KatSayisi");
                    ModelState.Remove("EvOzellikleri.IsitmaTuru");
                    ModelState.Remove("EvOzellikleri.Metrekare");
                    ModelState.Remove("EvOzellikleri.MetrekareFiyati");
                    ModelState.Remove("EvOzellikleri");
                    ModelState.Remove("ArsaOzellikleri.ImarDurumu");
                    ModelState.Remove("ArsaOzellikleri.TapuDurumu");
                    ModelState.Remove("ArsaOzellikleri.ZeminYapisi");
                    ModelState.Remove("ArsaOzellikleri.KullanimAmaci");
                    ModelState.Remove("ArsaOzellikleri");
                }
                // Ýlan kategorisi 3 ise (Arsa kategorisi)
                {
                    ModelState.Remove("EvOzellikleri.Asansor");
                    ModelState.Remove("EvOzellikleri.Somine");
                    ModelState.Remove("EvOzellikleri.MobilyaTakimi");
                    ModelState.Remove("EvOzellikleri.DusKabini");
                    ModelState.Remove("EvOzellikleri.Klima");
                    ModelState.Remove("EvOzellikleri.Balkon");
                    ModelState.Remove("EvOzellikleri.EsyaliMi");
                    ModelState.Remove("EvOzellikleri.Jakuzi");
                    ModelState.Remove("EvOzellikleri.Otopark");
                    ModelState.Remove("EvOzellikleri.OyunParki");
                    ModelState.Remove("EvOzellikleri.Guvenlik");
                    ModelState.Remove("EvOzellikleri.Kapici");
                    ModelState.Remove("EvOzellikleri.YuzmeHavuzu");
                    ModelState.Remove("EvOzellikleri.SporSalonu");
                    ModelState.Remove("EvOzellikleri.SiteÝcerisindeMi");
                    ModelState.Remove("EvOzellikleri.OdaSayisi");
                    ModelState.Remove("EvOzellikleri.SalonSayisi");
                    ModelState.Remove("EvOzellikleri.BinaYasi");
                    ModelState.Remove("EvOzellikleri.BulunduguKat");
                    ModelState.Remove("EvOzellikleri.KatSayisi");
                    ModelState.Remove("EvOzellikleri.IsitmaTuru");
                    ModelState.Remove("EvOzellikleri.Metrekare");
                    ModelState.Remove("EvOzellikleri.MetrekareFiyati");
                    ModelState.Remove("EvOzellikleri");
                    ModelState.Remove("IsyeriOzellikleri.IsitmaTuru");
                    ModelState.Remove("IsyeriOzellikleri.KullanimAmaci");
                    ModelState.Remove("IsyeriOzellikleri.Metrekare");
                    ModelState.Remove("IsyeriOzellikleri.BinaYasi");
                    ModelState.Remove("IsyeriOzellikleri.BulunduguKat");
                    ModelState.Remove("IsyeriOzellikleri.KatSayisi");
                    ModelState.Remove("IsyeriOzellikleri.Asansor");
                    ModelState.Remove("IsyeriOzellikleri.Klima");
                    ModelState.Remove("IsyeriOzellikleri.Balkon");
                    ModelState.Remove("IsyeriOzellikleri.Tuvalet");
                    ModelState.Remove("IsyeriOzellikleri.Otopark");
                    ModelState.Remove("IsyeriOzellikleri.Guvenlik");
                    ModelState.Remove("IsyeriOzellikleri.Kapici");
                    ModelState.Remove("IsyeriOzellikleri.Depo");
                    ModelState.Remove("IsyeriOzellikleri");
                }
            }

            try
            {
                if (!ModelState.IsValid)
                {
                    // Tüm model doðrulama hatalarýný tek tek alýp görüntüle
                    foreach (var key in ModelState.Keys) // Her bir model alaný için
                    {
                        var state = ModelState[key]; // Alanýn durumunu al
                        foreach (var error in state.Errors) // Alandaki her hata için
                        {
                            // Hata mesajýný ModelState'e ekle
                            ModelState.AddModelError("", $"Hata - {key}: {error.ErrorMessage}");
                        }
                    }
                    ViewBag.Islemler = _context.TblIslem
                        .Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                        {
                            Value = i.IlanIslemId.ToString(),
                            Text = i.IslemAd
                        }).ToList();
                    ViewBag.KategoriId = vm.Ilan?.IlanKategoriId;
                    return View(vm);
                }

                // Ýlan nesnesi null ise hata mesajý ver
                if (vm.Ilan == null)
                {
                    ModelState.AddModelError("", "Ýlan bilgisi eksik.");
                    ViewBag.Islemler = _context.TblIslem
                        .Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                        {
                            Value = i.IlanIslemId.ToString(),
                            Text = i.IslemAd
                        }).ToList();
                    ViewBag.KategoriId = null;
                    return View(vm);
                }

                // Ýlan tarihini þimdiki zaman olarak ayarla
                vm.Ilan.IlanTarih = DateTime.Now;

                // Vitrin Resmi Yükleme Ýþlemi
                // Yüklenen resim varsa ve boþ deðilse iþleme al
                if (vm.Resim != null && vm.Resim.Length > 0)
                {
                    var imagesDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                    if (!Directory.Exists(imagesDir))
                        Directory.CreateDirectory(imagesDir);

                    var dosyaAdi = Guid.NewGuid().ToString() + Path.GetExtension(vm.Resim.FileName);
                    var yol = Path.Combine(imagesDir, dosyaAdi);

                    using (var stream = new FileStream(yol, FileMode.Create))
                    {
                        vm.Resim.CopyTo(stream);
                    }

                    // Ýlanýn vitrin resim yolunu veritabaný için ayarla
                    vm.Ilan.IlanVResim = "/images/" + dosyaAdi;
                }

                // Ýlaný veritabanýna ekle
                _context.TblIlan.Add(vm.Ilan);

                // Veritabanýna kaydetme iþlemi için ayrý bir try-catch bloðu
                try
                {
                    // Deðiþiklikleri veritabanýna kaydet
                    _context.SaveChanges();
                }
                catch (Exception dbex) // Veritabaný hatasý durumunda
                {
                    // Veritabaný hatasýný kullanýcýya göstermek için ModelState'e ekle
                    ModelState.AddModelError("", "Veritabaný kaydýnda hata: " + dbex.Message);
                    ViewBag.Islemler = _context.TblIslem
                        .Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                        {
                            Value = i.IlanIslemId.ToString(),
                            Text = i.IslemAd
                        }).ToList();
                    ViewBag.KategoriId = vm.Ilan.IlanKategoriId;
                    return View(vm);
                }

                // Kategori türüne göre özel özellikleri kaydet
                // Kategori 1 (Ev) ve ev özellikleri mevcutsa
                if (vm.Ilan.IlanKategoriId == 1 && vm.EvOzellikleri != null) // 1: Ev
                {
                    // Ev özelliklerine ilan ID'sini ata
                    vm.EvOzellikleri.IlanId = vm.Ilan.IlanId;
                    // Ev özelliklerini veritabanýna ekle
                    _context.TblEvOzellikleri.Add(vm.EvOzellikleri);
                    // Deðiþiklikleri kaydet
                    _context.SaveChanges();
                }
                // Kategori 2 (Ýþyeri) ve iþyeri özellikleri mevcutsa
                else if (vm.Ilan.IlanKategoriId == 2 && vm.IsyeriOzellikleri != null) // 2: Ýþyeri
                {
                    // Ýþyeri özelliklerine ilan ID'sini ata
                    vm.IsyeriOzellikleri.IlanId = vm.Ilan.IlanId;
                    // Ýþyeri özelliklerini veritabanýna ekle
                    _context.TblÝsyeri.Add(vm.IsyeriOzellikleri);
                    // Deðiþiklikleri kaydet
                    _context.SaveChanges();
                }
                // Kategori 3 (Arsa) ve arsa özellikleri mevcutsa
                else if (vm.Ilan.IlanKategoriId == 3 && vm.ArsaOzellikleri != null) // 3: Arsa
                {
                    // Arsa özelliklerine ilan ID'sini ata
                    vm.ArsaOzellikleri.IlanId = vm.Ilan.IlanId;
                    // Arsa özelliklerini veritabanýna ekle
                    _context.TblArsaOzellikleri.Add(vm.ArsaOzellikleri);
                    // Deðiþiklikleri kaydet
                    _context.SaveChanges();
                }

                // Ýlan detay resimlerini kaydet
                // Detay resimleri varsa ve en az bir tane yüklenmiþse
                if (vm.DetayResimleri != null && vm.DetayResimleri.Count > 0)
                {
                    var imagesDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                    foreach (var detayResim in vm.DetayResimleri)
                    {
                        if (detayResim != null && detayResim.Length > 0)
                        {
                            var detayDosyaAdi = Guid.NewGuid().ToString() + Path.GetExtension(detayResim.FileName);
                            var detayYol = Path.Combine(imagesDir, detayDosyaAdi);

                            using (var stream = new FileStream(detayYol, FileMode.Create))
                            {
                                detayResim.CopyTo(stream);
                            }

                            // Veritabanýna kaydedilecek resim bilgilerini içeren nesne oluþtur
                            var resimEntity = new TblResimm
                            {
                                ResimAd = detayResim.FileName, // Orijinal dosya adý
                                ResimYolu = "/images/" + detayDosyaAdi, // Web'den eriþilebilir yol
                                IlanId = vm.Ilan.IlanId // Ýlan ile iliþkilendirme
                            };
                            _context.TblResimm.Add(resimEntity);
                        }
                    }
                    _context.SaveChanges();
                }

                return RedirectToAction("Ilanlarim");
            }
            catch (Exception ex)
            {
                // Genel hata mesajýný kullanýcýya göstermek için ModelState'e ekle
                ModelState.AddModelError("", "Kayýt sýrasýnda hata oluþtu: " + ex.Message);
                ViewBag.Islemler = _context.TblIslem
                    .Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                    {
                        Value = i.IlanIslemId.ToString(),
                        Text = i.IslemAd
                    }).ToList();
                ViewBag.KategoriId = vm.Ilan?.IlanKategoriId;
                return View(vm);
            }
        }

        // ÝLAN DÜZENLE GET - Ýlan düzenleme sayfasýný gösteren metot
        // Ýlan düzenleme sayfasýný gösteren metot - ID parametresi ile düzenlenecek ilaný belirler
        public IActionResult IlanDuzenle(int id)
        {
            ViewBag.Islemler = _context.TblIslem
                .Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = i.IlanIslemId.ToString(),
                    Text = i.IslemAd
                }).ToList();
            var ilan = _context.TblIlan
                .Include(i => i.TblEvOzellikleri)
                .Include(i => i.TblArsaOzellikleri)
                .Include(i => i.TblÝsyeri)
                .Include(i => i.TblResimm)
                .FirstOrDefault(i => i.IlanId == id);

            if (ilan == null)
                return NotFound();

            // ViewModel kullanýyoruz!
            var vm = new IlanDetayViewModel
            {
                Ilan = ilan,
                EvOzellikleri = ilan.TblEvOzellikleri,
                ArsaOzellikleri = ilan.TblArsaOzellikleri,
                IsyeriOzellikleri = ilan.TblÝsyeri,
                MevcutResimler = ilan.TblResimm?.ToList() ?? new List<TblResimm>()
            };
            // Ev özellikleri için null deðerleri false'a çevir (boolean deðerler için)
            // Ev özellikleri nesnesi varsa
            if (vm.EvOzellikleri != null)
            {
                // Ev özelliklerinde null olan boolean deðerleri false'a çevir
                vm.EvOzellikleri.Asansor = vm.EvOzellikleri.Asansor ?? false;
                vm.EvOzellikleri.Somine = vm.EvOzellikleri.Somine ?? false;
                vm.EvOzellikleri.MobilyaTakimi = vm.EvOzellikleri.MobilyaTakimi ?? false;
                vm.EvOzellikleri.DusKabini = vm.EvOzellikleri.DusKabini ?? false;
                vm.EvOzellikleri.Klima = vm.EvOzellikleri.Klima ?? false;
                vm.EvOzellikleri.Balkon = vm.EvOzellikleri.Balkon ?? false;
                vm.EvOzellikleri.EsyaliMi = vm.EvOzellikleri.EsyaliMi ?? false;
            }
            // Arsa özellikleri için null deðerleri false'a çevir (boolean deðerler için)
            // Arsa özellikleri nesnesi varsa
            if (vm.ArsaOzellikleri != null)
            {
                // Arsa özelliklerinde null olan boolean deðerleri false'a çevir
                vm.ArsaOzellikleri.ElektrikVar = vm.ArsaOzellikleri.ElektrikVar ?? false;
                vm.ArsaOzellikleri.SuVar = vm.ArsaOzellikleri.SuVar ?? false;
                vm.ArsaOzellikleri.DogalgazVar = vm.ArsaOzellikleri.DogalgazVar ?? false;
                vm.ArsaOzellikleri.YolVar = vm.ArsaOzellikleri.YolVar ?? false;
            }
            // Ýþyeri özellikleri için null deðerleri false'a çevir (boolean deðerler için)
            // Ýþyeri özellikleri nesnesi varsa
            if (vm.IsyeriOzellikleri != null)
            {
                // Ýþyeri özelliklerinde null olan boolean deðerleri false'a çevir
                vm.IsyeriOzellikleri.Asansor = vm.IsyeriOzellikleri.Asansor ?? false;
                vm.IsyeriOzellikleri.Klima = vm.IsyeriOzellikleri.Klima ?? false;
                vm.IsyeriOzellikleri.Balkon = vm.IsyeriOzellikleri.Balkon ?? false;
                vm.IsyeriOzellikleri.Tuvalet = vm.IsyeriOzellikleri.Tuvalet ?? false;
                vm.IsyeriOzellikleri.Otopark = vm.IsyeriOzellikleri.Otopark ?? false;
                vm.IsyeriOzellikleri.Guvenlik = vm.IsyeriOzellikleri.Guvenlik ?? false;
                vm.IsyeriOzellikleri.Kapici = vm.IsyeriOzellikleri.Kapici ?? false;
                vm.IsyeriOzellikleri.Depo = vm.IsyeriOzellikleri.Depo ?? false;
            }

            ViewBag.Kategoriler = _context.TblKategori.ToList();

            return View(vm);
        }

        // ÝLAN DÜZENLE POST - Ýlan düzenleme iþlemini gerçekleþtiren metot
        // Ýlan düzenleme iþlemini gerçekleþtiren metot - HTTP POST iþlemi için kullanýlýr
        // CSRF saldýrýlarýna karþý koruma saðlayan ValidateAntiForgeryToken özniteliði
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult IlanDuzenle(IlanDetayViewModel vm)
        {
            ViewBag.Islemler = _context.TblIslem
                .Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = i.IlanIslemId.ToString(),
                    Text = i.IslemAd
                }).ToList();
            // Resim alanlarýný model doðrulamasýndan çýkar
            ModelState.Remove("Ilan.IlanVResim"); // Vitrin resmi alanýný çýkar
            ModelState.Remove("Resim"); // Resim alanýný çýkar
            ModelState.Remove("DetayResimleri"); // Detay resimleri alanýný çýkar

            if (!ModelState.IsValid)
            {
                ViewBag.Kategoriler = _context.TblKategori.ToList();
                return View(vm);
            }

            try
            {
                var ilan = _context.TblIlan
                    .Include(i => i.TblEvOzellikleri)
                    .Include(i => i.TblArsaOzellikleri)
                    .Include(i => i.TblÝsyeri)
                    .Include(i => i.TblResimm)
                    .FirstOrDefault(i => i.IlanId == vm.Ilan.IlanId);

                if (ilan == null)
                    return NotFound();

                // Ana ilan bilgileri
                ilan.IlanBaslik = vm.Ilan.IlanBaslik;
                ilan.IlanFiyat = vm.Ilan.IlanFiyat;
                ilan.IlanTelefon = vm.Ilan.IlanTelefon;
                ilan.IlanAciklama = vm.Ilan.IlanAciklama;
                ilan.IlanIslemId = vm.Ilan.IlanIslemId;
                ilan.IlanKategoriId = vm.Ilan.IlanKategoriId;

                // Vitrin Resmi Yükleme (sadece yeni resim yüklendiyse)
                if (vm.Resim != null && vm.Resim.Length > 0)
                {
                    var imagesDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                    if (!Directory.Exists(imagesDir))
                        Directory.CreateDirectory(imagesDir);

                    var dosyaAdi = Guid.NewGuid().ToString() + Path.GetExtension(vm.Resim.FileName);
                    var yol = Path.Combine(imagesDir, dosyaAdi);

                    using (var stream = new FileStream(yol, FileMode.Create))
                    {
                        vm.Resim.CopyTo(stream);
                    }

                    ilan.IlanVResim = "/images/" + dosyaAdi;
                }

                // Özellik güncelleme
                if (ilan.IlanKategoriId == 1 && vm.EvOzellikleri != null) // Ev
                {
                    if (ilan.TblEvOzellikleri != null)
                    {
                        var evOzellikleri = ilan.TblEvOzellikleri;
                        evOzellikleri.Asansor = vm.EvOzellikleri.Asansor;
                        evOzellikleri.Somine = vm.EvOzellikleri.Somine;
                        evOzellikleri.MobilyaTakimi = vm.EvOzellikleri.MobilyaTakimi;
                        evOzellikleri.DusKabini = vm.EvOzellikleri.DusKabini;
                        evOzellikleri.Klima = vm.EvOzellikleri.Klima;
                        evOzellikleri.Balkon = vm.EvOzellikleri.Balkon;
                        evOzellikleri.EsyaliMi = vm.EvOzellikleri.EsyaliMi;
                        evOzellikleri.Jakuzi = vm.EvOzellikleri.Jakuzi;
                        evOzellikleri.Otopark = vm.EvOzellikleri.Otopark;
                        evOzellikleri.OyunParki = vm.EvOzellikleri.OyunParki;
                        evOzellikleri.Guvenlik = vm.EvOzellikleri.Guvenlik;
                        evOzellikleri.Kapici = vm.EvOzellikleri.Kapici;
                        evOzellikleri.YuzmeHavuzu = vm.EvOzellikleri.YuzmeHavuzu;
                        evOzellikleri.SporSalonu = vm.EvOzellikleri.SporSalonu;
                        evOzellikleri.SiteÝcerisindeMi = vm.EvOzellikleri.SiteÝcerisindeMi;
                        evOzellikleri.OdaSayisi = vm.EvOzellikleri.OdaSayisi;
                        evOzellikleri.SalonSayisi = vm.EvOzellikleri.SalonSayisi;
                        evOzellikleri.BinaYasi = vm.EvOzellikleri.BinaYasi;
                        evOzellikleri.BulunduguKat = vm.EvOzellikleri.BulunduguKat;
                        evOzellikleri.KatSayisi = vm.EvOzellikleri.KatSayisi;
                        evOzellikleri.IsitmaTuru = vm.EvOzellikleri.IsitmaTuru;
                        evOzellikleri.Metrekare = vm.EvOzellikleri.Metrekare;
                    }
                    else
                    {
                        vm.EvOzellikleri.IlanId = ilan.IlanId;
                        _context.TblEvOzellikleri.Add(vm.EvOzellikleri);
                    }
                }
                else if (ilan.IlanKategoriId == 2 && vm.IsyeriOzellikleri != null) // Ýþyeri
                {
                    if (ilan.TblÝsyeri != null)
                    {
                        var isyeri = ilan.TblÝsyeri;
                        isyeri.Asansor = vm.IsyeriOzellikleri.Asansor;
                        isyeri.Klima = vm.IsyeriOzellikleri.Klima;
                        isyeri.Balkon = vm.IsyeriOzellikleri.Balkon;
                        isyeri.Tuvalet = vm.IsyeriOzellikleri.Tuvalet;
                        isyeri.Otopark = vm.IsyeriOzellikleri.Otopark;
                        isyeri.Guvenlik = vm.IsyeriOzellikleri.Guvenlik;
                        isyeri.Kapici = vm.IsyeriOzellikleri.Kapici;
                        isyeri.Depo = vm.IsyeriOzellikleri.Depo;
                        isyeri.Metrekare = vm.IsyeriOzellikleri.Metrekare;
                        isyeri.BinaYasi = vm.IsyeriOzellikleri.BinaYasi;
                        isyeri.BulunduguKat = vm.IsyeriOzellikleri.BulunduguKat;
                        isyeri.KatSayisi = vm.IsyeriOzellikleri.KatSayisi;
                        isyeri.IsitmaTuru = vm.IsyeriOzellikleri.IsitmaTuru;
                        isyeri.KullanimAmaci = vm.IsyeriOzellikleri.KullanimAmaci;
                    }
                    else
                    {
                        vm.IsyeriOzellikleri.IlanId = ilan.IlanId;
                        _context.TblÝsyeri.Add(vm.IsyeriOzellikleri);
                    }
                }
                else if (ilan.IlanKategoriId == 3 && vm.ArsaOzellikleri != null) // Arsa
                {
                    if (ilan.TblArsaOzellikleri != null)
                    {
                        var arsa = ilan.TblArsaOzellikleri;
                        arsa.ElektrikVar = vm.ArsaOzellikleri.ElektrikVar;
                        arsa.SuVar = vm.ArsaOzellikleri.SuVar;
                        arsa.DogalgazVar = vm.ArsaOzellikleri.DogalgazVar;
                        arsa.YolVar = vm.ArsaOzellikleri.YolVar;
                        arsa.Metrekare = vm.ArsaOzellikleri.Metrekare;
                        arsa.MetrekareFiyati = vm.ArsaOzellikleri.MetrekareFiyati;
                        arsa.ImarDurumu = vm.ArsaOzellikleri.ImarDurumu;
                        arsa.TapuDurumu = vm.ArsaOzellikleri.TapuDurumu;
                        arsa.KullanimAmaci = vm.ArsaOzellikleri.KullanimAmaci;
                        arsa.ZeminYapisi = vm.ArsaOzellikleri.ZeminYapisi;
                    }
                    else
                    {
                        vm.ArsaOzellikleri.IlanId = ilan.IlanId;
                        _context.TblArsaOzellikleri.Add(vm.ArsaOzellikleri);
                    }
                }

                // Yeni Detay Resimleri Ekleme
                if (vm.DetayResimleri != null && vm.DetayResimleri.Count > 0)
                {
                    var imagesDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                    foreach (var detayResim in vm.DetayResimleri)
                    {
                        if (detayResim != null && detayResim.Length > 0)
                        {
                            var detayDosyaAdi = Guid.NewGuid().ToString() + Path.GetExtension(detayResim.FileName);
                            var detayYol = Path.Combine(imagesDir, detayDosyaAdi);

                            using (var stream = new FileStream(detayYol, FileMode.Create))
                            {
                                detayResim.CopyTo(stream);
                            }

                            var resimEntity = new TblResimm
                            {
                                ResimAd = detayResim.FileName,
                                ResimYolu = "/images/" + detayDosyaAdi,
                                IlanId = ilan.IlanId
                            };
                            _context.TblResimm.Add(resimEntity);
                        }
                    }
                }

                _context.SaveChanges();
                return RedirectToAction("Ilanlarim");
            }
            catch (Exception ex)
            {
                ViewBag.Islemler = _context.TblIslem
                    .Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                    {
                        Value = i.IlanIslemId.ToString(),
                        Text = i.IslemAd
                    }).ToList();
                ViewBag.Kategoriler = _context.TblKategori.ToList();
                ModelState.AddModelError("", "Güncelleme sýrasýnda bir hata oluþtu: " + ex.Message);
                return View(vm);
            }
        }

        // ÝLAN SÝL - Ýlan silme iþlemini gerçekleþtiren metot
        // Ýlan silme iþlemini gerçekleþtiren metot - ID parametresi ile silinecek ilaný belirler
        public IActionResult IlanSil(int id)
        {
            var ilan = _context.TblIlan
                .Include(i => i.TblResimm)
                .Include(i => i.TblEvOzellikleri)
                .Include(i => i.TblArsaOzellikleri)
                .Include(i => i.TblÝsyeri)
                .FirstOrDefault(i => i.IlanId == id);

            if (ilan == null)
                return NotFound();

            // Ýlana ait tüm resimleri sil
            // Resimler varsa ve en az bir tane mevcutsa
            if (ilan.TblResimm != null && ilan.TblResimm.Any())
                // Tüm resimleri toplu olarak sil
                _context.TblResimm.RemoveRange(ilan.TblResimm);

            // Ýlana ait ev özelliklerini sil (varsa)
            if (ilan.TblEvOzellikleri != null)
                // Ev özelliklerini veritabanýndan kaldýr
                _context.TblEvOzellikleri.Remove(ilan.TblEvOzellikleri);

            // Ýlana ait arsa özelliklerini sil (varsa)
            if (ilan.TblArsaOzellikleri != null)
                // Arsa özelliklerini veritabanýndan kaldýr
                _context.TblArsaOzellikleri.Remove(ilan.TblArsaOzellikleri);

            // Ýlana ait iþyeri özelliklerini sil (varsa)
            if (ilan.TblÝsyeri != null)
                // Ýþyeri özelliklerini veritabanýndan kaldýr
                _context.TblÝsyeri.Remove(ilan.TblÝsyeri);

            // Ýliþkili tüm veriler silindikten sonra ana ilan kaydýný sil
            _context.TblIlan.Remove(ilan);

            _context.SaveChanges();

            return RedirectToAction("Ilanlarim");
        }

        // Ýlana ait bir resmi silmek için kullanýlan metot - HTTP POST iþlemi için kullanýlýr
        [HttpPost]
        public IActionResult ResimSil(int resimId)
        {
            try
            {
                var resim = _context.TblResimm.Find(resimId);
                if (resim != null)
                {
                    // Fiziksel dosyayý diskten sil
                    // Tam dosya yolunu oluþtur (wwwroot klasöründeki yol)
                    var dosyaYolu = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", resim.ResimYolu.TrimStart('/'));
                    // Dosya gerçekten varsa sil
                    if (System.IO.File.Exists(dosyaYolu))
                    {
                        // Dosyayý diskten sil
                        System.IO.File.Delete(dosyaYolu);
                    }

                    // Resim kaydýný veritabanýndan sil
                    _context.TblResimm.Remove(resim);
                    // Deðiþiklikleri kaydet
                    _context.SaveChanges();

                    return Json(new { success = true });
                }
                // Resim bulunamadýðýnda hata mesajýný JSON olarak döndür
                return Json(new { success = false, message = "Resim bulunamadý." });
            }
            // Herhangi bir hata durumunda
            catch (Exception ex)
            {
                // Hata mesajýný JSON olarak döndür
                return Json(new { success = false, message = ex.Message });
            }
        }

        // Kategoriye göre filtrelenmiþ ilanlarý listeleyen metot
        public IActionResult FilteredIndex(int kategoriId)
        {
            var ilanlar = _context.TblIlan
                .Include(i => i.TblKategori)
                .Include(i => i.TblIslem)
                .Include(i => i.TblResimm)
                .Where(i => i.IlanKategoriId == kategoriId)
                .OrderByDescending(i => i.IlanTarih)
                .ToList();
            ViewBag.Kategoriler = _context.TblKategori.ToList();
            return View("Index", ilanlar);
        }

        // Ýlan talebi göndermek için kullanýlan metot - HTTP POST iþlemi için kullanýlýr
        [HttpPost]
        public IActionResult IlanIstekGonder(IlanIstek model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                    return Json(new { success = false, message = "Validasyon hatasý: " + errors });
                }

                // Ýlan nesnesini kontrol et
                var ilan = _context.TblIlan.Find(model.ilanId);
                if (ilan == null)
                {
                    return Json(new { success = false, message = "Ýlan bulunamadý." });
                }

                model.IstekTarihi = DateTime.Now;
                model.Okundu = false;

                try
                {
                    _context.IlanIstekler.Add(model);
                    _context.SaveChanges();
                    return Json(new { success = true });
                }
                catch (DbUpdateException dbEx)
                {
                    var innerException = dbEx.InnerException;
                    var errorMessage = "Veritabaný hatasý: ";

                    while (innerException != null)
                    {
                        errorMessage += innerException.Message + " ";
                        innerException = innerException.InnerException;
                    }

                    _logger.LogError($"Ýlan isteði kaydedilirken hata: {errorMessage}");
                    return Json(new { success = false, message = errorMessage });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ýlan isteði gönderilirken hata: {ex.Message}");
                return Json(new { success = false, message = "Bir hata oluþtu: " + ex.Message });
            }
        }

        // Tüm ilan isteklerini listeleyen metot
        public IActionResult IlanIstekleri()
        {
            var istekler = _context.IlanIstekler
                .OrderByDescending(i => i.IstekTarihi)
                .ToList();
            return View(istekler);
        }

        // Ýsteðin okundu olarak iþaretlenmesi için kullanýlan metot - HTTP POST iþlemi için kullanýlýr
        [HttpPost]
        public IActionResult IstekOkundu(int id)
        {
            var istek = _context.IlanIstekler.Find(id);
            if (istek != null)
            {
                istek.Okundu = true;
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        // Ýlan isteði detayýný gösteren metot
        public IActionResult IlanIstekDetay(int istekId)
        {
            var istek = _context.IlanIstekler.FirstOrDefault(x => x.IstekId == istekId);
            if (istek == null)
                return NotFound();

            var ilan = _context.TblIlan
                .Include(i => i.TblKategori)
                .Include(i => i.TblIslem)
                .Include(i => i.TblEvOzellikleri)
                .Include(i => i.TblArsaOzellikleri)
                .Include(i => i.TblÝsyeri)
                .Include(i => i.TblResimm)
                .FirstOrDefault(i => i.IlanId == istek.ilanId);

            if (ilan == null)
                return NotFound();

            var vm = new IlanIstekDetayViewModel
            {
                Ilan = ilan,
                IlanIstek = istek
            };
            return View(vm);
        }

        // Ýlan isteðini silmek için kullanýlan metot - HTTP POST iþlemi için kullanýlýr
        [HttpPost]
        public IActionResult IstekSil(int id)
        {
            try
            {
                var istek = _context.IlanIstekler.Find(id);
                if (istek == null)
                {
                    return Json(new { success = false, message = "Silinecek istek bulunamadý." });
                }

                _context.IlanIstekler.Remove(istek);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ýlan isteði silinirken hata: {ex.Message}");
                return Json(new { success = false, message = "Silme iþlemi sýrasýnda bir hata oluþtu: " + ex.Message });
            }
        }

        // Hata sayfasýný gösteren metot - Hata sayfasýnýn önbelleðe alýnmamasýný saðlar
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // Hata sayfasýný hata bilgileriyle birlikte gösterir
            // Activity.Current?.Id ?? HttpContext.TraceIdentifier ile mevcut istek için benzersiz bir kimlik oluþturur
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}